using Newtonsoft.Json;
using StackExchange.Redis;

namespace AlumniProject.Service.ServiceImp
{
    public class RedisService : IDisposable
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisURL");
            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            IDatabase db = _redis.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }
        public void PublishMessage<T>(string queueName,T message)
        {
            IDatabase db = _redis.GetDatabase();
            string serializedMessage = JsonConvert.SerializeObject(message);
            db.ListLeftPush(queueName, serializedMessage);
        }
        public void Dispose()
        {
            _redis.Dispose();
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            IDatabase db = _redis.GetDatabase();
            string serializedValue = await db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(serializedValue))
            {
                return JsonConvert.DeserializeObject<T>(serializedValue);
            }
            else
            {
                return default(T);
            }
        }

        public async Task<bool> SetObjectAsync<T>(string key, T value)
        {
            IDatabase db = _redis.GetDatabase();
            string serializedValue = JsonConvert.SerializeObject(value);
            return await db.StringSetAsync(key, serializedValue);

        }

        public async Task<bool> UpdateObjectAsync<T>(string key, T value)
        {
            IDatabase db = _redis.GetDatabase();
            if (await db.KeyExistsAsync(key))
            {
                string serializedValue = JsonConvert.SerializeObject(value);
                return await db.StringSetAsync(key, serializedValue);
            }
            else
            {
                throw new InvalidOperationException("Key does not exist");
            }
        }
    }
}
