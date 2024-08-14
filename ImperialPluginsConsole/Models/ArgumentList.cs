using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool If(string key)
        {
            return ContainsKey(key);
        }

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

        public T GetOrThrow<T>(string key)
        {
            if (ContainsKey(key))
            {
                var val = this[key];
                if (StringParser.TryParse<T>(val, out var res) && res != null)
                {
                    return res;
                }
                else
                {
                    throw new InvalidCastException($"'{val}' is not a valid value for argument {key}. Expected type: {typeof(T).Name}.");
                }
            }
            throw new MissingArgumentException(key);
        }

        public T GetOrThrow<T>(string[] keys)
        {
            if (keys.Length == 0)
            {
                throw new ArgumentException("Missing value for argument: keys");
            }

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

                if (ContainsKey(key))
                {
                    var val = this[key];
                    if (StringParser.TryParse<T>(val, out var res) && res != null)
                    {
                        return res;
                    }
                    else
                    {
                        throw new InvalidCastException($"'{val}' is not a valid value for argument {key}. Expected type: {typeof(T).Name}.");
                    }
                }
            }

            throw new MissingArgumentException(keys.First());
        }
    }
}