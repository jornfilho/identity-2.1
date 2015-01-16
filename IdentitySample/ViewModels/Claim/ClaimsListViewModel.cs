using System.Collections.Generic;
using IdentitySample.Identity.MongoDb.Models;

namespace IdentitySample.ViewModels.Claim
{
    public class ClaimsListViewModel
    {
        public IList<IdentityClaim> Claims { get; set; }
    }
}
