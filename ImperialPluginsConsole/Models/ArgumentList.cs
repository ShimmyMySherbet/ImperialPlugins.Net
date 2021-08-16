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
    }
}