using IdentitySample.Identity.MongoDb.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace IdentitySample.Identity.MongoDb.Manager
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            :base(userManager, authenticationManager){}

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();
            return new ApplicationSignInManager(userManager, context.Authentication);
        }
    }
}