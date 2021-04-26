using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyWorkCoreApi.Model
{
    [BsonIgnoreExtraElements]
    public class user
    {
        //[BsonIgnore]
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string _id { get; set; }
        public string Userid
        { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email
        { get; set; }
        public string ContactNo { get; set; }
        public string Country { get; set; }
         

    }
}
