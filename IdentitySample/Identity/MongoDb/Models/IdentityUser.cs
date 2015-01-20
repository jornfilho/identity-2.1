using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb.Models
{
    public class IdentityUser : IUser<string>
    {
        #region Propriedades
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string UserName { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Roles { get; set; }

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        [BsonIgnoreIfNull]
        public List<UserLoginInfo> Logins { get; set; }

        [BsonIgnoreIfNull] 
        public List<IdentityUserClaim> Claims { get; set; }
        #endregion

        #region Construtor
        public IdentityUser()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Roles = new List<string>();
            Logins = new List<UserLoginInfo>();
            Claims = new List<IdentityUserClaim>();
        } 
        #endregion

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(login);
        }

        public virtual void RemoveLogin(UserLoginInfo login)
        {
            var loginsToRemove = Logins
                .Where(l => l.LoginProvider == login.LoginProvider)
                .Where(l => l.ProviderKey == login.ProviderKey);

            Logins = Logins.Except(loginsToRemove).ToList();
        }

        public virtual bool HasPassword()
        {
            return false;
        }

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }

        public virtual void RemoveClaim(Claim claim)
        {
            var claimsToRemove = Claims
                .Where(c => c.ClaimType == claim.Type)
                .Where(c => c.ClaimValue == claim.Value);

            Claims = Claims.Except(claimsToRemove).ToList();
        }
    }
}