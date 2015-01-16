using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentitySample.Identity.SqlServer.Models
{
    [Table("AspNetClients")]
    public class IdentityClient
    {
        [Key]
        public string Id { get; set; }
        public string ClientKey { get; set; }

        public IdentityClient()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}