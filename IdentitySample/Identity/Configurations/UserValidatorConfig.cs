namespace IdentitySample.Identity.Configurations
{
    public class UserValidatorConfig
    {
        public bool AllowOnlyAlphanumericUserNames { get; private set; }
        public bool RequireUniqueEmail { get; private set; }

        public UserValidatorConfig(bool allowOnlyAlphanumericUserNames, bool requireUniqueEmail)
        {
            AllowOnlyAlphanumericUserNames = allowOnlyAlphanumericUserNames;
            RequireUniqueEmail = requireUniqueEmail;
        }
    }
}