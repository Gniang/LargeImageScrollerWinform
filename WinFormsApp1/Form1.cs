using NumSharp;
using NumSharp.Generic;
using ScottPlot;
using ScottPlot.Plottable;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private SKBitmap? _bmp;
        private Dictionary<int, OffsetHeatmap> _heatmaps;


        public Form1()
        {
            InitializeComponent();
            this.Load += Form_Load;
        }

        private void Form_Load(object? sender, EventArgs e)
        {
            _heatmaps = new Dictionary<int, OffsetHeatmap>();

            // プロットｎ個
            var plots = Enumerable.Range(0, 2)
                .Select(x => new ScottPlot.FormsPlot() { Dock = DockStyle.Fill })
                .ToList();

            // プロットを縦にならべる
            this.tlpPlots.RowStyles.Clear();
            this.tlpPlots.RowCount = plots.Count;
            this.tlpPlots.ColumnStyles.Clear();
            this.tlpPlots.ColumnCount = 1;
            foreach (var (plot, i) in plots.Indexed())
            {
                this.tlpPlots.RowStyles.Add(new RowStyle() { SizeType = SizeType.Percent, Height = 1.0f / plots.Count });
                this.tlpPlots.Controls.Add(plot, column: 0, row: i);
            }

            //// 200_000x200pxの画像を作成、表示してみる。
            //this.skControl1.PaintSurface += sk_Paint;
            //CreateBaseImage2();
            //_bmp = SKBitmap.Decode(IMG_PATH);


            // オフセット値を変えたら、1つ目のプロットだけｘ座標をずらす
            this.txtOffsetX.TextChanged += (s, e) =>
            {
                if (int.TryParse(this.txtOffsetX.Text, out int v))
                {
                    _heatmaps[0].SetXOffset(v);
                    _heatmaps[0].Plot.Refresh();
                }
            };

            // プロットのデータをランダム値のヒートマップで作る
            foreach (var (plot, i) in plots.Indexed())
            {
                var hms = new List<Heatmap>();
                // 縦横が 65536÷2 以下の必要があるので、とりあえず横10000pxごとに分割して作る （.net のbitmap制約）
                foreach (var n in Enumerable.Range(0, 20))
                {
                    const int widthSize = 10_000;
                    double[,] heatmapVal = (double[,])np.random.rand(Constants.IMG_SIZE.Height, widthSize).ToMuliDimArray<double>();
                    var hm = plot.Plot.AddHeatmap(heatmapVal, lockScales: false);
                    hm.XMin = n * widthSize;
                    hm.XMax = (n + 1) * widthSize - 1;
                    hms.Add(hm);
                }

                _heatmaps.Add(i, new OffsetHeatmap(plot, hms));

                plot.Plot.XAxis.TickLabelFormat(x => $"{x / Constants.PIXEL_PER_METER:0.0}m");
                plot.Plot.YAxis.TickLabelFormat(y => $"{y / Constants.PIXEL_PER_METER:0.00}m");
                plot.Plot.SetAxisLimits(xMin: 0, xMax: 2_000, yMin: 0, yMax: Constants.IMG_SIZE.Height);
                plot.Refresh();
            };


            // スクロールバーの位置にプロットの表示範囲が同期するようにする
            this.hScrollBar1.Minimum = 0;
            this.hScrollBar1.Maximum = Constants.IMG_SIZE.Width;
            this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 20;
            this.hScrollBar1.Maximum = Constants.IMG_SIZE.Width + this.hScrollBar1.LargeChange;
            this.hScrollBar1.ValueChanged += (s, e) =>
            {
                //skControl1.Invalidate();
                var x = Math.Min(this.hScrollBar1.Value, Constants.IMG_SIZE.Width - this.tlpPlots.ClientRectangle.Width);
                plots.ForEach(plot =>
                {
                    plot.Plot.SetAxisLimits(xMin: x, xMax: x + 2_000, yMin: 0, yMax: Constants.IMG_SIZE.Height);
                    plot.Refresh();
                });
            };
        }


        private void sk_Paint(object? sender, SKPaintSurfaceEventArgs e)
        {
            var s = e.Surface;
            var c = s.Canvas;

            if (_bmp is null)
            {
                return;
            }
            return;
            c.DrawBitmap(_bmp, -Math.Min(this.hScrollBar1.Value, Constants.IMG_SIZE.Width - this.ClientRectangle.Width), 0);
        }


        private void CreateBaseImage2()
        {
            // Create an image and fill it blue
            using SKBitmap bmp = new(Constants.IMG_SIZE.Width, Constants.IMG_SIZE.Height);
            using SKCanvas canvas = new(bmp);
            canvas.Clear(SKColor.Parse("#003366"));

            // Draw lines with random positions and thicknesses
            Random rand = new(0);
            SKPaint paint = new() { Color = SKColors.White.WithAlpha(100), IsAntialias = true };
            for (int i = 0; i < 100; i++)
            {
                SKPoint pt1 = new(rand.Next(bmp.Width), rand.Next(bmp.Height));
                SKPoint pt2 = new(rand.Next(bmp.Width), rand.Next(bmp.Height));
                paint.StrokeWidth = rand.Next(1, 10);
                canvas.DrawLine(pt1, pt2, paint);
            }

            // Save the image to disk
            SKFileWStream fs = new(Constants.IMG_PATH);
            bmp.Encode(fs, SKEncodedImageFormat.Png, quality: 85);
        }

        private void CreateBaseImage()
        {
            using var redStroke = new SKPaint() { Color = SKColor.Parse("FF0000") };
            using var skyPen = new SKPaint() { Color = SKColor.Parse("0088AA") };
            using var dotStroke = new SKPaint()
            {
                Color = SKColor.Parse("FF0000"),
                PathEffect = SKPathEffect.CreateDash(new[] { 2f, 2f }, 0)
            };


            var info = new SkiaSharp.SKImageInfo() { Width = Constants.IMG_SIZE.Width, Height = Constants.IMG_SIZE.Height };
            using SKBitmap bmp = new(info);
            //var img = SkiaSharp.SKSurface.Create(info);


            //SKSurface surface = ;
            using SKCanvas canvas = new(bmp);
            canvas.Clear();

            // Translate to center
            canvas.Translate(info.Width / 2, info.Height / 2);

            // Draw the circle
            float radius = Math.Min(info.Width, info.Height) / 4;
            canvas.DrawCircle(0, 0, radius, redStroke);

            // Get the value of the Slider
            //float angle = (float)angleSlider.Value;
            float angle = 0;

            // Calculate sin and cosine for half that angle
            float sin = (float)Math.Sin(Math.PI * angle / 180 / 2);
            float cos = (float)Math.Cos(Math.PI * angle / 180 / 2);

            // Find the points and weight
            SKPoint point0 = new SKPoint(-radius * sin, radius * cos);
            SKPoint point1 = new SKPoint(0, radius / cos);
            SKPoint point2 = new SKPoint(radius * sin, radius * cos);
            float weight = cos;

            // Draw the points
            canvas.DrawCircle(point0.X, point0.Y, 10, skyPen);
            canvas.DrawCircle(point1.X, point1.Y, 10, skyPen);
            canvas.DrawCircle(point2.X, point2.Y, 10, skyPen);

            // Draw the tangent lines
            canvas.DrawLine(point0.X, point0.Y, point1.X, point1.Y, dotStroke);
            canvas.DrawLine(point2.X, point2.Y, point1.X, point1.Y, dotStroke);

            // Draw the conic
            using SKPath path = new();
            path.MoveTo(point0);
            path.ConicTo(point1, point2, weight);
            canvas.DrawPath(path, redStroke);


            bmp.Encode(new SKFileWStream("./test.png"), SKEncodedImageFormat.Png, quality: 100);

            Console.WriteLine("Image Created");
        }
    }
}