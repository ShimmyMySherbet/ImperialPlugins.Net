using ImperialPlugins;
using ImperialPlugins.Models;
using ImperialPlugins.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ImperialPluginsConsole.Models
{
    public class CacheClient
    {
        protected readonly ImperialPluginsClient m_ImperialPlugins;

        private EnumerableResponse<PluginRegistration>? m_Registrations;
        private EnumerableResponse<IPUser>? m_Users;
        private EnumerableResponse<IPPlugin>? m_Plugins;

        private CacheWaitHandle m_WaitHandle = new CacheWaitHandle();

        public CacheClient(ImperialPluginsClient imperialPlugins)
        {
            m_ImperialPlugins = imperialPlugins;
        }

        public virtual void RefreshReg()
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                if (m_Registrations == null)
                {
                    m_Registrations = new EnumerableResponse<PluginRegistration>();
                }
                lock (m_Registrations)
                {
                    try
                    {
                        m_Registrations = m_ImperialPlugins.GetRegistrations(100000);
                        Debug.WriteLine($"Loaded {m_Registrations.TotalCount} registrations");
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        public virtual void StartInit()
        {
            var regHandle = m_WaitHandle.CreateTask();
            var userHandle = m_WaitHandle.CreateTask();
            var pluginHandle = m_WaitHandle.CreateTask();

            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    m_Registrations = m_ImperialPlugins.GetRegistrations(100000);
                    Debug.WriteLine($"Loaded {m_Registrations.TotalCount} registrations");
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    regHandle.SendComplete();
                }
            });

            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    m_Users = m_ImperialPlugins.GetUsers(100000);
                    Debug.WriteLine($"Loaded {m_Users.TotalCount} users");
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    userHandle.SendComplete();
                }
            });

            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    m_Plugins = m_ImperialPlugins.GetPlugins(1000);
                    Debug.WriteLine($"Loaded {m_Plugins.TotalCount} plugins");
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    pluginHandle.SendComplete();
                }
            });

            m_WaitHandle.Activate();
        }

        public virtual PluginRegistration? GetRegistration(int ID)
        {
            var reg = GetRegistrations(2000);
            return reg.Items.FirstOrDefault(x => x.ID == ID);
        }

        public virtual EnumerableResponse<PluginRegistration> GetRegistrations(int max = 20, bool refresh = false)
        {
            if (m_Registrations == null)
            {
                m_WaitHandle.Wait();
            }

            if (m_Registrations == null)
            {
                return m_ImperialPlugins.GetRegistrations(max);
            }

            if (refresh)
            {
                m_Registrations = m_ImperialPlugins.GetRegistrations(1000);
            }

            lock (m_Registrations)
                return new EnumerableResponse<PluginRegistration>()
                {
                    TotalCount = m_Registrations.TotalCount,
                    ImperialPlugins = m_ImperialPlugins,
                    Items = m_Registrations.Items.Limit(max).ToArray()
                };
        }

        public virtual EnumerableResponse<IPUser> GetUsers(int max = 20)
        {
            if (m_Users == null)
            {
                m_WaitHandle.Wait();
            }

            if (m_Users == null)
            {
                return m_ImperialPlugins.GetUsers(max);
            }

            lock (m_Users)
                return new EnumerableResponse<IPUser>()
                {
                    TotalCount = m_Users.TotalCount,
                    ImperialPlugins = m_ImperialPlugins,
                    Items = m_Users.Items.Take(max).ToArray()
                };
        }

        public virtual EnumerableResponse<IPPlugin> GetPlugins(int max = 20)
        {
            if (m_Plugins == null)
            {
                m_WaitHandle.Wait();
            }

            if (m_Plugins == null)
            {
                return m_ImperialPlugins.GetPlugins(max);
            }

            lock (m_Plugins)
                return new EnumerableResponse<IPPlugin>()
                {
                    TotalCount = m_Plugins.TotalCount,
                    ImperialPlugins = m_ImperialPlugins,
                    Items = m_Plugins.Items.Take(max).ToArray()
                };
        }

        public virtual IPUser? GetUser(string userHandle)
        {
            if (m_Users == null)
            {
                m_WaitHandle.Wait();
            }
            if (m_Users == null) return null;

            IPUser? usr = null;

            lock (m_Users)
            {
                usr = m_Users.Items.FirstOrDefault(x => x.Id == userHandle || x.Email.Equals(userHandle, StringComparison.InvariantCultureIgnoreCase) || x.UserName.Equals(userHandle, StringComparison.InvariantCultureIgnoreCase));
            }
            return usr;
        }

        public virtual IPPlugin? GetPlugin(int id)
        {
            if (m_Plugins == null)
            {
                m_WaitHandle.Wait();
            }
            if (m_Plugins == null) return null;

            IPPlugin? usr = null;

            lock (m_Plugins)
            {
                usr = m_Plugins.Items.FirstOrDefault(x => x.ID == id);
            }

            return usr;
        }

        public virtual IPPlugin? GetPluginByName(string name)
        {
            if (m_Plugins == null)
            {
                m_WaitHandle.Wait();
            }
            if (m_Plugins == null) return null;

            IPPlugin? usr = null;

            lock (m_Plugins)
            {
                usr = m_Plugins.Items.FirstOrDefault(x => x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            }

            return usr;
        }

        public virtual List<IPPlugin> GetSelfPlugins() => GetMerchantPlugins(m_ImperialPlugins.Self.ID);

        public virtual List<IPPlugin> GetMerchantPlugins(string merchantID)
        {
            if (m_Plugins == null)
            {
                m_WaitHandle.Wait();
            }
            if (m_Plugins == null) return new List<IPPlugin>();

            lock (m_Plugins)
            {
                return m_Plugins.Items.Where(x => x.Merchant.ID == merchantID).ToList();
            }
        }
    }
}