using DailyWorkCoreApi.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyWorkCoreApi.Services
{
    public   class MongoHelperService
    {
          IOptions<SystemConfiguration> _CONFIG;
        
        public   MongoHelperService(IOptions<SystemConfiguration> _config)
        {
            _CONFIG = _config;
            
        }
        public  IMongoDatabase GetMongDb()
        {
            var client = new MongoClient(_CONFIG.Value. ConnectionString);
            var database = client.GetDatabase(_CONFIG.Value. DatabaseName);
            return database;
        }
    }
}
