using OxyPlot;
using System.Collections.Generic;

namespace irc_core.Models
{
    public class PlotModel : DataModel
    {
        private OxyPlot.PlotModel model;

        public OxyPlot.PlotModel Model
        {
            get
            {
                if (model == null)
                {
                    model = new OxyPlot.PlotModel();
                }
                return model;
            }
            set => model = value;
        }
    }
}
