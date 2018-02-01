using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using Common.Util;
using Common.Services;

namespace Common.Web.ApiControllers
{
    public class UtilityApiController : ApiControllerBase
    {
        [Route("api/WxUserInfo")]
        [HttpGet]
        public IHttpActionResult GetWxUser(string code, string iv, string encryptedData)
        {
            WxService ws = new WxService();

            dynamic res = ws.GetWxUser(code, iv, encryptedData);

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
        
    }
}
