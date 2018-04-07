using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class StringHelper
    {
        public static string ReplaceWithSpecialChar(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int lenth = value.Length - startLen - endLen;

                    string replaceStr = value.Substring(startLen, lenth);

                    string specialStr = string.Empty;

                    for (int i = 0; i < replaceStr.Length; i++)
                    {
                        specialStr += specialChar;
                    }

                    value = value.Replace(replaceStr, specialStr);
                }

            }
            catch (Exception ex)
            {
                value = "*error*";
            }

            return value;
        }

        //automapper中value如果是数据库null值则无法进入，空则可以
        public static string Nvl(string value, string replace)
        {
            return string.IsNullOrEmpty(value) ? replace : value;
        }

        public static string Nvl2(string value, string replace, string replace2)
        {
            return string.IsNullOrEmpty(value) ? replace2 : replace;
        }
    }
}
