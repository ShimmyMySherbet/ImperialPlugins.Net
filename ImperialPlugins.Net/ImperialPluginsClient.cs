using System;
using System.Net;
using System.Threading.Tasks;
using ImperialPlugins.Models;
using ImperialPlugins.Models.API;
using ImperialPlugins.Models.Coupons;
using ImperialPlugins.Models.Exceptions;
using ImperialPlugins.Models.Files;
using ImperialPlugins.Models.Helpers;
using ImperialPlugins.Models.Internals;
using ImperialPlugins.Models.Notifications;
using ImperialPlugins.Models.Plugins;
using ImperialPlugins.Models.Reviews;
using ImperialPlugins.Models.Servers;
using ImperialPlugins.Models.Service;
using ImperialPlugins.Models.Users;
using Newtonsoft.Json;

namespace ImperialPlugins
{
    public class ImperialPluginsClient
    {
        private CookieContainer CookieContainer = new CookieContainer();
        public URIHelper URIHelper = new URIHelper();
        public NotificationTypeParseClient NotificationTypeParseClient = new NotificationTypeParseClient();
        public ThrowHelper ThrowHelper;

        public string UserAgent { get; set; } = "ImperialPlugins.net client (v1.0)";

        public IPSession Session { get; private set; }
        public Merchant Self => Session == null ? null : Session.Merchant;

        public bool IsLoggedIn { get; private set; } = false;
        public IPSessionCredentials SessionCredentials { get; private set; }

        public event AuthenticatedArgs Authenticated;

        public ImperialPluginsClient()
        {
            ThrowHelper = new ThrowHelper(this);
        }

        public IPLoginClient CreateLogin()
        {
            IPLoginClient client = new IPLoginClient(this);
            return client;
        }

        /// <summary>
        /// Logs in using existing credentials.
        /// To login using a username and password, see CreateLogin()
        /// </summary>
        public bool Login(IPSessionCredentials credentials)
        {
            SessionCredentials = credentials;
            try
            {
                IsLoggedIn = true;
                Session = GetSession();
                Authenticated?.Invoke(this, credentials);
            }
            catch (WebException)
            {
                IsLoggedIn = false;
                return false;
            }
            return true;
        }

        internal bool LoginCallback(IPSessionCredentials credentials)
        {
            try
            {
                IsLoggedIn = true;
                SessionCredentials = credentials;
                Session = GetSession();
                Authenticated?.Invoke(this, credentials);
                return true;
            }
            catch (ImperialPluginsException ex)
            {
                Console.WriteLine($"IPError: {ex.Message} {string.Join(",", ex.Errors)}");
                IsLoggedIn = false;
                return false;
            }
            catch (WebException ex)
            {
                Console.WriteLine($"WebEx: {ex.Message}");
                IsLoggedIn = false;
                return false;
            }
        }

        internal HttpWebRequest CreateWebRequest(string uri, string method)
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = method;
            request.CookieContainer = CookieContainer;

            if (IsLoggedIn)
            {
                request.Headers.Add("access-control-request-headers", "authorization");
                request.Headers.Add("access-control-request-method", "GET");
                request.Headers.Add("origin", "https://imperialplugins.com");

                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-site", "same-site");
                request.Headers.Add("sec-gpc", "1");
                request.Headers.Add("origin", "val");
                request.Headers.Add("origin", "val");
                request.Headers.Add(SessionCredentials.Header, SessionCredentials.AuthHeaderContent);
                request.Headers.Add("dnt", "1");
                request.Referer = "https://imperialplugins.com";
            }

            if (!string.IsNullOrEmpty(UserAgent))
            {
                request.UserAgent = UserAgent;
            }
            return request;
        }

        /// <summary>
        /// Retrieves an ordered list of all registrations
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public EnumerableResponse<PluginRegistration> GetRegistrations(int max = 5) => BasicAPICall<EnumerableResponse<PluginRegistration>>($"Products/Registrations?MerchantId={Self.ID}&Sorting=id%20DESC&MaxResultCount={max}");

