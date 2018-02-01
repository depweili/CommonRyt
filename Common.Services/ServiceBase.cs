using Common.Domain;
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
        protected DbContext NewDB()
        {
            //return new DbContext("default");
            return new CommonContext();
        }
    }
}
