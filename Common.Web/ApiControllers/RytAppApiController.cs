using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    public class RytAppApiController : ApiControllerBase
    {

        [Route("api/Recommend")]
        [HttpGet]
        public IHttpActionResult GetRecommend(Guid authid)
        {
            var res = new ResponseBase();
            try
            {
                //var service = new RytService();
                //var data = service.GetArticles(authid);

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
