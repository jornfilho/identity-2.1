using System.Collections.Generic;

namespace IdentitySample.ViewModels.Role
{
    public class RoleDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<string> Users { get; set; }
    }
}
