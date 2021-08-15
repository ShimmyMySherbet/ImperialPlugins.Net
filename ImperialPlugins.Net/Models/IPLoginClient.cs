using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using ImperialPlugins.Crypto;

namespace ImperialPlugins.Models
{
    public class IPLoginClient
    {
        private string GeneratedURL;
        private string LoginURL;
        public bool HasLoginURL => !string.IsNullOrEmpty(GeneratedURL);
        private ImperialPluginsClient Client;

        private string m_Session;
        private string m_Ex;
        private string m_Tab;

        public string m_sessionState;
        public string m_idToken;
        public string m_accessToken;
        public string m_tokenType;

        private string m_state;

        internal IPLoginClient(ImperialPluginsClient client)
        {
            Client = client;
        }

        internal void RetriveLoginURL()
        {
            HttpWebRequest request = Client.CreateWebRequest(GeneratedURL, "GET");

            string html = request.ReadString(out HttpWebResponse response);


            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var node = document.DocumentNode.SelectSingleNode("//form[@id='kc-form-login']");

            if (node != null)
            {
                var act = node.GetAttributeValue("action", null);

                if (act != null)
                {
                    act = act.Replace("&amp;", "&");
                    LoginURL = act;
                    var parameters = Client.URIHelper.GetURLParameters(LoginURL);
                    m_Session = parameters["session_code"];
                    m_Ex = parameters["execution"];
                    m_Tab = parameters["tab_id"];

                }
            }
        }


        public bool Login(string APIKey)
        {
            return Client.LoginCallback(new IPSessionCredentials(APIKey));
        }


        public bool Login(string username, string password)
        {
            m_state = CryptoHelper.GetRandomString(60);
            GeneratedURL = Client.URIHelper.GenerateLoginURL(m_state);
            LoginURL = GeneratedURL;
            Console.WriteLine(LoginURL);
            string content = $"username={WebUtility.UrlEncode(username)}&password={WebUtility.UrlEncode(password)}";

            HttpWebRequest request = Client.CreateWebRequest(LoginURL, "POST");
            request.WriteString(content, "application/x-www-form-urlencoded");

            HttpWebResponse response = request.GetResponseHTTP();
            if (response.StatusCode == HttpStatusCode.Unauthorized) return false;

            string uri = response.ResponseUri.ToString();

            Dictionary<string, string> parameters = Client.URIHelper.GetURLParameters(uri);
            if (parameters.Count < 4) return false;


            m_sessionState = parameters["session_state"];

            m_idToken = parameters["id_token"];
            m_accessToken = parameters["access_token"];
            m_tokenType = parameters["token_type"];

            return Client.LoginCallback(new IPSessionCredentials(m_tokenType, m_accessToken));
        }
    }
}