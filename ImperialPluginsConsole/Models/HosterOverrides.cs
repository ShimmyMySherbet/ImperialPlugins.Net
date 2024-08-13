using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Models
{
    public static class HosterOverrides
    {
        private static readonly Dictionary<string, string> m_HosterOverrides = new Dictionary<string, string>()
        {
            { "modern-hosting.com", "Modern Hosting" },
            { "lyhmehosting.com", "Lyhme Hosting" },
            { "lyhme.io", "Lyhme Hosting" },
            { "pinehosting.com", "Pine Hosting" }
        };

        public static string? GetResellerName(string IP)
        {
            try
            {
                var IPAddr = IPAddress.Parse(IP);
                var DNSEntry = Dns.GetHostEntry(IPAddr);
                if (string.IsNullOrEmpty(DNSEntry.HostName))
                {
                    return null;
                }
                var tld = GetTLD(DNSEntry.HostName);
                if (tld == null) // shits fucked
                {
                    return null;
                }

                if (m_HosterOverrides.TryGetValue(tld, out var reseller))
                {
                    return reseller;
                }
                return null;
            }
            catch (SocketException) // DNS Lookup Failed
            {
                return null;
            }
        }

        public static async Task<string?> GetResellerNameAsync(string IP)
        {
            try
            {
                var IPAddr = IPAddress.Parse(IP);
                var DNSEntry = await Dns.GetHostEntryAsync(IPAddr);
                if (string.IsNullOrEmpty(DNSEntry.HostName))
                {
                    return null;
                }
                var tld = GetTLD(DNSEntry.HostName);
                if (tld == null) // shits fucked
                {
                    return null;
                }

                if (m_HosterOverrides.TryGetValue(tld, out var reseller))
                {
                    return reseller;
                }
                return null;
            }
            catch (SocketException) // DNS Lookup Failed
            {
                return null;
            }
        }

        private static string? GetTLD(string domain)
        {
            var parts = domain.Split(".");
            if (parts.Length < 2) // something has gone wrong
            {
                return null;
            }
            var partsLen = parts.Length;
            return $"{parts[partsLen - 2]}.{parts[partsLen - 1]}".ToLowerInvariant();
        }
    }
}