using MongoDB.Driver;

namespace IdentitySample.Identity.MongoDb
{
    public class IdentityContext
    {
        #region Propriedades
        public MongoCollection UsersCollection { get; set; }
        public MongoCollection RolesCollection { get; set; } 
        public MongoCollection ClainsCollection { get; set; } 
        public MongoCollection ClientCollection { get; set; } 
        #endregion

        #region Construtor
        public IdentityContext()
        {
        }

        public IdentityContext(MongoCollection usersCollection)
        {
            UsersCollection = usersCollection;
        }

        public IdentityContext(MongoCollection usersCollection, MongoCollection rolesCollection)
            : this(usersCollection)
        {
            RolesCollection = rolesCollection;
        }

        public IdentityContext(MongoCollection usersCollection, MongoCollection rolesCollection, MongoCollection clainsCollection)
            : this(usersCollection,rolesCollection)
        {
            ClainsCollection = clainsCollection;
        }

        public IdentityContext(MongoCollection usersCollection, MongoCollection rolesCollection, MongoCollection clainsCollection, MongoCollection clientCollection)
            : this(usersCollection, rolesCollection, clainsCollection)
        {
            ClientCollection = clientCollection;
        } 
        #endregion
    }
}