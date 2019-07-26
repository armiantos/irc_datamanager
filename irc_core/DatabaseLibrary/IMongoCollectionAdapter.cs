using irc_core.DataSources;
using irc_core.Models;
using LiveCharts;
using LiveCharts.Wpf;
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


        public override async Task<DataModel> GetDataModel(string type, List<string> labels)
        {
            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                Limit = 70
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
                PlotModel plot = new PlotModel { Label = RandomString(5) };
                labels.ForEach(labelName =>
                {
                    LineSeries line = new LineSeries();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var result in resultsList)
                    {
                        values.Add((double)result[labelName]);
                    }
                    line.Values = values;
                    plot.Series.Add(line);
                });
                return plot;
            }

            else if (type == "Table")
            {
                TableModel table = new TableModel { Label = RandomString(5) };
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
            DataTable dt = new DataTable();
            DataColumn includeCol = dt.Columns.Add("Include", typeof(bool));
            includeCol.DefaultValue = false;
            dt.Columns.Add("Tag");
            dt.Columns.Add("Type");
            var document = await mongoCollection.Find(new BsonDocument()).FirstOrDefaultAsync();
            foreach (var element in document)
            {
                DataRow r = dt.NewRow();
                r["Tag"] = element.Name;
                r["Type"] = BsonTypeMapper.MapToDotNetValue(element.Value).GetType();
                dt.Rows.Add(r);
            }
            return dt;
        }
    }
}
