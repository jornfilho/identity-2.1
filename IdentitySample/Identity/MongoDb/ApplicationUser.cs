using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentitySample.Identity.MongoDb.Models;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentitySample.Identity.MongoDb
{
    public class ApplicationUser : IdentityUser
    {
        #region Propriedades
        public virtual ICollection<IdentityClient> Clients { get; set; }

        [BsonIgnore]
        public string CurrentClientId { get; set; }
        #endregion

        #region Construtor
        public ApplicationUser()
        {
            Clients = new Collection<IdentityClient>();
        }
        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, ClaimsIdentity ext = null)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(CurrentClientId))
            {
                claims.Add(new Claim("AspNet.Identity.ClientId", CurrentClientId));
            }

            //  Adicione novos Claims aqui //

            // Adicionando Claims externos capturados no login
            if (ext != null)
            {
                await Global.SetExternalProperties(userIdentity, ext);
            }

            // Gerenciamento de Claims para informaçoes do usuario
            //claims.Add(new Claim("AdmRoles", "True"));

            userIdentity.AddClaims(claims);

            // Add custom user claims here
            return userIdentity;
        }
    }
}