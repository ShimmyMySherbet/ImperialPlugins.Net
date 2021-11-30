namespace ImperialPluginsConsole.Models
{
    public static class StringParser
    {
        public static bool TryParse<T>(string value, out T? result)
        {
            result = default(T);
            var t = typeof(T);
            if (t == typeof(sbyte))
            {
                if (sbyte.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(byte))
            {
                if (byte.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(short))
            {
                if (short.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(ushort))
            {
                if (short.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(int))
            {
                if (int.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(uint))
            {
                if (uint.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(long))
            {
                if (long.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(ulong))
            {
                if (ulong.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(float))
            {
                if (float.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(double))
            {
                if (double.TryParse(value, out var r))
                {
                    result = (T)(object)r;
                    return true;
                }
            }
            else if (t == typeof(string))
            {
                result = (T)(object)value;
                return true;
            }
            return false;
        }
    }
}