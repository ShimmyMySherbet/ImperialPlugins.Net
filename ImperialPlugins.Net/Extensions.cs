using System.IO;
using System.Net;

namespace ImperialPlugins
{
    public static class Extensions
    {
        public static void WriteString(this HttpWebRequest request, string content, string type = null)
        {
            if (type != null) request.ContentType = type;
            using (MemoryStream buffer = new MemoryStream())
            using (StreamWriter wr = new StreamWriter(buffer))
            {
                wr.Write(content);
                wr.Flush();
                request.ContentLength = buffer.Length;
                buffer.Position = 0;
                using (Stream network = request.GetRequestStream())
                {
                    buffer.CopyTo(network);
                    network.Flush();
                }
            }
        }

        public static HttpWebResponse GetResponseHTTP(this HttpWebRequest req) => (HttpWebResponse)req.GetResponse();

        public static string ReadString(this HttpWebRequest req, out HttpWebResponse response)
        {
            using (MemoryStream reads = new MemoryStream())
            {
                response = req.GetResponseHTTP();
                using (Stream network = response.GetResponseStream())
                {
                    network.CopyTo(reads);
                    network.Flush();
                }

                reads.Position = 0;

                using (StreamReader reader = new StreamReader(reads))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}