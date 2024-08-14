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
                using var network = r.GetResponseStream();
                using var reader = new StreamReader(network);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "No Content";
            }
        }

        public static int GetPadBase<T>(this IEnumerable<T> e, Func<T, int> func)
        {
            int mx = -1;

            using (var en = e.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var v = func(en.Current);
                    if (v > mx)
                        mx = v;
                }
            }
            return mx;
        }

        public static bool Check(this string[] args, string key)
        {
            return args.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string Pad(this string input, int padTo, int excess = 0)
        {
            var inlen = 0;
            var b = new StringBuilder();
            if (input != null)
            {
                inlen = input.Length + excess;
                b.Append(input);
            }
            else inlen = excess;

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
            using (var e = inp.GetEnumerator())
            {
                while (e.MoveNext())
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

        /// <summary>
        /// Represents a mapping function, that takes a value as an input, returning an object in <see langword="out"/> field when returning true
        /// </summary>
        /// <typeparam name="I">Input type</typeparam>
        /// <typeparam name="O">Output argument type</typeparam>
        /// <param name="input">Input value</param>
        /// <param name="output">Output value</param>
        /// <returns>True if the conversion was successful, and <paramref name="output"/> was set</returns>
        public delegate bool MappingFunction<I, O>(I input, out O? output);

        /// <summary>
        /// Applies a mapping function over an enumerable, yielding the output of the mapping function when it returns true
        /// </summary>
        /// <remarks>
        /// Like a mix between Select and Where
        /// </remarks>
        /// <typeparam name="I">Input argument type</typeparam>
        /// <typeparam name="O">Output argument type</typeparam>
        /// <param name="values">Input values to enumerate over</param>
        /// <param name="mapper">Mapping function to apply to input values</param>
        /// <returns></returns>
        public static IEnumerable<O> Map<I, O>(this IEnumerable<I> values, MappingFunction<I, O> mapper)
        {
            foreach (var value in values)
            {
                if (mapper(value, out var output) && output != null)
                {
                    yield return output;
                }
            }
        }
    }
}