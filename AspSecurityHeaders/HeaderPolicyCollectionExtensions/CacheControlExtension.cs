using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions
{
    public static class CacheControlExtension
    {
        private const string Header = "Cache-Control";
        
        public static HeaderPolicyCollection AddCacheControlPrivate(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCacheControl("private");
        }
        
        public static HeaderPolicyCollection AddCacheControlPublic(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCacheControl("public");
        }
        
        public static HeaderPolicyCollection AddCacheControlNoCache(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCacheControl("no-cache");
        }
        
        public static HeaderPolicyCollection AddCacheControlNoStore(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCacheControl("no-store");
        }
        
        public static HeaderPolicyCollection AddCacheControl(this HeaderPolicyCollection headerPolicyCollection,
            string cacheControl)
        {
            return headerPolicyCollection.AddCustomHeader(Header, cacheControl);
        }
    }
}