using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class MD5Encrypt
    {
        public static string GetStrMD5(string str)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] bt = md.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.Append(bt[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
