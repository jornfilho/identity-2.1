using System;

namespace IdentitySample.Identity.Configurations
{
    public class LockoutConfig
    {
        public bool UserLockoutEnabledByDefault { get; private set; }
        public TimeSpan DefaultAccountLockoutTimeSpan { get; private set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; private set; }

        public LockoutConfig(bool userLockoutEnabledByDefault, TimeSpan defaultAccountLockoutTimeSpan, int maxFailedAccessAttemptsBeforeLockout)
        {
            UserLockoutEnabledByDefault = userLockoutEnabledByDefault;
            DefaultAccountLockoutTimeSpan = defaultAccountLockoutTimeSpan;
            MaxFailedAccessAttemptsBeforeLockout = maxFailedAccessAttemptsBeforeLockout;
        }
    }
}
