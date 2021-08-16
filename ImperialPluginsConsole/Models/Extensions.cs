using System;
using System.IO;
using System.Net;

namespace ImperialPluginsConsole.Models
{
    public static class Extensions
    {
        public static string ReadContent(this WebException ex)
        {
            var r = ex.Response;

            if (r == null || r.ContentLength == 0)
            {
                return "No Content";
            }

            if (r.ContentLength > 500)
            {
                return "Content too long";
            }
            try
            {
                using (var network = r.GetResponseStream())
                using (var reader = new StreamReader(network))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "No Content";
            }
        }
    }
}