using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace NVCMS.Web.Components
{
    public class HttpCacheHelper
    {
        public static object GetFromCache(string key)
        {
            if (HttpContext.Current.Cache == null)
                return null;
            return HttpContext.Current.Cache[key];
        }

        public static void SaveToCache(string key, object item, TimeSpan expiry)
        {
            if (HttpContext.Current.Cache != null)
                HttpContext.Current.Cache.Insert(key, item, null,  DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration);
        }

        public static void SaveToCacheDependency(string database, string[] tableName, string cacheName, object data, TimeSpan expiry)
        {
            if (HttpContext.Current.Cache == null)
                return;
            var dependencies = new AggregateCacheDependency();
            var dependencyArray = new SqlCacheDependency[tableName.Length];
            for (int i = 0; i < tableName.Length; i++)
            {
                dependencyArray[i] = new SqlCacheDependency(database, tableName[i]);
            }
            dependencies.Add(dependencyArray);
            if (data != null)
            {
                HttpContext.Current.Cache.Insert(cacheName, data, dependencies,  DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration);
            }
        }

        public static void SaveToCacheDependency(string database, string tableName, string cacheName, object data, TimeSpan expiry)
        {
            if (HttpContext.Current.Cache == null)
                return;
            var dependencies = new SqlCacheDependency(database, tableName);
            if (data != null)
            {
                HttpContext.Current.Cache.Insert(cacheName, data, dependencies, DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration);
            }
        }

        public static void RemoveCache(string key)
        {
            if (HttpContext.Current.Cache != null && HttpContext.Current.Cache[key] != null)
                HttpContext.Current.Cache.Remove(key);
        }
    }
}