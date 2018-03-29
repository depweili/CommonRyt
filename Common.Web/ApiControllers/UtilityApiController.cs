using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using Common.Util;
using Common.Services;
using Common.Services.Dtos;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Common.Web.ApiControllers
{
    
    public class UtilityApiController : ApiControllerBase
    {
        /// <summary>
        /// 微信用户授权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iv"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        [Route("api/WxUserInfo")]
        [HttpGet]
        public IHttpActionResult GetWxUser(string code, string iv, string encryptedData)
        {
            WxService ws = new WxService();

            dynamic res = ws.GetWxUser(code, iv, encryptedData);

            return Ok(res);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="Register"></param>
        /// <returns></returns>
        [Route("api/Register")]
        [HttpPost]
        public IHttpActionResult RegisterUser(RegisterDto Register)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                dynamic data = ws.RegisterUser(Register);

                if (data == null)
                {
                    throw new Exception("注册失败");
                }

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginDto login)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                dynamic data = ws.Login(login);

                if (data == null)
                {
                    throw new Exception("登陆失败");
                }

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        [Route("api/Image/{img}")]
        [HttpGet]
        public HttpResponseMessage GetImage(string img)
        {
            try
            {
                string key = "Image_" + img;
                byte[] imgByte;

                if (CacheHelper.Exist(key))
                {
                    imgByte = CacheHelper.Get<byte[]>(key);
                }
                else
                {
                    var service = new UtilityService();

                    string mime;

                    imgByte = service.GetImageByEncrypt(img, out mime);

                    CacheHelper.Set(key, imgByte, DateTime.Now.AddMinutes(_cacheabsoluteminutes));
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(imgByte)
                };
                
                return resp;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return resp;
            }
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("api/QrCode/{content}")]
        [HttpGet]
        public HttpResponseMessage GetQrCode(string content)
        {
            try
            {
                var service = new UtilityService();

                var imgByte = service.GetGetQrCode(content);

                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return resp;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return resp;
            }
        }

        
        [Route("api/Test/UploadImages")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImages(string content)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var path = Function.GetImageDirectory("Test");
            var provider = new ReNameMultipartFormDataStreamProvider(path, content);

            List<string> files = new List<string>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(Path.GetFileName(file.LocalFileName));
                }

                // Send OK Response along with saved file names to the client.  
                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return resp;
            }
        }

        


        [Route("api/Test/UploadImages1")]
        [HttpPost]
        public HttpResponseMessage UploadImages1()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                var httpRequest = HttpContext.Current.Request;

                var sss = httpRequest.Form.AllKeys;

                if (httpRequest.Files.Count > 0)
                {

                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile == null) continue;
                        var filePath = HttpContext.Current.Server.MapPath("~/images/Test/" + postedFile.FileName);
                        //先删除
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        postedFile.SaveAs(filePath);
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                //在webapi中要想抛出异常必须这样抛出，否则之抛出一个默认500的异常
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);
            }
        }

    }
}
