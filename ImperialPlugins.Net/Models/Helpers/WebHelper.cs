using System.IO;
using System.Net;

namespace ImperialPlugins.Models.Helpers
{
    public static class WebHelper
    {
        public static void WriteString(this HttpWebRequest request, string content, string type = null)
        {
            if (type != null)
            {
                request.ContentType = type;
            }

            using (MemoryStream buffer = new MemoryStream())
            using (StreamWriter writes = new StreamWriter(buffer))
            {
                writes.Write(content);
                writes.Flush();


                request.ContentLength = buffer.Length;
                buffer.Position = 0;

                using(Stream network = request.GetRequestStream())
                {
                    buffer.CopyTo(network);
                    network.Flush();
                }
            }
        }

        public static HttpWebResponse GetResponseHTTP(this HttpWebRequest request) => (HttpWebResponse)request.GetResponse();

        public static string ReadString(this HttpWebRequest request, out HttpWebResponse response)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

                using (Stream network = resp.GetResponseStream())
                {
                    network.CopyTo(buffer);
                    network.Flush();
                    buffer.Position = 0;
                    using (StreamReader reader = new StreamReader(buffer))
                    {
                        response = resp;
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}