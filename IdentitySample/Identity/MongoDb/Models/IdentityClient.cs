using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityClient
    {
        #region Propriedades
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string ClientKey { get; set; } 
        #endregion

        #region Construtor
        public IdentityClient()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public IdentityClient(string clientKey)
            : this()
        {
            ClientKey = clientKey;
        } 
        #endregion
    }
}