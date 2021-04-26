﻿using System;
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

            _users = database.GetCollection<user>("Users");
        }

        public List<user> Get() => _users.Find(U => true).ToList();
        public user Get(string userid) =>
           _users.Find<user>(U => U.Userid == userid).FirstOrDefault();

        public user Validate(string userid,string password) =>
          _users.Find<user>(U => U.Userid == userid && U.Password==password).FirstOrDefault();

        public user Create(user User)
        {
            _users.InsertOne(User);
            return User;
        }

        public void Update(string userid, user User) =>
            _users.ReplaceOne(book => book.Userid == userid, User);

        public void Remove(user User) =>
            _users.DeleteOne(U => U.Userid == User.Userid);

        public void Remove(string userid) =>
           _users.DeleteOne(U => U.Userid == userid);


    }
}
