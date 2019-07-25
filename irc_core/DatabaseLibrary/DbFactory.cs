using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public static class DbFactory
    {
        public static IDatabase CreateDatabase(string type)
        {
            if (type == "mongoDB")
            {
                return new MongoDBWrapper();
            }
            throw new NotImplementedException();
        }
    }
}
