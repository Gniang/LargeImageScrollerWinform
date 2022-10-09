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
    /// 表示範囲、表示位置を指定できるScottPlotのヒートマップ（WinForms用）
    /// </summary>
    public class OffsetHeatmap
    {
        private readonly IEnumerable<Heatmap> heatmaps;
        private double xOffsetMeter = 0;
        private double xMin;
        private double xZoomMeter = 0;

        public Double XOffsetPixel => xOffsetMeter * Constants.PIXEL_PER_METER;
        public Double XZoomPixel => xZoomMeter * Constants.PIXEL_PER_METER;

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

        /// <summary>
        /// WindowsFormのPlotコントロールを再描画する
        /// </summary>
        public void Refresh()
        {
            this.Plot.Refresh();
        }

        /// <summary>
        /// X軸の表示範囲をメートル単位で指定する
        /// </summary>
        /// <param name="xMeter"></param>
        public void SetXZoom(double xMeter)
        {
            this.xZoomMeter = xMeter;
            ResetAxisLimits();
        }

        /// <summary>
        /// X軸の開始位置をピクセル単位で指定する
        /// </summary>
        /// <param name="xPixel"></param>
        public void SetXMin(double xPixel)
        {
            this.xMin = xPixel;
            ResetAxisLimits();
        }

        /// <summary>
        /// 表示範囲を指定する
        /// </summary>
        private void ResetAxisLimits()
        {
            if (this.XZoomPixel == 0)
            {
                return;
            }

            this.Plot.Plot.SetAxisLimits(
                xMin: this.xMin,
                xMax: this.xMin + this.XZoomPixel,
                yMin: 0,
                yMax: Constants.IMG_SIZE.Height);
        }
    }

}
