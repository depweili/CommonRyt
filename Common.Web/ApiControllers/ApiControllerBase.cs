using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    public class ApiControllerBase : ApiController
    {
        protected int _cacheabsoluteminutes = ConfigHelper.GetConfigInt("CacheAbsoluteMinutes", 5);
        protected int _cacheslidingminutes = ConfigHelper.GetConfigInt("CacheSlidingMinutes", 5);
    }
}
