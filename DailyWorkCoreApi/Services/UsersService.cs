using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using DailyWorkCoreApi.Model;
using Microsoft.Extensions.Options;
using DailyWorkCoreApi.Shared;

namespace DailyWorkCoreApi.Services
{

    public class UsersService
    {
        private readonly IMongoCollection<Users> _users;
        public UsersService(IOptions<SystemConfiguration> _config)
        {
            var client = new MongoClient(_config.Value.ConnectionString);
            var database = client.GetDatabase(_config.Value.DatabaseName);

            _users = database.GetCollection<Users>("Users");
        }

        public List<Users> Get() => _users.Find(U => true).ToList();
        public Users Get(string userid) =>
           _users.Find<Users>(U => U.Userid == userid).FirstOrDefault();

        public Users Validate(string userid,string password) =>
          _users.Find<Users>(U => U.Userid == userid && U.Password==password).FirstOrDefault();

        public Users Create(Users User)
        {
            _users.InsertOne(User);
            return User;
        }

        public void Update(string userid, Users User) =>
            _users.ReplaceOne(book => book.Userid == userid, User);

        public void Remove(Users User) =>
            _users.DeleteOne(U => U.Userid == User.Userid);

        public void Remove(string userid) =>
           _users.DeleteOne(U => U.Userid == userid);


    }
}
