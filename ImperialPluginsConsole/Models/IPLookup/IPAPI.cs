using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImperialPluginsConsole.Models.IPLookup
{
    public static class IPAPI
    {
        private const string URL = "http://ip-api.com/batch?fields=status,message,country,regionName,city,isp,org,hosting,query";

        private static Dictionary<string, IPAPIResponse> m_Cached = new Dictionary<string, IPAPIResponse>();

        public static Dictionary<string, IPAPIResponse?> LookupIP(string[] IPs)
        {
            var responses = new Dictionary<string, IPAPIResponse?>();

            foreach (var ip in IPs)
            {
                if (!responses.ContainsKey(ip))
                {
                    if (m_Cached.ContainsKey(ip))
                    {
                        responses[ip] = m_Cached[ip];
                    }
                    else
                    {
                        responses[ip] = null;
                    }
                }
            }

            var lookupIPs = responses.Where(x => x.Value == null).Select(x => x.Key);

            if (lookupIPs.Count() == 0)
            {
                return responses;
            }

            var json = JsonConvert.SerializeObject(lookupIPs);

            var request = WebRequest.CreateHttp(URL);
            request.Method = "POST";
            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                request.ContentLength = jsonStream.Length;
                request.ContentType = "application/json";
                using (var network = request.GetRequestStream())
                {
                    jsonStream.CopyTo(network);
                    network.Flush();
                }
            }
            var response = request.GetResponse();
            using (var network = response.GetResponseStream())
            using (var netReader = new StreamReader(network))
            {
                var responseJson = netReader.ReadToEnd();
                var resp = JsonConvert.DeserializeObject<IPAPIResponse[]>(responseJson);

                if (resp != null)
                {
                    foreach (var r in resp)
                    {
                        responses[r.Query] = r;
                        m_Cached[r.Query] = r;
                    }
                }
            }

            return responses;
        }
    }
}