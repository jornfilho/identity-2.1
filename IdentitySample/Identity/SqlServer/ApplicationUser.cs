using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentitySample.Identity.SqlServer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentitySample.Identity.SqlServer
{
    public class ApplicationUser : IdentityUser
    {
        #region Propriedades
        public virtual ICollection<IdentityClient> Clients { get; set; }

        [NotMapped]
        public string CurrentClientId { get; set; } 
        #endregion

        #region Construtor
        public ApplicationUser()
        {
            Clients = new Collection<IdentityClient>();
        } 
        #endregion

        #region Metodos publicos
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, ClaimsIdentity ext = null)
        {
            // Observe que o authenticationType precisa ser o mesmo que foi definido em CookieAuthenticationOptions.AuthenticationType
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

            return userIdentity;
        } 
        #endregion
    }
}