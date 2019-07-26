﻿using irc_core.DataSources;
using irc_core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public class IMongoCollectionAdapter : DatabaseCollection
    {
        private IMongoCollection<BsonDocument> mongoCollection;

        public IMongoCollectionAdapter(IMongoCollection<BsonDocument> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }


        public override Task<DataModel> GetDataModel(string type, List<string> labels)
        {
            throw new NotImplementedException();
        }

        public override async Task<DataTable> ListData()
        {
            throw new NotImplementedException();
        }
    }
}
