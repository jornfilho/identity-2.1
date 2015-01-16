using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityRole : IRole<string>
    {
        #region Propriedades
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; private set; }

        [BsonElement("name")]
        public string Name { get; set; } 
        #endregion

        #region Construtor
        public IdentityRole()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public IdentityRole(string roleName)
            : this()
        {
            Name = roleName;
        } 
        #endregion
    }
}