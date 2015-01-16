namespace IdentitySample.Identity.Configurations
{
    public class TwoFactorSmsConfig
    {
        public string Name { get; private set; }
        public string Message { get; private set; }
        public TwoFactorSmsConfig(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
