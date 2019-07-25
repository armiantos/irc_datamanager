using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public class MongoDBWrapper : IDatabase
    {
        private MongoClient client; 

        public void Connect(string host, string user, string pass)
        {
            string dbAuth = "admin";
            string port = "27017";

            string[] userTokens = user.Split('/');
            if (userTokens.Length > 1)
            {
                user = userTokens[1];
                dbAuth = userTokens[0];
            }

            string[] hostTokens = host.Split(':');
            if (hostTokens.Length > 1)
            {
                host = hostTokens[0];
                port = hostTokens[1];
            }
            client = new MongoClient($"mongodb://{user}:{pass}@{host}:{port}/{dbAuth}");
            Console.WriteLine($"mongodb://{user}:{pass}@{host}:{port}/{dbAuth}");
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListCollections()
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> ListDatabases()
        {
            var spaces = await client.ListDatabaseNamesAsync();
            return await spaces.ToListAsync();
        }
    }
}
