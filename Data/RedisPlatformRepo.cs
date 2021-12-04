using System.Text.Json;
using CacheService.Models;
using StackExchange.Redis;

namespace CacheService.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentOutOfRangeException(nameof(plat));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(plat);

            //db.StringSet(plat.Id, serialPlat);
            db.HashSet($"hashplatform", new HashEntry[] 
                {new HashEntry(plat.Id, serialPlat)});
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _redis.GetDatabase();

            //var plat = db.StringGet(id);

            var plat = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null;
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.HashGetAll("hashplatform");
            
            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => 
                    JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
                return obj;   
            }
            
            return null;
        }
    }
}
