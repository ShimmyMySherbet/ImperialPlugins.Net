using ImperialPluginsConsole.Servicing.Interfaces;
using ImperialPluginsConsole.Servicing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ImperialPluginsConsole.Servicing
{
    public class ServiceHost : IServiceProvider, IServiceContainer, IActivator
    {
        private List<TransientService> m_TransientTypes = new List<TransientService>();
        private List<SingletonService> m_Singletons = new List<SingletonService>();
        private List<TransientService> m_NewSingletons = new List<TransientService>();
        private List<HostedService> m_HostedServices = new List<HostedService>();

        private List<Type> m_NewHostedServices = new List<Type>();

        private bool m_IsRunning { get; set; } = false;

        public ServiceHost()
        {
            // Register Self
            m_Singletons.Add(new SingletonService(GetType(), GetType(), this));
        }

        public int HostedServices
        {
            get
            {
                return m_HostedServices.Count + m_NewHostedServices.Count;
            }
        }

        public object GetService(Type serviceType)
        {
            return GetService(serviceType, false);
        }

        public object GetService(Type serviceType, bool implemntationTypeStrict)
        {
            TransientService? transient;
            lock (m_TransientTypes)
            {
                if (implemntationTypeStrict)
                {
                    transient = m_TransientTypes.FirstOrDefault(x => serviceType.IsAssignableFrom(x.ImplementationType));
                }
                else
                {
                    transient = m_TransientTypes.FirstOrDefault(x => serviceType.IsAssignableFrom(x.SpecifiedType));
                }
            }
            if (transient is not null)
            {
                return Activate(transient.ImplementationType);
            }

            SingletonService? singleton;
            lock (m_Singletons)
            {
                singleton = m_Singletons.FirstOrDefault(x => x.Matches(serviceType));
            }

            if (singleton != null)
            {
                return singleton.Instance;
            }

            TransientService? newSingleton;
            lock (m_NewSingletons)
            {
                newSingleton = m_NewSingletons.FirstOrDefault(x => serviceType.IsAssignableFrom(x.ImplementationType));
            }

            if (newSingleton is not null)
            {
                lock (m_NewSingletons)
                    m_NewSingletons.RemoveAll(x => x == newSingleton);

                var newInstance = Activate(newSingleton.ImplementationType);
                var instanceStore = new SingletonService(newSingleton.ImplementationType, newSingleton.SpecifiedType, newInstance);
                lock (m_Singletons)
                    m_Singletons.Add(instanceStore);

                return newInstance;
            }

            HostedService? service;
            lock (m_HostedServices)
            {
                service = m_HostedServices.FirstOrDefault(x => x.ServiceType == serviceType);
            }

            if (service != null)
            {
                return service.Instance;
            }

            Type? newService;

            lock (m_NewHostedServices)
            {
                newService = m_NewHostedServices.FirstOrDefault(x => x == serviceType);
            }

            if (newService != null && typeof(IHostedService).IsAssignableFrom(newService))
            {
                lock (m_NewHostedServices)
                    m_NewHostedServices.RemoveAll(x => x == newService);

                var instance = Activate(newService);

                var hosted = new HostedService(newService, (IHostedService)instance);

                lock (m_HostedServices)
                    m_HostedServices.Add(hosted);

                return hosted.Instance;
            }

            if (!implemntationTypeStrict)
            {
                return GetService(serviceType, true);
            }

            throw new InvalidOperationException($"Cannot find matching service for type {serviceType.Name}");
        }

        private bool HasType(Type t)
        {

            lock (m_TransientTypes)
            {
                if (m_TransientTypes.Any(x => t.IsAssignableFrom(x.ImplementationType)))
                    return true;
            }

            lock (m_Singletons)
            {
                if (m_Singletons.Any(x => t.IsAssignableFrom(x.ImplementationType)))
                    return true;
            }

            lock(m_HostedServices)
            {
                if (m_HostedServices.Any(x => t.IsAssignableFrom(x.ServiceType)))
                    return true;
            }

            lock (m_NewHostedServices)
            {
                if (m_NewHostedServices.Any(x => t.IsAssignableFrom(x)))
                    return true;
            }

            lock (m_NewSingletons)
            {
                if (m_NewSingletons.Any(x => t.IsAssignableFrom(x.ImplementationType)))
                    return true;
            }

            return false;
        }

        public object Activate(Type t)
        {
            return ActivateType(t);
        }

        public object ActivateType(Type t, params object[] parameters)
        {
            var constructors = t.GetConstructors();
            var validConstructors = new List<ConstructorInfo>();
            foreach (var c in constructors)
            {
                var param = c.GetParameters();

                bool canActivate = true;

                foreach (var p in param)
                {
                    var hasType = HasType(p.ParameterType);
                    var extraHasType = parameters.Any(x => p.ParameterType.IsAssignableFrom(x.GetType()));

                    if (!hasType && !extraHasType)
                    {
                        canActivate = false;
                        break;
                    }
                }


                if (canActivate)
                {
                    validConstructors.Add(c);
                }
            }


            var ordered = validConstructors.OrderByDescending(x => x.GetParameters().Length).ToArray();

     

            if (ordered.Length == 0)
            {
                throw new InvalidOperationException($"Cannot find a valid constructor for type {t.Name}");
            }

            var constructor = ordered[0];

            var paramInstances = new List<object>();

            foreach (var p in constructor.GetParameters())
            {
                var argP = parameters.FirstOrDefault(x => p.ParameterType.IsAssignableFrom(x.GetType()));

                if (argP != null)
                {
                    paramInstances.Add(argP);
                }
                else
                {
                    paramInstances.Add(GetService(p.ParameterType));
                }
            }

            var instance = Activator.CreateInstance(t, args: paramInstances.ToArray());
            if (instance == null)
            {
                throw new InvalidOperationException($"Failed to activate {t.Name}");
            }

            return instance;
        }

        public void RegisterSingleton<I, T>() where T : I
        {
            RegisterSingleton(typeof(T), typeof(I));
        }

        public void RegisterTransient<I, T>() where T : I
        {
            RegisterTransient(typeof(T), typeof(I));
        }

        public void RegisterSingleton<T>()
        {
            RegisterSingleton(typeof(T));
        }

        public void RegisterTransient<T>()
        {
            RegisterTransient(typeof(T));
        }

        public void RegisterHostedService<T>() where T : IHostedService
        {
            RegisterHostedService(typeof(T));
        }

        public void RegisterHostedService(Type t)
        {
            if (!typeof(IHostedService).IsAssignableFrom(t))
            {
                throw new InvalidCastException($"Type {t.Name} does not implement IHostedService");
            }

            lock (m_NewHostedServices)
                m_NewHostedServices.Add(t);
        }

        public void RegisterSingleton(Type t, Type? specified = null)
        {
            lock (m_NewSingletons)
                m_NewSingletons.Add(new TransientService(specified != null ? specified : t, t));
        }

        public void RegisterTransient(Type t, Type? specified = null)
        {
            lock (m_TransientTypes)
                m_TransientTypes.Add(new TransientService(specified != null ? specified : t, t));
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            AddService(serviceType, callback, false);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            if (promote)
            {
                lock (m_TransientTypes)
                    m_TransientTypes.Insert(0, new TransientService(serviceType, serviceType));
            }
            else
            {
                lock (m_TransientTypes)
                    m_TransientTypes.Add(new TransientService(serviceType, serviceType));
            }

            callback.Invoke(this, serviceType);
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            AddService(serviceType, serviceInstance, false);
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            var singletonContainer = new SingletonService(serviceInstance.GetType(), serviceType, serviceInstance);
            if (promote)
            {
                lock (m_Singletons)
                    m_Singletons.Insert(0, singletonContainer);
            }
            else
            {
                lock (m_Singletons)
                    m_Singletons.Add(singletonContainer);
            }
        }

        public void RemoveService(Type serviceType)
        {
            RemoveService(serviceType, false);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            lock (m_TransientTypes)
                m_TransientTypes.RemoveAll(x => x.SpecifiedType == serviceType);

            lock (m_NewSingletons)
                m_NewSingletons.RemoveAll(x => x.SpecifiedType == serviceType);

            lock (m_Singletons)
                m_Singletons.RemoveAll(x => x.ImplementationType == serviceType);
        }

        public T Resolve<T>()
        {
            return (T)GetService(typeof(T));
        }

        public T Activate<T>()
        {
            return (T)Activate(typeof(T));
        }

        public T ActivateType<T>(params object[] arguments)
        {
            return (T)ActivateType(typeof(T), arguments);
        }

        private IHostedService? GetHostedService(int index)
        {
            int i = index;
            lock (m_NewHostedServices)
            {
                if (m_HostedServices.Count - 1 >= i)
                {
                    return m_HostedServices[index].Instance;
                }
                i -= m_HostedServices.Count;
            }

            lock (m_NewHostedServices)
            {
                if (m_NewHostedServices.Count - 1 >= i)
                {
                    var type = m_NewHostedServices[i];
                    m_NewHostedServices.RemoveAt(i);

                    var instance = Activate(type);

                    var container = new HostedService(type, (IHostedService)instance);

                    lock (m_HostedServices)
                    {
                        m_HostedServices.Add(container);
                    }

                    return (IHostedService)instance;
                }
            }

            return null;
        }

        public void Start()
        {
            if (m_IsRunning)
            {
                throw new InvalidOperationException("Service is already running.");
            }
            m_IsRunning = true;

            int i = 0;
            while (true)
            {
                var service = GetHostedService(i);
                if (service != null)
                {
                    service.Start();
                    i++;
                }
                else
                {
                    break;
                }
            }
        }

        public void Stop()
        {
            IHostedService[] services;
            lock (m_HostedServices)
                services = m_HostedServices.Select(x => x.Instance).ToArray();

            foreach (var service in services)
            {
                service.Stop();
            }
        }

        public void Run()
        {
            try
            {
                Start();
            }
            finally
            {
                Stop();
            }
        }
    }
}