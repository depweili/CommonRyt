using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{
    public class CacheHelper
    {
        private static System.Web.Caching.Cache _cache = HttpRuntime.Cache;

        public static bool Exist(string key)
        {
            return !(null == Get(key));
        }

        public static object Get(string key)
        {
            return _cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            return (T)Get(key);
        }

        public static void Set(string key, object value)
        {
            _cache.Insert(key, value);
        }

        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            _cache.Insert(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void Set(string key, object value, TimeSpan slidingExpiration)
        {
            _cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}
