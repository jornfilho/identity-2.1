using System.Security.Claims;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityUserClaim
    {
        #region Propriedades
        [BsonElement("type")]
        public string ClaimType { get; set; }
        [BsonElement("value")]
        public string ClaimValue { get; set; } 
        #endregion

        #region Construtor
        public IdentityUserClaim()
        {
        }

        public IdentityUserClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        } 
        #endregion

        public Claim ToSecurityClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}