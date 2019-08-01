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
                    model.DefaultColors = new List<OxyColor>
                    {
                        OxyColor.FromRgb(166, 8, 84),
                        OxyColor.FromRgb(166, 8, 8),
                        OxyColor.FromRgb(66, 66, 66),
                        OxyColor.FromRgb(87, 8, 8),
                        OxyColor.FromRgb(61, 59, 71),
                        OxyColor.FromRgb(105, 105, 105),
                    };
                }
                return model;
            }
            set => model = value; }
    }
}
