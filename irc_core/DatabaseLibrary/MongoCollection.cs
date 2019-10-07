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
using System.IO;

namespace irc_core.DatabaseLibrary
{
    /// <summary>
    /// Collection adapter for MongoDB collections generated from MongoDB databases
    /// </summary>
    public class MongoCollection : DbCollection
    {
        private IMongoCollection<BsonDocument> mongoCollection;

        private DataTable listDataCache;  // for optimization

        private string timeTag; // automatically checks which tag should be treated as time

        /// <summary>
        /// Links an IMongoCollection to the adapter class upon construction
        /// </summary>
        /// <param name="mongoCollection"> IMongoColleciton returned by <c>IMongoDatabase.GetCollection()</c></param>
        public MongoCollection(IMongoCollection<BsonDocument> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }

        /// <summary>
        /// Creates a data model of a specified type (plot or table) of the given tags, keys or column labels
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
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
                    DataRow row = listDataCache.NewRow();
                    row["Tag"] = element.Name;

                    // standardize types from MongoDB types
                    row["Type"] = BsonTypeMapper.MapToDotNetValue(element.Value).GetType();  
                    
                    if (string.IsNullOrEmpty(timeTag))
                        if (row["Type"] is DateTime || element.Name.ToLower().Contains("time"))
                            timeTag = element.Name;
                        
                    listDataCache.Rows.Add(row);
                }
            }

            // by default do not include any tags
            foreach (DataRow row in listDataCache.Rows)
            {
                row["Include"] = false;
            }

            return listDataCache;
        }

        /// <summary>
        /// Helper function to populate data model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<List<BsonDocument>> GetData(DataModel model)
        {
            if (!string.IsNullOrEmpty(timeTag) && !model.Tags.Contains(timeTag))
            {
                model.Tags.Add(timeTag);
            }

            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            FindOptions<BsonDocument> options;

            if (!string.IsNullOrEmpty(timeTag)) {
                options = new FindOptions<BsonDocument>
                {
                    Limit = 500,
                    Sort = $"{{{timeTag}: -1}}"  
                };
            }
            else
            {
                options = new FindOptions<BsonDocument>
                {
                    Limit = 500,
                    Sort = $"{{natural: -1}}" // get latest data
                };
            }

            // if tags are given, then only retrieve the given tags
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

        /// <summary>
        /// Streams data to csv file iteratively
        /// Iterative fashion reduces memory usage while saving file
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="timeRange"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override async Task SaveToFile(List<string> tags, Tuple<DateTime, DateTime> timeRange, string path)
        {
            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            bool firstDocument = true;
            if (!string.IsNullOrEmpty(timeTag) && !tags.Contains(timeTag))
            {
                tags.Add(timeTag);
            }
            if (timeRange.Item1 != DateTime.MinValue)
            {
                var builder = new FilterDefinitionBuilder<BsonDocument>();
                filter = builder.Gte(timeTag, timeRange.Item1.ToLocalTime());
                if (timeRange.Item2 != DateTime.MinValue)
                {
                    filter &= builder.Lte(timeTag, timeRange.Item2.ToLocalTime());
                }
            }

            IFindFluent<BsonDocument, BsonDocument> find = mongoCollection.Find(filter)
                .Sort(new SortDefinitionBuilder<BsonDocument>().Ascending(timeTag));

            if (tags.Count > 0)
            {
                string projectionBuilder = "{";
                tags.ForEach(label => projectionBuilder += $"\"{label}\" : 1, ");
                projectionBuilder = projectionBuilder.Remove(projectionBuilder.Length - 2);
                projectionBuilder += "}";
                find = find.Project(projectionBuilder);
            }

            // save documents to file
            StreamWriter sw = new StreamWriter(path, true);  

            await find.ForEachAsync(document =>
            {
                if (firstDocument)
                {
                    foreach (BsonElement element in document)
                        sw.Write(element.Name + ",");
                    sw.Write(sw.NewLine);
                    firstDocument = false;
                }
                foreach (BsonElement element in document)
                    sw.Write(element.Value + ",");
                sw.Write(sw.NewLine);
            });

            sw.Close();            
        }

        /// <summary>
        /// Updates data model contents
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override async Task Update(DataModel model)
        {
            List<BsonDocument> results = await GetData(model);

            if (model is PlotModel plotModel)
            {
                if (!string.IsNullOrEmpty(timeTag))
                {
                    if (plotModel.Model.Axes.FirstOrDefault(axis => axis is DateTimeAxis) == null)
                        plotModel.Model.Axes.Add(new DateTimeAxis());
                }

                plotModel.Model.Series.Clear(); 
                foreach (string tag in plotModel.Tags)
                {
                    if (tag == timeTag)
                        continue;
                    LineSeries line = new LineSeries { Title = tag };

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
                                line.Points.Add(new OxyPlot.DataPoint(
                                    results.Count - i, 
                                    results[i][tag].ToDouble()));
                            }
                        }
                        catch
                        {
                        }
                    }
                    plotModel.Model.Series.Add(line);
                    plotModel.Model.InvalidatePlot(true);
                }
            }

            else if (model is TableModel tableModel)
            {
                DataTable dt = new DataTable();

                model.Tags.ForEach(labelName => dt.Columns.Add(labelName,
                    BsonTypeMapper.MapToDotNetValue(results[0][labelName]).GetType()));

                foreach (var result in results)
                {
                    DataRow r = dt.NewRow();
                    tableModel.Tags.ForEach(label => r[label] = result[label]);
                    dt.Rows.Add(r);
                }

                tableModel.DataView = dt.AsDataView();
            }

        }

    }
}
