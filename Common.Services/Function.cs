using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static string GetDicDesc(string type, int value)
        {
            string res = string.Empty;
            switch (type)
            {
                case "审批":
                    res = EnumHelper.GetDescription((AuditEnum)value);
                    break;
                default:
                    break;

            }
            return res;
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

        public static bool IsInTreeBySql(DbContext db, int cid, int tid, string table, string key = "ID", string pkey = "PID")
        {
            var supsql = string.Format(@"WITH temp AS
                                        (
                                        SELECT {0} FROM {1}  WHERE {0} = {3}
                                        UNION ALL
                                        SELECT d.{0} FROM {1} AS d
                                        INNER JOIN temp ON d.{2} = temp.{0}
                                        )
                                        SELECT * FROM temp", key, table, pkey, cid);

            var list = db.Database.SqlQuery<int>(supsql);

            return list.ToList().Contains(tid);
        }


        public static List<T> GetColumnListByTree<T>(DbContext db, string cid,string table, string key = "ID", string pkey = "PID")
        {
            var supsql = string.Format(@"WITH temp AS
                                        (
                                        SELECT {0} FROM {1}  WHERE {0} = {3}
                                        UNION ALL
                                        SELECT d.{0} FROM {1} AS d
                                        INNER JOIN temp ON d.{2} = temp.{0}
                                        )
                                        SELECT * FROM temp", key, table, pkey, cid);

            var list = db.Database.SqlQuery<T>(supsql);

            return list.ToList();
        }

    }
}
