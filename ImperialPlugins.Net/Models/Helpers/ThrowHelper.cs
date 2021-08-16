using ImperialPlugins.Models.ErrorHandling;
using ImperialPlugins.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace ImperialPlugins.Models.Helpers
{
    public class ThrowHelper
    {
        private ImperialPluginsClient Client;

        public ThrowHelper(ImperialPluginsClient client)
        {
            Client = client;
        }

        public void ThrowIfNotLoggedIn()
        {
            if (!Client.IsLoggedIn)
            {
                throw new NotLoggedInException();
            }
        }



        public void ThrowIfIpEx(WebException ex)
        {
            var resp = ex.Response;
            if (resp == null) return;
            
            if (!resp.ContentType.Contains("application/json"))
            {
                return;
            }

            try
            {
                using(var network = resp.GetResponseStream())
                using (var reader = new StreamReader(network))
                {
                    var err = JsonConvert.DeserializeObject<IPErrorResponse>(reader.ReadToEnd());
                    if (err.Error != null)
                    {
                        Debug.WriteLine("Throwing IP Error");
                        throw new ImperialPluginsException(err.Error, ex);
                    }
                }
            }
            catch(ImperialPluginsException ipe)
            {
                throw ipe;
            } catch(Exception)
            {
                //ignore all other errors
            }
     
        }

    }
}