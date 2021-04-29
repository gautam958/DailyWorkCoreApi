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
        private readonly IMongoCollection<user> _users;
        public UsersService(IOptions<SystemConfiguration> _config)
        {
            var client = new MongoClient(_config.Value.ConnectionString);
            var database = client.GetDatabase(_config.Value.DatabaseName);

            _users = database.GetCollection<user>("users");
        }

        public List<user> Get() => _users.Find(U => true).ToList();
 
        public user GetById(string _id) =>
       _users.Find<user>(U => U._id == _id).FirstOrDefault();
        public user GetByUserId(string _Userid) =>
       _users.Find<user>(U => U.Userid == _Userid).FirstOrDefault();

        public user Validate(string userid,string password) =>
          _users.Find<user>(U => U.Userid == userid && U.Password==password).FirstOrDefault();

        public user Create(user user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string userid, user User) =>
            _users.ReplaceOne(book => book.Userid == userid, User);

        public void Remove(user User) =>
            _users.DeleteOne(U => U.Userid == User.Userid);

        public void Remove(string _id) =>
           _users.DeleteOne(U => U._id == _id);


    }
}
