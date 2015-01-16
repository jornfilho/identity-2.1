using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentitySample.Identity.Configurations;
using IdentitySample.Identity.MongoDb.Models;
using IdentitySample.Identity.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace IdentitySample.Identity.MongoDb.Manager
{
    // Configuração do UserManager Customizado

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            var configurations = new IdentityConfigurations();

            #region UserValidator
            // Configurando validator para nome de usuario
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = configurations.UserValidator.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = configurations.UserValidator.RequireUniqueEmail
            }; 
            #endregion

            #region PasswordValidator
            // Logica de validação e complexidade de senha
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = configurations.PasswordValidator.RequiredLength,
                RequireNonLetterOrDigit = configurations.PasswordValidator.RequireNonLetterOrDigit,
                RequireDigit = configurations.PasswordValidator.RequireDigit,
                RequireLowercase = configurations.PasswordValidator.RequireLowercase,
                RequireUppercase = configurations.PasswordValidator.RequireUppercase
            }; 
            #endregion

            #region Lockout
            // Configuração de Lockout
            manager.UserLockoutEnabledByDefault = configurations.Lockout.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = configurations.Lockout.DefaultAccountLockoutTimeSpan;
            manager.MaxFailedAccessAttemptsBeforeLockout = configurations.Lockout.MaxFailedAccessAttemptsBeforeLockout; 
            #endregion

            #region Two Factor
            // Registrando os providers para Two Factor.
            manager.RegisterTwoFactorProvider(configurations.TwoFactor.Sms.Name, new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = configurations.TwoFactor.Sms.Message
            });

            manager.RegisterTwoFactorProvider(configurations.TwoFactor.Email.Name, new EmailTokenProvider<ApplicationUser>
            {
                Subject = configurations.TwoFactor.Email.Subject,
                BodyFormat = configurations.TwoFactor.Email.Message
            }); 
            #endregion

            // Definindo a classe de serviço de e-mail
            manager.EmailService = new EmailService();

            // Definindo a classe de serviço de SMS
            manager.SmsService = new SmsService();

            #region UserTokenProvider
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                var userTokenProvider = dataProtectionProvider.Create(configurations.UserTokenProvider);
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(userTokenProvider);
            } 
            #endregion

            return manager;
        }

        // Metodo para login async que guarda os dados Client conectado
        public async Task<IdentityResult> SignInClientAsync(ApplicationUser user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.Clients.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client == null)
            {
                client = new IdentityClient
                {
                    ClientKey = clientKey
                };
                user.Clients.Add(client);
            }

            var result = await UpdateAsync(user);
            user.CurrentClientId = client.Id.ToString(CultureInfo.InvariantCulture);
            return result;
        }

        // Metodo para login async que remove os dados Client conectado
        public async Task<IdentityResult> SignOutClientAsync(ApplicationUser user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.Clients.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client != null)
            {
                user.Clients.Remove(client);
            }

            user.CurrentClientId = null;
            return await UpdateAsync(user);
        }
    }
}