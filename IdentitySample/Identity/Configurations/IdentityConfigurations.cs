using System;

namespace IdentitySample.Identity.Configurations
{
    public class IdentityConfigurations
    {
        public UserValidatorConfig UserValidator { get; private set; }
        public PasswordValidatorConfig PasswordValidator { get; private set; }
        public LockoutConfig Lockout { get; private set; }
        public TwoFactorConfig TwoFactor { get; private set; }
        public string UserTokenProvider { get; private set; }

        public IdentityConfigurations()
        {
            UserValidator = new UserValidatorConfig(true, false);
            PasswordValidator = new PasswordValidatorConfig(6, false, false, false, false);
            Lockout = new LockoutConfig(true, TimeSpan.FromMinutes(5), 5);
            UserTokenProvider = "ASP.NET Identity";

            var twoFactorSmsConfig = new TwoFactorSmsConfig("Código via SMS", "Seu código de segurança é: {0}");
            var twoFactorEmailConfig = new TwoFactorEmailConfig("Código via E-mail", "Código de Segurança", "Seu código de segurança é: {0}");
            TwoFactor = new TwoFactorConfig(twoFactorSmsConfig, twoFactorEmailConfig);
        }
    }
}