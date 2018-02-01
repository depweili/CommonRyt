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
    }
}