        public EnumerableResponse<PluginCategory> GetPluginCategories(int max = 5) => BasicAPICall<EnumerableResponse<PluginCategory>>($"/Products/Categories&MaxResultCount={max}");

        public EnumerableResponse<Coupon> GetCoupons(int max = 5) => BasicAPICall<EnumerableResponse<Coupon>>($"/Coupons?MaxResultCount={max}");

        public async Task<EnumerableResponse<Coupon>> GetCouponsAsync(int max = 5) => await BasicAPICallAsync<EnumerableResponse<Coupon>>($"/Coupons?MaxResultCount={max}");

        public PluginCategory GetPluginCategory(int categoryID) => BasicAPICall<PluginCategory>($"/Products/Categories/{categoryID}");

        public void CreateCoupon(CouponBuilder coupon) => BasicAPIOperation("Coupons", "POST", coupon);

        public async Task CreateCouponAsync(CouponBuilder coupon) => await BasicAPIOperationAsync("Coupons", "POST", coupon);

        [Obsolete]
        public void DeleteCoupon(int couponID) => BasicAPIOperation($"Coupons/{couponID}", "DELETE");

        public async Task DeleteCouponAsync(int couponID) => await BasicAPIOperationAsync($"Coupons/{couponID}", "DELETE");

        public Coupon GetCoupon(string couponKey) => BasicAPICall<Coupon>($"Coupons/{couponKey}");

        public async Task<Coupon> GetCouponAsync(string couponKey) => await BasicAPICallAsync<Coupon>($"Coupons/{couponKey}");

        public EnumerableResponse<IPFile> GetFiles(int max = 5) => BasicAPICall<EnumerableResponse<IPFile>>($"/Products/Files?MaxResultCount={max}");

        public void UpdateFileChangelog(int fileID, string changelog, bool forceUpdate)
        {
            var model = new t_UpdateChangelog() { forceUpdate = forceUpdate, markdownChangelog = changelog };
            BasicAPIOperation($"Products/Files/{fileID}", "PUT", model);
        }

        public Ticket GetTicketByID(int ID) => BasicAPICall<Ticket>($"tickets/Entries?TicketId={ID}");

        public EnumerableResponse<IPNotification> GetNotifications(int max = 5) => BasicAPICall<EnumerableResponse<IPNotification>>($"Notifications?NotificationStatus=Any&Sorting=creationTime%20DESC&SkipCount=0&MaxResultCount={max}");

        public RefundVerifyToken RefundProduct(string licenceKey) => new RefundVerifyToken(this, licenceKey);

        public void UnblockProduct(string licenceKey) => BasicAPIOperation($"/Products/Registrations/Unblock?licenseKey={licenceKey}", "POST", new object());

        public void BlockProduct(string licenceKey, string reason) => BasicAPIOperation($"/Products/Registrations/Block?licenseKey={licenceKey}&reason={WebUtility.UrlEncode(reason)}", "POST", new object());

        public PluginRegistration GetPluginRegistration(int id) => BasicAPICall<PluginRegistration>($"/Products/Registrations/{id}");

        public MerchantRating GetMerchantRating(string id) => BasicAPICall<MerchantRating>($"/Products/Reviews/GetMerchantRating/{id}");

        public PluginReview GetPluginReview(string id) => BasicAPICall<PluginReview>($"/Products/Reviews/{id}");

        public EnumerableResponse<PluginReview> GetPluginReviews() => BasicAPICall<EnumerableResponse<PluginReview>>($"/Products/Reviews");

        public MerchantResponse RespondToReview(string reviewID, string response)
        {
            t_MerchantResponse r = new t_MerchantResponse() { markdownContent = response };
            return BasicAPIOperation<t_MerchantResponse, MerchantResponse>($"/Products/Reviews/{reviewID}/MerchantResponse", "PUT", r);
        }

        public void UpdatePluginRegistration(int id, string licencekey, DateTime? expires, bool isactive)
        {
            var val = new t_UpdatePluginRegistration() { expireTime = expires, isActive = isactive, licenseKey = licencekey };
            BasicAPIOperation($"/Products/Registrations/{id}", "PUT", val);
        }

