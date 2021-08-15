using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ImperialPlugins.Models.Helpers
{
    public class URIHelper
    {
        public const string Pattern_LoginURL = "https://id.imperialplugins.com/auth/realms/master/protocol/openid-connect/auth?response_type=id_token%20token&client_id=shopcore-spa&state={0}&redirect_uri=https%3A%2F%2Fimperialplugins.com%2Fsignin-oidc&scope=openid%20profile%20email&nonce={0}";

        /// <summary>
        /// Generates a login URL
        /// </summary>
        /// <param name="nonce">60 character nonce</param>
        /// <returns></returns>
        public string GenerateLoginURL(string nonce)
        {
            return string.Format(Pattern_LoginURL, nonce);
        }

        public Dictionary<string, string> GetURLParameters(string url)
        {
            // Parameters start can be ? or #

            char baseSplit = '?';

            if (!url.Contains(baseSplit))
            {
                if (url.Contains('#'))
                {
                    baseSplit = '#';
                } else
                {
                    return new Dictionary<string, string>();
                }
            }

            string bse = url.Substring(url.IndexOf(baseSplit) + 1);

            Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var att in bse.Split('&'))
            {
                if (att.IndexOf('=') != -1)
                {
                    string key = att.Split('=')[0];
                    string value = att.Substring(key.Length + 1);

                    key = WebUtility.UrlDecode(key);
                    value = WebUtility.UrlDecode(value);
                    parameters.Add(key, value);
                }
                else
                {
                    parameters.Add(WebUtility.UrlDecode(att), null);
                }
            }

            return parameters;
        }
    }
}