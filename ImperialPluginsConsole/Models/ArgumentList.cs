using System;
using System.Collections.Generic;

namespace ImperialPluginsConsole.Models
{
    public class ArgumentList : Dictionary<string, string>
    {
        /// <summary>
        /// Throws <see cref="MissingArgumentException"/> if an argument is missing
        /// </summary>
        public void Enforce(params string[] ars)
        {
            foreach (var arg in ars)
            {
                if (!ContainsKey(arg))
                {
                    throw new MissingArgumentException(arg);
                }
            }
        }

        public bool If(string key) => ContainsKey(key);

        public T GetOrDefault<T>(string key, T def, bool throwOnParseFail = false)
        {
            if (ContainsKey(key))
            {
                var val = this[key];
                if (StringParser.TryParse<T>(val, out var res) && res != null)
                {
                    return res;
                }
            }

            if (throwOnParseFail)
            {
                throw new InvalidCastException();
            }

            return def;
        }
    }
}