        public Merchant GetMerchant(string id) => BasicAPICall<Merchant>($"/Merchants/{id}");

        public MerchantDashboard GetDashboardStats() => BasicAPICall<MerchantDashboard>($"/Dashboard");

        public EnumerableResponse<Merchant> GetMerchants(int max = 1000) => BasicAPICall<EnumerableResponse<Merchant>>($"/Merchants?MaxResultCount={max}");

        public EnumerableResponse<Server> GetCustomerServers() => BasicAPICall<EnumerableResponse<Server>>("/Dashboard​/CustomerServers");

        public EnumerableResponse<ProductInstallation> GetInstallations(string customerID, int max = 100) => BasicAPICall<EnumerableResponse<ProductInstallation>>($"/Products/Installations?CustomerId={WebUtility.UrlEncode(customerID)}&MaxResultCount={max}");

        public EnumerableResponse<APIKey> GetAPIKeys() => BasicAPICall<EnumerableResponse<APIKey>>("/ApiKeys");

        public APIKey CreateAPIKey(string name)
        {
            t_ApiKeyModel key = new t_ApiKeyModel() { name = name };
            return BasicAPIOperation<t_ApiKeyModel, APIKey>("/ApiKeys", "POST", key);
        }

        public EnumerableResponse<IPPlugin> GetPlugins(int max = 100) => BasicAPICall<EnumerableResponse<IPPlugin>>($"/Products?MaxResultCount={max}");

        public EnumerableResponse<IPPlugin> GetMerchantPlugins(string merchantID, int max = 100) => BasicAPICall<EnumerableResponse<IPPlugin>>($"/Products?MaxResultCount={max}&MerchantIds={WebUtility.UrlEncode(merchantID)}");

        public async Task<EnumerableResponse<IPPlugin>> GetMerchantPluginsAsync(string merchantID, int max = 100) => await BasicAPICallAsync<EnumerableResponse<IPPlugin>>($"/Products?MaxResultCount={max}&MerchantIds={WebUtility.UrlEncode(merchantID)}");

        public EnumerableResponse<IPPlugin> GetOwnPlugins(int max = 100) => GetMerchantPlugins(Session.UserID, max);

        public Task<EnumerableResponse<IPPlugin>> GetOwnPluginsAsync(int max = 100) => GetMerchantPluginsAsync(Session.UserID, max);

        public APIKey RenameAPIkey(int id, string name)
        {
            t_ApiKeyModel key = new t_ApiKeyModel() { name = name };
            return BasicAPIOperation<t_ApiKeyModel, APIKey>($"/ApiKeys/{id}", "PUT", key);
        }

        public void DeleteAPIKey(int id) => BasicAPIOperation($"/ApiKeys/{id}", "DELETE");

        [Obsolete]
        public EnumerableResponse<PluginRegistration> SearchRegistrations(PluginSearchQuery query)
        {
            return null;
        }

        public IPUser GetUser(string id) => BasicAPICall<IPUser>($"Merchant/Customers/{id}");

        public EnumerableResponse<IPUser> GetUsers(int max = 100, string query = null) => BasicAPICall<EnumerableResponse<IPUser>>($"Merchant/Customers?MaxResultCount={max}{(query != null ? $"Filter={WebUtility.UrlEncode(query)}" : "")}");

        public PluginWhitelist WhitelistPlugin(int installationID, bool whitelist) => BasicAPICall<PluginWhitelist>($"/Merchant/Whitelist/WhitelistProductInstallation?productInstallationId]{installationID}&isWhitelisted={whitelist}");

        public PluginWhitelist[] WhitelistServer(string host, bool whitelist, int port = -1) => BasicAPICall<PluginWhitelist[]>($"Merchant/Whitelist/WhitelistServer?host={WebUtility.UrlEncode(host)}&isWhitelisted={whitelist}{(port != -1 ? $"&port={port}" : "")}");

