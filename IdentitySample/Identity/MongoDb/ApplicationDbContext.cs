using System;
using System.Collections.Generic;
using System.Linq;
using DevUtils.WebConfig;
using IdentitySample.Identity.MongoDb.Models;
using MongoDB.Driver;

namespace IdentitySample.Identity.MongoDb
{
    public class ApplicationDbContext : IdentityContext, IDisposable
    {
        public IList<IdentityClient> Clients { get; set; }

        public IList<IdentityClaim> Claims { get; set; }

        public ApplicationDbContext(MongoCollection usersCollection, MongoCollection rolesCollection, MongoCollection clainsCollection, MongoCollection clientCollection)
            : base(usersCollection, rolesCollection, clainsCollection, clientCollection)
        {
            Clients = clientCollection.FindAs<IdentityClient>(null).ToList();
            Claims = clainsCollection.FindAs<IdentityClaim>(null).ToList();
        }

        public static ApplicationDbContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application

            var baseConnectionString = WebConfigExtensions.GetFromAppSettings("MongoDB-ConnectionString");
            var user = WebConfigExtensions.GetFromAppSettings("MongoDB-User");
            var password = WebConfigExtensions.GetFromAppSettings("MongoDB-Password");
            var mongoDatabase = WebConfigExtensions.GetFromAppSettings("MongoDB-Database");
            string credentials;

            if (!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(password))
                credentials = user + ":" + password + "@";
            else
                credentials = "";

            var mongoClient = new MongoClient(credentials + baseConnectionString);
            var database = mongoClient.GetServer().GetDatabase(mongoDatabase);
            var users = database.GetCollection<IdentityUser>("aspnet-identity-users");
            var roles = database.GetCollection<IdentityRole>("aspnet-identity-roles");
            var claims = database.GetCollection<IdentityUserClaim>("aspnet-identity-claims");
            var client = database.GetCollection<IdentityClient>("aspnet-identity-client");
            return new ApplicationDbContext(users, roles, claims, client);
        }

        public void Dispose()
        {
        }
    }
}