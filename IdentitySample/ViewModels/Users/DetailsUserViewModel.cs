using System.Collections.Generic;

namespace IdentitySample.ViewModels.Users
{
    public class DetailsUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
        public IList<Claim> Claims { get; set; }

        public DetailsUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<Claim>();
        }

        public class Claim
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
