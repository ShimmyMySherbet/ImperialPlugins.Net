using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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

        public static int GetPadBase<T>(this IEnumerable<T> e, Func<T, int> func)
        {
            return e.Max(func);
        }

        public static string Pad(this string input, int padTo)
        {
            var inlen = 0;
            var b = new StringBuilder();
            if (input != null)
            {
                inlen = input.Length;
                b.Append(input);
            }

            var paddingNeeded = padTo - inlen;

            if (paddingNeeded <= 0)
            {
                return b.ToString();
            }

            for (int i = 0; i < paddingNeeded; i++)
            {
                b.Append(' ');
            }
            return b.ToString();
        }

        public static string Pad(this char padChar, int filled, int padTo)
        {
            var b = new StringBuilder();

            var paddingNeeded = padTo - filled;

            if (paddingNeeded <= 0)
            {
                return b.ToString();
            }

            for (int i = 0; i < paddingNeeded; i++)
            {
                b.Append(' ');
            }
            return b.ToString();
        }


        public static IEnumerable<T> Limit<T>(this IEnumerable<T> inp, int limit)
        {
            var ot = new List<T>();
            var took = 0;
            using(var e = inp.GetEnumerator())
            {
                while(e.MoveNext())
                {
                    took++;
                    ot.Add(e.Current);
                    if (took >= limit)
                    {
                        return ot;
                    }
                }
            }
            return ot;
        }
    }
}