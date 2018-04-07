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
using Common.Services.Dtos;
using Newtonsoft.Json;

namespace Common.Services
{
    public static class Function
    {
        public static Random random = new Random();


        public static string SendSms(SmsDto sms)
        {
            string res = string.Empty;
            try
            {
                string apikey = TripleDESDEncrypt.Decrypt(@"CJwn/xNScEJiG1yux7kXqBg1Kl0Qnt1zREn2v0RLDdjrckSKKHaM0w==");

                string mobile = sms.MobilePhone;

                string text = "【仁医通】您的验证码是" + sms.SmsCode+"，有效期3分钟。如非本人操作请忽略。";

                string url_send_sms = "https://sms.yunpian.com/v2/sms/single_send.json";

                string data_send_sms = "apikey=" + apikey + "&mobile=" + mobile + "&text=" + text;

                WebClientHelper wc = new WebClientHelper();
                wc.Encoding = Encoding.UTF8;

                string resp = wc.Post(url_send_sms, data_send_sms);

                var back = JsonConvert.DeserializeObject<dynamic>(resp);

                if (back.code != "0")
                {
                    res = back.msg;
                }

                
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public static string SendSmsTpl(SmsDto sms)
        {
            string res = string.Empty;
            try
            {
                string apikey = TripleDESDEncrypt.Decrypt(@"CJwn/xNScEJiG1yux7kXqBg1Kl0Qnt1zREn2v0RLDdjrckSKKHaM0w==");

                int tpl_id = 2231786;

                string mobile = sms.MobilePhone;

                //string text = "【仁医通】您的验证码是" + sms.SmsCode + "，有效期3分钟。如非本人操作请忽略。";

                string url_send_sms = "https://sms.yunpian.com/v2/sms/tpl_single_send.json";

                string tpl_value = HttpUtility.UrlEncode(
                    HttpUtility.UrlEncode("#code#", Encoding.UTF8) + "=" + HttpUtility.UrlEncode(sms.SmsCode, Encoding.UTF8), Encoding.UTF8);

                string data_tpl_sms = "apikey=" + apikey + "&mobile=" + mobile +
                "&tpl_id=" + tpl_id.ToString() + "&tpl_value=" + tpl_value;

                WebClientHelper wc = new WebClientHelper();
                wc.Encoding = Encoding.UTF8;

                string resp = wc.Post(url_send_sms, data_tpl_sms);

                var back = JsonConvert.DeserializeObject<dynamic>(resp);

                if (back.code != "0")
                {
                    res = back.msg;
                }


            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

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


        public static List<string> SaveImages(HttpRequest httpRequest, string subdir, string namekey, DateTime createtime,string [] namefilter)
        {
            var path = Function.GetImageStorageDirectory(subdir, createtime);
            List<string> files = new List<string>();

            try
            {
                var index = 1;
                if (namefilter.Length > 0)
                {
                    foreach (var file in httpRequest.Files.AllKeys.Where(t => namefilter.Contains(t)))
                    {
                        var postedFile = httpRequest.Files[file];

                        if (postedFile == null) continue;

                        var newFileName = Path.GetExtension(postedFile.FileName);//获取后缀名  
                        newFileName = namekey + "_" + file + newFileName;
                        var filePath = path + newFileName;
                        //if (File.Exists(filePath))
                        //{
                        //    File.Delete(filePath);
                        //}
                        postedFile.SaveAs(filePath);

                        files.Add(filePath);
                    }
                }
                else
                {
                    //foreach (var file in httpRequest.Files.AllKeys)
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var postedFile = httpRequest.Files[i];
                        if (postedFile == null) continue;

                        var newFileName = Path.GetExtension(postedFile.FileName);//获取后缀名  
                        newFileName = namekey + "_" + index.ToString() + newFileName;
                        var filePath = path + newFileName;
                        index++;
                        //if (File.Exists(filePath))
                        //{
                        //    File.Delete(filePath);
                        //}
                        postedFile.SaveAs(filePath);

                        files.Add(filePath);
                    }
                }
                

                return files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
