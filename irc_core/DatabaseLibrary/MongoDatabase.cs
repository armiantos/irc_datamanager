using irc_core.DataSources;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    class MongoDatabase : DbDatabase
    {
        IMongoDatabase db;

        public MongoDatabase(IMongoDatabase db)
        {
            this.db = db;
        }

        public override DbCollection GetCollection(string name)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>(name);
            return new MongoCollection(collection) { Label = name };
        }

        public override async Task<List<string>> ListCollections()
        {
            var collectionNames = await db.ListCollectionNamesAsync();
            return await collectionNames.ToListAsync();
        }
    }
}
