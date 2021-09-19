using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ImperialPlugins.Models.Har
{
    public static class HarAuthManager
    {
        public static IPSessionCredentials? ExtractCredentialsFromHAR(Stream stream)
        {
            string json;
            using (var reader = new StreamReader(stream))
                json = reader.ReadToEnd();

            var model = HarContent.FromJson(json);

            string authValue = null;

            foreach (var request in model.Log.Entries)
            {
                var authHeader = request.Request.Headers.FirstOrDefault(x => x.Name.Equals("authorization", StringComparison.InvariantCultureIgnoreCase));

                if (authHeader != null)
                {
                    if (authValue == null || authHeader.Value.Length > authValue.Length)
                    {
                        authValue = authHeader.Value;
                    }
                }
            }

            if (authValue != null)
            {
                var token = new IPSessionCredentials()
                {
                    Header = "authorization",
                    AuthHeaderContent = authValue
                };


                return token;
            } else
            {
                Console.WriteLine("Failed to find auth header");
            }

            return null;
        }
    }
}