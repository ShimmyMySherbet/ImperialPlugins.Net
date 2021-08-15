using System;
using System.Security.Cryptography;
using System.Text;

namespace ImperialPlugins.Crypto
{
    public static class CryptoHelper
    {
        public static readonly string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I",
        "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        public static string GetRandomString(int len)
        {
            StringBuilder b = new StringBuilder();
            using (RandomNumberGenerator RNG = RandomNumberGenerator.Create())
            {
                for(int l = 0; l < len; l++)
                {
                    int i = GetRandomInt(RNG, alphabet.Length - 1);
                    b.Append(alphabet[i]);
                }
            }
            return b.ToString();
        }

        public static int GetRandomInt(RandomNumberGenerator gen, int max, bool positive = true)
        {
            byte[] buffer = new byte[4];
            gen.GetBytes(buffer);

            int bse = BitConverter.ToInt32(buffer, 0);

            if (positive)
                bse = Math.Abs(bse);
            return bse % max;
        }
    }
}