        public void UploadFile(FileUpload file) => BasicAPIOperation("Products/Files", "POST", file);

        public void UpdatePlugin(int pluginID, string branch, string version, string changelog, byte[] fileData, string fileName, bool forceUpdate = false)
        {
            var f = new FileUpload()
            {
                DisplayVersion = version,
                ForceUpdate = forceUpdate,
                MarkdownChangelog = changelog,
                ProductId = pluginID,
                ProductBranchIdentifier = branch,
                File = new IPFileData()
            };

            f.File.FileName = fileName;
            f.File.Base64 = "data:application/x-zip-compressed;base64," + Convert.ToBase64String(fileData);

            BasicAPIOperation("Products/Files", "POST", f);
        }

        public async Task UpdatePluginAsync(int pluginID, string branch, string version, string changelog, byte[] fileData, string fileName, bool forceUpdate = false)
        {
            var f = new FileUpload()
            {
                DisplayVersion = version,
                ForceUpdate = forceUpdate,
                MarkdownChangelog = changelog,
                ProductId = pluginID,
                ProductBranchIdentifier = branch,
                File = new IPFileData()
            };

            f.File.FileName = fileName;
            f.File.Base64 = "data:application/x-zip-compressed;base64," + Convert.ToBase64String(fileData);

            BasicAPIOperation("Products/Files", "POST", f);
        }

        public IPFileData DownloadFile(int fileID, string branch = null) => BasicAPICall<IPFileData>($"Products/Files/Download/{fileID}{(branch != null ? $"?BranchKey={branch}" : "")}");

        public IPFileInfo GetFileInfo(int fileID, EChangelogType type, string branch = null) => BasicAPICall<IPFileInfo>($"Products/Files/{fileID}?changelogType={type}{(branch != null ? $"branchKey={branch}" : "")}");

        /// <summary>
        /// Gets details about the merchant wallet, including balance and commission rate.
        /// </summary>
        public MerchantWallet GetWallet() => BasicAPICall<MerchantWallet>("Payments/MerchantWallet");

        /// <summary>
        /// The client automatically gets the session details on login.
        /// Try to use ImperialPluginsClient.Session instead to reduce requests.
        /// </summary>
        /// <returns>Current login session</returns>
        public IPSession GetSession()
        {
            Session = BasicAPICall<IPSession>("Session");
            return Session;
        }

        /// <summary>
        /// Gets the number of unread notifications.
        /// Returns -1 on error
        /// </summary>
        public int UnreadMessages()
        {
            if (int.TryParse(BasicAPICall<string>("Notifications/UnreadCount"), out int count))
                return count;
            else
                return -1;
        }

