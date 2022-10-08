using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    /// <summary>
    /// オフセット位置を指定できるScottPlotのヒートマップ
    /// </summary>
    public class OffsetHeatmap
    {
        private readonly IEnumerable<Heatmap> heatmaps;
        private double xOffsetMeter;

        public Double XOffsetPixel => xOffsetMeter * Constants.PIXEL_PER_METER;

        public FormsPlot Plot { get; }

        public OffsetHeatmap(ScottPlot.FormsPlot plot, IEnumerable<Heatmap> heatmaps)
        {
            this.heatmaps = heatmaps;
            this.Plot = plot;
        }

        /// <summary>
        /// M単位でオフセット量を設定
        /// </summary>
        /// <param name="xOffsetMeter"></param>
        public void SetXOffset(double xOffsetMeter)
        {
            double oldOffsetPixcel = this.XOffsetPixel;
            this.xOffsetMeter = xOffsetMeter;

            double changedOffset = this.XOffsetPixel - oldOffsetPixcel;

            foreach (var hm in heatmaps)
            {
                hm.XMin += changedOffset;
                hm.XMax += changedOffset;
            }
        }
    }

}
