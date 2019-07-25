using irc_core.DataSources;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    class IMongoDatabaseAdapter : DatabaseSpace
    {
        IMongoDatabase db;

        public IMongoDatabaseAdapter(IMongoDatabase db)
        {
            this.db = db;
        }

        public override async Task<List<string>> ListCollections()
        {
            var collectionNames = await db.ListCollectionNamesAsync();
            return await collectionNames.ToListAsync();
        }
    }
}
