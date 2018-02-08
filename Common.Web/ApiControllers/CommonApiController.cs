using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    public class CommonApiController : ApiController
    {
        /// <summary>
        /// 微信用户登陆
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iv"></param>
        /// <param name="encryptedData"></param>
        /// <param name="sharecode"></param>
        /// <returns></returns>
        [Route("api/WxUser")]
        [HttpGet]
        public IHttpActionResult GetWxUser(string code, string iv, string encryptedData, string sharecode = null)
        {
            var res = new ResponseBase();
            try
            {
                var service = new WxService();

                dynamic wxuser = service.GetWxUser(code, iv, encryptedData);

                wxuser.sharecode = sharecode;
                
                var data = service.GetUser(wxuser);

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
        /// 导航信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("api/Navigations")]
        [HttpGet]
        public IHttpActionResult GetNavigations(int type = 0)
        {
            var res = new ResponseBase();
            try
            {
                var service = new CommonService();
                var data = service.GetNavigations(type);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }

            return Ok(res);
        }


        [Route("api/Area")]
        [HttpGet]
        public IHttpActionResult GetAreas()
        {
            var res = new ResponseBase();
            try
            {
                var service = new CommonService();
                //var data = service.GetAreas();

                //res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }

            return Ok(res);
        }


    }
}
