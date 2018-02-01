using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Services
{
    public static class Function
    {
        public static Random random = new Random();

        public static string GetHostAndApp()
        {

            var scheme = HttpContext.Current.Request.Url.Scheme;
            var authority = HttpContext.Current.Request.Url.Authority;
            var applicationPath = HttpContext.Current.Request.ApplicationPath;

            var url = scheme + "://" + authority + (applicationPath == "/" ? "" : applicationPath);

            return url;
        }

        public static string GetPicUrl(string pic)
        {
            try
            {
                var host = GetHostAndApp();
                //Server.MapPath();
                //var url = host + @"/SxcWebApi/api/Image/" + Cryptography.Base64ForUrlEncode(pic);
                var url = host + @"/api/Image/" + Base64DEncrypt.Base64ForUrlEncode(pic);
                //return @"http://192.168.31.199/SxcWebApi/api/Image/" + Cryptography.Base64ForUrlEncode(pic);
                return url;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string GetStaticPicUrl(string pic, string subdir = null)
        {
            var host = GetHostAndApp();
            var url = host + @"/Images/";
            if (!string.IsNullOrEmpty(subdir))
            {
                url += subdir + "/";
            }
            url += pic;

            return url;
        }

        public static string GetImageDirectory()
        {
            var imgDir = ConfigHelper.GetConfig("ImagesPhysicalPath");

            if (string.IsNullOrEmpty(imgDir))
            {
                imgDir = HttpContext.Current.Server.MapPath("~/Images") + @"\";
            }

            return imgDir;
        }

        public static string GetImagePath(string img)
        {
            var imgDir = GetImageDirectory();

            var imgPath = imgDir + img;

            return imgPath;
        }


    }
}
