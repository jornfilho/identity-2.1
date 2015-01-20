using System;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.AspNet.Identity;
using IdentitySample.Identity;
using IdentitySample.Identity.Configurations;
using IdentitySample.Identity.MongoDb.Manager;	
using DevUtils.PrimitivesExtensions;
using DevUtils.WebConfig;

namespace IdentitySample
{
    public partial class Startup
    {
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
        private IdentityConfigurations Configurations = new IdentityConfigurations();

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            #region Sql Server
            //ConfigureSqlServerContext(ref app);
            //CookieAuthenticationProvider cookieAuthenticationProvider = GetSqlServerCookieAuthenticationProvider();
            #endregion

            #region MongoDb
            ConfigureMongoDbContext(ref app);
            CookieAuthenticationProvider cookieAuthenticationProvider = GetMongoDbCookieAuthenticationProvider(); 
            #endregion

            SetCookieAuthenticationProvider(ref app, cookieAuthenticationProvider);
            SetSocialLogins(ref app);
        }

        #region SqlServer
        //private void ConfigureSqlServerContext(ref IAppBuilder app)
        //{
        //    app.CreatePerOwinContext(Identity.SqlServer.ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<Identity.SqlServer.Manager.ApplicationUserManager>(Identity.SqlServer.Manager.ApplicationUserManager.Create);
        //    app.CreatePerOwinContext<Identity.SqlServer.Manager.ApplicationRoleManager>(Identity.SqlServer.Manager.ApplicationRoleManager.Create);
        //    app.CreatePerOwinContext<Identity.SqlServer.Manager.ApplicationSignInManager>(Identity.SqlServer.Manager.ApplicationSignInManager.Create);
        //}

        //private CookieAuthenticationProvider GetSqlServerCookieAuthenticationProvider()
        //{
        //    return new CookieAuthenticationProvider
        //    {
        //        // Enables the application to validate the security stamp when the user logs in.
        //        // This is a security feature which is used when you change a password or add an external login to your account.  

        //        //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
        //        //    validateInterval: TimeSpan.FromMinutes(30),
        //        //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

        //        OnValidateIdentity = Identity.SqlServer.Manager.ApplicationCookieIdentityValidator.OnValidateIdentity(
        //            validateInterval: TimeSpan.FromMinutes(0),
        //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //    };
        //}
        #endregion

        #region Mongodb
        private void ConfigureMongoDbContext(ref IAppBuilder app)
        {
            app.CreatePerOwinContext(Identity.MongoDb.ApplicationDbContext.Create);
            app.CreatePerOwinContext<Identity.MongoDb.Manager.ApplicationUserManager>(Identity.MongoDb.Manager.ApplicationUserManager.Create);
            app.CreatePerOwinContext<Identity.MongoDb.Manager.ApplicationRoleManager>(Identity.MongoDb.Manager.ApplicationRoleManager.Create);
            app.CreatePerOwinContext<Identity.MongoDb.Manager.ApplicationSignInManager>(Identity.MongoDb.Manager.ApplicationSignInManager.Create);
        }

        private CookieAuthenticationProvider GetMongoDbCookieAuthenticationProvider()
        {
            return new CookieAuthenticationProvider
            {
                // Enables the application to validate the security stamp when the user logs in.
                // This is a security feature which is used when you change a password or add an external login to your account.  

                //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                //    validateInterval: TimeSpan.FromMinutes(30),
                //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                OnValidateIdentity = Identity.MongoDb.Manager.ApplicationCookieIdentityValidator.OnValidateIdentity(
                    validateInterval: TimeSpan.FromMinutes(0),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            };
        }
        #endregion

        public void SetCookieAuthenticationProvider(ref IAppBuilder app, CookieAuthenticationProvider provider)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            var cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = provider
            };

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(cookieAuthenticationOptions);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        private void SetSocialLogins(ref IAppBuilder app)
        {
            #region Microsoft Account
            var microsoftLoginEnabled = WebConfigExtensions.GetFromAppSettings("MicrosoftLogin-Enabled").TryParseBool();
            if (microsoftLoginEnabled)
            {
                app.UseMicrosoftAccountAuthentication(
                clientId: WebConfigExtensions.GetFromAppSettings("MicrosoftLogin-Id"),
                clientSecret: WebConfigExtensions.GetFromAppSettings("MicrosoftLogin-Secret")); 
            }
            #endregion

            #region Twitter
            var twitterLoginEnabled = WebConfigExtensions.GetFromAppSettings("TwitterLogin-Enabled").TryParseBool();
            if (twitterLoginEnabled)
            {
                app.UseTwitterAuthentication(
                    consumerKey: WebConfigExtensions.GetFromAppSettings("TwitterLogin-Key"),
                    consumerSecret: WebConfigExtensions.GetFromAppSettings("TwitterLogin-Secret"));
            }
            #endregion

            #region Google
            var googleLoginEnabled = WebConfigExtensions.GetFromAppSettings("GoogleLogin-Enabled").TryParseBool();
            if (googleLoginEnabled)
            {
                app.UseGoogleAuthentication(
                    clientId: WebConfigExtensions.GetFromAppSettings("GoogleLogin-Id"),
                    clientSecret: WebConfigExtensions.GetFromAppSettings("GoogleLogin-Secret"));
            }
            #endregion
            
            #region Facebook
            var facebookLoginEnabled = WebConfigExtensions.GetFromAppSettings("FacebookLogin-Enabled").TryParseBool();
            if (facebookLoginEnabled)
            {
                var fao = new FacebookAuthenticationOptions
                {
                    AppId = WebConfigExtensions.GetFromAppSettings("FacebookLogin-Id"),
                    AppSecret = WebConfigExtensions.GetFromAppSettings("FacebookLogin-Secret")
                };
                fao.Scope.Add("email");
                fao.Scope.Add("basic_info");
                fao.Provider = new FacebookAuthenticationProvider()
                {

                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
                        foreach (var x in context.User)
                        {
                            var claimType = string.Format("urn:facebook:{0}", x.Key);
                            string claimValue = x.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

                        }
                        return Task.FromResult(0);
                    }
                };

                fao.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
                app.UseFacebookAuthentication(fao);
            } 
            #endregion
        }
    }
}