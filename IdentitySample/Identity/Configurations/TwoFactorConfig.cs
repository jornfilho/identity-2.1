namespace IdentitySample.Identity.Configurations
{
    public class TwoFactorConfig
    {
        public TwoFactorSmsConfig Sms { get; private set; }
        public TwoFactorEmailConfig Email { get; private set; }

        public TwoFactorConfig(TwoFactorSmsConfig sms, TwoFactorEmailConfig email)
        {
            Sms = sms;
            Email = email;
        }
    }
}
