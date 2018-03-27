using Common.Domain;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ServiceBase
    {
        protected int _cacheabsoluteminutes = ConfigHelper.GetConfigInt("CacheAbsoluteMinutes", 5);
        protected int _cacheslidingminutes = ConfigHelper.GetConfigInt("CacheSlidingMinutes", 5);

        protected DbContext NewDB()
        {
            //return new DbContext("default");
            return new CommonContext();
        }
    }
}
