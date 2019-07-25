using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public class MongoDBWrapper : IDatabase
    {
        public void Connect(string dbHost, string dbUser, string dbPass)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public List<string> ListCollections()
        {
            throw new NotImplementedException();
        }

        public List<string> ListDatabases()
        {
            throw new NotImplementedException();
        }
    }
}