        public T BasicAPICall<T>(string endpoint)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);
            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, "GET");
                string jsval = request.ReadString(out _);
                if (typeof(T) == typeof(NoReturn)) return default;
                if (jsval is T c)
                    return c;
                else
                {
                    T r = JsonConvert.DeserializeObject<T>(jsval);
                    if (r is IPObject obj)
                    {
                        obj.ImperialPlugins = this;
                    }
                    return r;
                }
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        public async Task<T> BasicAPICallAsync<T>(string endpoint)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);
            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, "GET");
                (string jsval, HttpWebResponse resp) = await request.ReadStringAsync();
                if (typeof(T) == typeof(NoReturn)) return default;
                if (jsval is T c)
                    return c;
                else
                {
                    T r = JsonConvert.DeserializeObject<T>(jsval);
                    if (r is IPObject obj)
                    {
                        obj.ImperialPlugins = this;
                    }
                    return r;
                }
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        public void BasicAPIOperation(string endpoint, string method)
        {
            try
            {
                ThrowHelper.ThrowIfNotLoggedIn();
                EnsureEndpoint(ref endpoint);
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                request.GetResponse();
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        public async Task BasicAPIOperationAsync(string endpoint, string method)
        {
            try
            {
                ThrowHelper.ThrowIfNotLoggedIn();
                EnsureEndpoint(ref endpoint);
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                await request.GetResponseAsync();
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        public void BasicAPIOperation<T>(string endpoint, string method, T Payload)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);
            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                string payload;
                string type;
                if (Payload is string pl)
                {
                    type = "plain/text";
                    payload = pl;
                }
                else
                {
                    type = "application/json";
                    payload = JsonConvert.SerializeObject(Payload);
                }
                request.WriteString(payload, type);
                request.GetResponse();
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        public async Task BasicAPIOperationAsync<T>(string endpoint, string method, T Payload)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);
            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                string payload;
                string type;
                if (Payload is string pl)
                {
                    type = "plain/text";
                    payload = pl;
                }
                else
                {
                    type = "application/json";
                    payload = JsonConvert.SerializeObject(Payload);
                }
                await request.WriteStringAsync(payload, type);
                await request.GetResponseAsync();
            }
            catch (WebException wex)
            {
                ThrowHelper.ThrowIfIpEx(wex);
                throw wex;
            }
        }

        //public async Task BasicAPIOperationAsync<T>(string endpoint, string method, T Payload)
        //{
        //    ThrowHelper.ThrowIfNotLoggedIn();
        //    EnsureEndpoint(ref endpoint);
        //    try
        //    {
        //        HttpWebRequest request = CreateWebRequest(endpoint, method);
        //        string payload;
        //        string type;
        //        if (Payload is string pl)
        //        {
        //            type = "plain/text";
        //            payload = pl;
        //        }
        //        else
        //        {
        //            type = "application/json";
        //            payload = JsonConvert.SerializeObject(Payload);
        //        }
        //        request.WriteString(payload, type);
        //        await request.GetResponseAsync();
        //    }
        //    catch (WebException wex)
        //    {
        //        ThrowHelper.ThrowIfIpEx(wex);
        //        throw wex;
        //    }
        //}

        public O BasicAPIOperation<T, O>(string endpoint, string method, T Payload)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);

            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                string payload;
                string type;
                if (Payload is string pl)
                {
                    type = "plain/text";
                    payload = pl;
                }
                else
                {
                    type = "application/json";
                    payload = JsonConvert.SerializeObject(Payload);
                }
                request.WriteString(payload, type);
                string response = request.ReadString(out _);

                if (response is O t)
                {
                    return t;
                }
                {
                    O r = JsonConvert.DeserializeObject<O>(response);
                    if (r is IPObject obj)
                    {
                        obj.ImperialPlugins = this;
                    }
                    return r;
                }
            }
            catch (WebException ex)
            {
                ThrowHelper.ThrowIfIpEx(ex);
                throw ex;
            }
        }

        public async Task<O> BasicAPIOperationAsync<T, O>(string endpoint, string method, T Payload)
        {
            ThrowHelper.ThrowIfNotLoggedIn();
            EnsureEndpoint(ref endpoint);

            try
            {
                HttpWebRequest request = CreateWebRequest(endpoint, method);
                string payload;
                string type;
                if (Payload is string pl)
                {
                    type = "plain/text";
                    payload = pl;
                }
                else
                {
                    type = "application/json";
                    payload = JsonConvert.SerializeObject(Payload);
                }
                request.WriteString(payload, type);
                (string response, HttpWebResponse resp) = await request.ReadStringAsync();

                if (response is O t)
                {
                    return t;
                }
                {
                    O r = JsonConvert.DeserializeObject<O>(response);
                    if (r is IPObject obj)
                    {
                        obj.ImperialPlugins = this;
                    }
                    return r;
                }
            }
            catch (WebException ex)
            {
                ThrowHelper.ThrowIfIpEx(ex);
                throw ex;
            }
        }

        public void EnsureEndpoint(ref string endpoint)
        {
            if (Uri.TryCreate(endpoint, UriKind.RelativeOrAbsolute, out Uri res))
            {
                if (!res.IsAbsoluteUri)
                {
                    endpoint = $"https://api.imperialplugins.com/v2/{endpoint.TrimStart('/')}";
                }
            }
        }
    }
}