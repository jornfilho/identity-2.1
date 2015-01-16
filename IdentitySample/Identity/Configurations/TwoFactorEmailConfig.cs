namespace IdentitySample.Identity.Configurations
{
    public class TwoFactorEmailConfig
    {
        public string Name { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }

        public TwoFactorEmailConfig(string name, string subject, string message)
        {
            Name = name;
            Subject = subject;
            Message = message;
        }
    }
}
