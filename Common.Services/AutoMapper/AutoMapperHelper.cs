using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public static class AutoMapperHelper
    {
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            return Mapper.Map<T>(obj);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource self, TDestination result)
        {
            if (self == null) return result;
            return Mapper.Map<TSource, TDestination>(self, result);

        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return Mapper.Map<List<TDestination>>(source);
        }

        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            return Mapper.Map<List<TDestination>>(source);
        }
    }
}
