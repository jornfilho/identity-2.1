using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace IdentitySample.Identity.MongoDb.Models
{
    /// <summary>
    ///     Note: Deleting and updating do not modify the roles stored on a user document. If you desire this dynamic
    ///     capability, override the appropriate operations on RoleStore as desired for your application. For example you could
    ///     perform a document modification on the users collection before a delete or a rename.
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    public class RoleStore<TRole> : IRoleStore<TRole>, IQueryableRoleStore<TRole>
        where TRole : IdentityRole
    {
        #region Propriedades
        private readonly IdentityContext _context;
        public virtual IQueryable<TRole> Roles
        {
            get
            {
                return _context.RolesCollection.AsQueryable<TRole>();
            }
        } 
        #endregion

        #region Construtor
        public RoleStore(IdentityContext context)
        {
            _context = context;
        } 
        #endregion

        #region Métodos
        public virtual void Dispose()
        {
            // no need to dispose of anything, mongodb handles connection pooling automatically
        }

        public virtual Task CreateAsync(TRole role)
        {
            return Task.Run(() => _context.RolesCollection.Insert(role));
        }

        public virtual Task UpdateAsync(TRole role)
        {
            return Task.Run(() => _context.RolesCollection.Save(role));
        }

        public virtual Task DeleteAsync(TRole role)
        {
            var queryUser = Query.In("Roles", new BsonValue[] { role.Name });
            var updateUser = Update.Pull("Roles", role.Name);
            var updateResult = _context.UsersCollection.Update(queryUser,updateUser);
            if (!updateResult.Ok)
                return null;

            var queryById = Query<TRole>.EQ(r => r.Id, role.Id);
            return Task.Run(() => _context.RolesCollection.Remove(queryById));
        }

        public virtual Task<TRole> FindByIdAsync(string roleId)
        {
            return Task.Run(() => _context.RolesCollection.FindOneByIdAs<TRole>(ObjectId.Parse(roleId)));
        }

        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            var queryByName = Query<TRole>.EQ(r => r.Name, roleName);
            return Task.Run(() => _context.RolesCollection.FindOneAs<TRole>(queryByName));
        }
        #endregion
    }
}
