using irc_core.DataSources;
using irc_core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using OxyPlot.Axes;

namespace irc_core.DatabaseLibrary
{
    public class IMongoCollectionAdapter : DatabaseCollection
    {
        private IMongoCollection<BsonDocument> mongoCollection;

        private DataTable listDataCache;

        private string timeTag;

        public IMongoCollectionAdapter(IMongoCollection<BsonDocument> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }

        public override async Task<DataModel> GetDataModel(string type, List<string> tags)
        {
            DataModel datamodel;
            if (type == "Plot")
            {
                datamodel = new PlotModel();
            }
            else if (type == "Table")
            {
                datamodel = new TableModel();
            }
            else
            {
                throw new NotImplementedException();
            }

            // set up data model
            datamodel.Tags = tags;

            await Update(datamodel);

            return datamodel;
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

                    if (string.IsNullOrEmpty(timeTag))
                    {
                        if (r["Type"] is DateTime || element.Name.ToLower().Contains("time"))
                        {
                            timeTag = element.Name;
                        }
                    }

                    listDataCache.Rows.Add(r);
                }
            }
            foreach (DataRow row in listDataCache.Rows)
            {
                row["Include"] = false;
            }
            return listDataCache;
        }

        private async Task<List<BsonDocument>> GetData(DataModel model)
        {
            if (!string.IsNullOrEmpty(timeTag) && !model.Tags.Contains(timeTag))
            {
                model.Tags.Add(timeTag);
            }

            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                Limit = 500,
                Sort = "{$natural:-1}" // get latest data 
            }; 

            if (model.Tags.Count > 0)
            {
                string projectionBuilder = "{";
                model.Tags.ForEach(label => projectionBuilder += $"\"{label}\" : 1, ");
                projectionBuilder = projectionBuilder.Remove(projectionBuilder.Length - 2);
                projectionBuilder += "}";
                options.Projection = projectionBuilder;
            }

            var results = await mongoCollection.FindAsync(filter, options);

            return await results.ToListAsync();
        }

        protected override async Task Update(DataModel model)
        {
            List<BsonDocument> results = await GetData(model);
            
            if (model is PlotModel)
            {
                PlotModel actualModel = (PlotModel)model;

                if (!string.IsNullOrEmpty(timeTag))
                {
                    if (actualModel.Model.Axes.FirstOrDefault(axis => axis is DateTimeAxis) == null)
                        actualModel.Model.Axes.Add(new DateTimeAxis());
                }

                actualModel.Model.Series.Clear();
                foreach (string tag in actualModel.Tags)
                {
                    if (tag == timeTag)
                        continue;
                    LineSeries line = new LineSeries();
                    line.Title = tag;

                    for (int i = 0; i < results.Count; i++)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(timeTag))
                            {
                                line.Points.Add(new OxyPlot.DataPoint(
                                    Axis.ToDouble(results[i][timeTag].ToUniversalTime()),
                                    results[i][tag].ToDouble()));
                            }
                            else
                            {
                                line.Points.Add(new OxyPlot.DataPoint(results.Count - i, results[i][tag].ToDouble()));
                            }
                        }
                        catch 
                        {
                        }
                    }
                    actualModel.Model.Series.Add(line);
                    actualModel.Model.InvalidatePlot(true);
                }
                actualModel.Tags.ForEach(tag =>
                {
                    
                });
            }

            else if (model is TableModel)
            {
                TableModel actualModel = (TableModel)model;
                DataTable dt = new DataTable();

                model.Tags.ForEach(labelName => dt.Columns.Add(labelName,
                    BsonTypeMapper.MapToDotNetValue(results[0][labelName]).GetType()));

                foreach (var result in results)
                {
                    DataRow r = dt.NewRow();
                    actualModel.Tags.ForEach(label => r[label] = result[label]);
                    dt.Rows.Add(r);
                }

                actualModel.DataView = dt.AsDataView();
            }

        }

        protected override void ExportData(object filterArgs)
        {
            throw new NotImplementedException();
        }
    }
}
