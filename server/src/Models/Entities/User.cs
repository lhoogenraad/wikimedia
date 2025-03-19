using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models.Entities
{
    public class User
    {
        [BsonId] // Marks the Id property as the primary key for MongoDB.
        public ObjectId Id { get; set; }

        [BsonElement("firstName")] // Specifies the name of the field in MongoDB.
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("createdDate")] // Date when the user was created
        public DateTime CreatedDate { get; set; }

        [BsonElement("isActive")] // Whether the user is active or deactivated
        public bool IsActive { get; set; }
    }
}

