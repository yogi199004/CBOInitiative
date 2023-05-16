using L10N.API.Model;
using System.Runtime.Caching;

namespace L10N.API.Common
{
    public static class CacheHelper
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        public static void SetCacheItem(string itemName, string itemValue)
        {
            Cache.Set(itemName, itemValue,
                new CacheItemPolicy
                { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1440) });
        }

        public static string GetCacheItem(string itemName)
        {
            return Cache.GetCacheItem(itemName)?.Value?.ToString();
        }

        public static Object GetCachedMetaData(string itemName)
        {
            itemName = (itemName + "_MetaData").ToLower();
            return Cache.GetCacheItem(itemName)?.Value;
        }

        public static void SetCachedMetaData(string itemName, List<IEnumerable<MetaDataModel>> metaData)
        {
            itemName = (itemName + "_MetaData").ToLower();
            Cache.Set(itemName, metaData,
                new CacheItemPolicy
                { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) });
        }


        public static Object GetCachedKeyValues(string itemName, string locale)
        {
            itemName = (itemName + "_" + locale).ToLower();
            return Cache.GetCacheItem(itemName)?.Value;
        }

        public static void SetCachedKeyValues(string itemName, string locale, List<KeyValues> metaData)
        {
            itemName = (itemName + "_" + locale).ToLower();
            Cache.Set(itemName, metaData,
                new CacheItemPolicy
                { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) });
        }

    }
}
