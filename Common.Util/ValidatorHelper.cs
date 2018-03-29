using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Util
{
    public class ValidatorHelper
    {
        private static Regex _mobileregex = new Regex("^(0|86|17951)?(13[0-9]|15[012356789]|17[013678]|18[0-9]|14[57])[0-9]{8}$");

        public static bool IsMobile(string s)
        {
            return _mobileregex.IsMatch(s);

        }
    }
}
