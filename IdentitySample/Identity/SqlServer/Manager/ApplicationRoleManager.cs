using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace IdentitySample.Identity.SqlServer.Manager
{
    // Configurando o RoleManager utilizado na aplicação.
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole,string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var applicationDbContext = context.Get<ApplicationDbContext>();
            var roleStore = new RoleStore<IdentityRole>(applicationDbContext);
            return new ApplicationRoleManager(roleStore);
        }
    }
}