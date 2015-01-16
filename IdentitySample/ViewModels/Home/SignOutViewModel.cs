using System.Collections.Generic;
using System.Collections.ObjectModel;
using IdentitySample.Identity.MongoDb.Models;

namespace IdentitySample.ViewModels.Home
{
    public class SignOutViewModel
    {
        public ICollection<IdentityClient> Clients { get; set; }

        public SignOutViewModel()
        {
            Clients = new Collection<IdentityClient>();
        }
    }
}
