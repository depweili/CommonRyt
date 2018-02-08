using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class RandomHelper
    {
        private static string randomString = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
        private static Random random = new Random(DateTime.Now.Second);

        public static string GetRandomString(int size)
        {
            char[] chars = new char[62];
            //string a;
            //a = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = randomString.ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
    }
}
