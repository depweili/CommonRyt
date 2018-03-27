using Common.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace Common.Services
{
    public static class Function
    {
        public static Random random = new Random();

        public static string PathToRelativeUrl(string path)
        {
            string root = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string newPath = path.Replace(root, ""); //转换成相对路径
            newPath = newPath.Replace(@"\", @"/");
            return newPath;
        }

        public static string RelativeUrlToLocal(string url)
        {
            string root = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string path = root + url.Replace(@"/", @"\"); //转换成绝对路径
            return path;
        }


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

        public static string GetStaticPicUrlHost(string subdir = null)
        {
            var host = GetHostAndApp();
            var url = host + @"/Images/";
            if (!string.IsNullOrEmpty(subdir))
            {
                url += subdir + "/";
            }
            return url;
        }

        public static string GetImageStorageDirectory(string subdir = null,DateTime dateTime=default(DateTime))
        {
            DateTime distribDate = (dateTime == default(DateTime) ? DateTime.Now.Date : dateTime);

            var imgDir = ConfigHelper.GetConfig("ImagesPhysicalPath");

            if (string.IsNullOrEmpty(imgDir))
            {
                imgDir = HttpContext.Current.Server.MapPath("~/Images") + @"\";
            }

            if (!string.IsNullOrEmpty(subdir))
            {
                imgDir = imgDir  + subdir + @"\";
            }

            imgDir = imgDir + distribDate.Year + @"\" + distribDate.Month + @"\" + distribDate.Day + @"\";

            if (!Directory.Exists(imgDir))
            {
                Directory.CreateDirectory(imgDir);
            }

            return imgDir;
        }

        public static string GetImageDirectory(string subdir=null)
        {
            var imgDir = ConfigHelper.GetConfig("ImagesPhysicalPath");

            if (string.IsNullOrEmpty(imgDir))
            {
                imgDir = HttpContext.Current.Server.MapPath("~/Images") + @"\";
            }

            if (!string.IsNullOrEmpty(subdir))
            {
                imgDir = imgDir  + subdir + @"\";
            }

            if (!Directory.Exists(imgDir))
            {
                Directory.CreateDirectory(imgDir);
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

        public static IEnumerable<T> GetPageData<T>(IQueryable<T> query, JObject queryParam)
        {
            int pageNum = queryParam["pageNum"].IsEmpty() ? 1 : queryParam["pageNum"].ToString().ToInt();
            int pageSize = queryParam["pageSize"].IsEmpty() ? 20 : queryParam["pageSize"].ToString().ToInt();

            return query.Skip(pageSize * (pageNum - 1)).Take(pageSize);
        }
        

        public static async Task<List<string>> SaveImagesAsync(HttpRequestMessage request,string subdir, string namekey,DateTime createtime)
        {
            var path = Function.GetImageStorageDirectory(subdir, createtime);
            var provider = new ReNameMultipartFormDataStreamProvider(path, namekey);

            List<string> files = new List<string>();

            try
            {
                await request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    //files.Add(Path.GetFileName(file.LocalFileName));
                    files.Add(file.LocalFileName);
                }

                // Send OK Response along with saved file names to the client.  
                //return Request.CreateResponse(HttpStatusCode.OK, files);
                return files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
