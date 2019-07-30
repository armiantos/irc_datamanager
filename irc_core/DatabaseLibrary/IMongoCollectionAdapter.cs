using irc_core.DataSources;
using irc_core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace irc_core.DatabaseLibrary
{
    public class IMongoCollectionAdapter : DatabaseCollection
    {
        private IMongoCollection<BsonDocument> mongoCollection;

        private DataTable listDataCache;

        public IMongoCollectionAdapter(IMongoCollection<BsonDocument> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }

        public override async Task<DataModel> GetDataModel(string type, List<string> labels)
        {
            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                Limit = 500
            };

            if (labels.Count > 0)
            {
                string projectionBuilder = "{";
                labels.ForEach(label => projectionBuilder += $"\"{label}\" : 1, ");
                projectionBuilder = projectionBuilder.Remove(projectionBuilder.Length - 2);
                projectionBuilder += "}";
                options.Projection = projectionBuilder;
            }

            var results = await mongoCollection.FindAsync(filter, options);
            var resultsList = await results.ToListAsync();

            if (type == "Plot")
            {
                PlotModel plot = new PlotModel();
                labels.ForEach(labelName =>
                {
                    LineSeries line = new LineSeries();
                    line.Title = labelName;
                    
                    for (int i = 0; i < resultsList.Count; i++)
                    {
                        line.Points.Add(new OxyPlot.DataPoint(i, (double)resultsList[i][labelName]));
                    }
                    plot.Model.Series.Add(line);
                });
                return plot;
            }

            else if (type == "Table")
            {
                TableModel table = new TableModel();

                DataTable dt = new DataTable();
                
                labels.ForEach(labelName => dt.Columns.Add(labelName, 
                    BsonTypeMapper.MapToDotNetValue(resultsList[0][labelName]).GetType()));

                foreach (var result in resultsList)
                {
                    DataRow r = dt.NewRow();
                    labels.ForEach(label => r[label] = result[label]);
                    dt.Rows.Add(r);
                }

                table.DataView = dt.AsDataView();
                return table;
            }
            throw new NotImplementedException();
        }

        public override async Task<DataTable> ListData()
        {
            if (listDataCache == null)
            {
                listDataCache = new DataTable();
                DataColumn includeCol = listDataCache.Columns.Add("Include", typeof(bool));
                includeCol.DefaultValue = false;
                listDataCache.Columns.Add("Tag");
                listDataCache.Columns.Add("Type");
                var document = await mongoCollection.Find(new BsonDocument()).Limit(1).FirstOrDefaultAsync();

                foreach (var element in document)
                {
                    DataRow r = listDataCache.NewRow();
                    r["Tag"] = element.Name;
                    r["Type"] = BsonTypeMapper.MapToDotNetValue(element.Value).GetType();
                    listDataCache.Rows.Add(r);
                }
            }
            return listDataCache;
        }
    }
}
