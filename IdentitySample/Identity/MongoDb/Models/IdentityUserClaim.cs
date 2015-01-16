using System.Security.Claims;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityUserClaim
    {
        #region Propriedades
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("value")]
        public string Value { get; set; } 
        #endregion

        #region Construtor
        public IdentityUserClaim()
        {
        }

        public IdentityUserClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        } 
        #endregion

        public Claim ToSecurityClaim()
        {
            return new Claim(Type, Value);
        }
    }
}