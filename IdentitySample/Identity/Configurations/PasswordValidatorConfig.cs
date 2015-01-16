namespace IdentitySample.Identity.Configurations
{
    public class PasswordValidatorConfig
    {
        public int RequiredLength { get; private set; }
        public bool RequireNonLetterOrDigit { get; private set; }
        public bool RequireDigit { get; private set; }
        public bool RequireLowercase { get; private set; }
        public bool RequireUppercase { get; private set; }

        public PasswordValidatorConfig(int requiredLength, bool requireNonLetterOrDigit, bool requireDigit, bool requireLowercase, bool requireUppercase)
        {
            RequiredLength = requiredLength;
            RequireNonLetterOrDigit = requireNonLetterOrDigit;
            RequireDigit = requireDigit;
            RequireLowercase = requireLowercase;
            RequireUppercase = requireUppercase;
        }
    }
}