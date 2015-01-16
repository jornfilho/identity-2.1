using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityClaim
    {
        #region Propriedades
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; private set; }

        [BsonElement("name")]
        public string Name { get; set; }
        #endregion

        #region Construtor
        public IdentityClaim()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public IdentityClaim(string roleName)
            : this()
        {
            Name = roleName;
        }
        #endregion
    }
}