using System.Data.Entity;
using IdentitySample.Identity.SqlServer.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentitySample.Identity.SqlServer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<IdentityClient> Client { get; set; }

        public DbSet<IdentityClaim> Claims { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}