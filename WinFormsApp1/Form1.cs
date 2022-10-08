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

        // Controls

        private readonly TableLayoutPanel tlpPlots;
        private readonly TextBoxEx txtOffsetX;
        private readonly TextBoxEx txtZoomX;
        private readonly TableLayoutPanel tlpCommands;
        private readonly HScrollBar hScrollBar;

        public Form1()
        {
            InitializeComponent();

            // コマンドパネル作成
            this.txtOffsetX = Input();
            this.txtZoomX = Input();
            this.tlpCommands = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
            };
            this.tlpCommands.RowStyles.Clear();
            this.tlpCommands.ColumnStyles.Clear();

            this.tlpCommands.RowCount = 1;
            var cmdColumns = new (Control?, ColumnStyle)[]
            {
                (Label(text: "オフセット[m]"), ColStyleAbs(100)),
                (this.txtOffsetX, ColStyleAbs(160)),
                (null, ColStyleAbs(20)),
                (Label(text: "Zoom[m]"), ColStyleAbs(100)),
                (this.txtZoomX, ColStyleAbs(160)),
                (null, new ColumnStyle{ SizeType = SizeType.AutoSize }),
            }
            .ToList();
            this.tlpCommands.ColumnCount = cmdColumns.Count();
            foreach (var (item, i) in cmdColumns.Indexed())
            {
                var (ctl, style) = item;
                this.tlpCommands.ColumnStyles.Add(style);
                if (ctl != null)
                {
                    this.tlpCommands.Controls.Add(ctl, i, 0);
                }
            };

            this.tlpPlots = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
            };
            this.hScrollBar = new HScrollBar
            {
                Dock = DockStyle.Bottom,
            };
            this.Controls.AddRange(new Control[]
            {
                this.tlpCommands,
                this.tlpPlots,
                this.hScrollBar
            });


            this.Load += Form_Load;
        }



        private ColumnStyle ColStyleAbs(float width)
        {
            return new ColumnStyle { SizeType = SizeType.Absolute, Width = width };
        }

        private Label Label(string text)
        {
            return new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, };
        }
        private TextBoxEx Input()
        {
            return new TextBoxEx() { Dock = DockStyle.Fill };
        }

        private void Form_Load(object? sender, EventArgs e)
        {
            const double defaultZoom = 200;
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


            // オフセット値を変えたら、1つ目のプロットだけｘ座標をずらす
            this.txtOffsetX.TextChanged += (s, e) =>
            {
                if (double.TryParse(this.txtOffsetX.Text, out double v))
                {
                    _heatmaps[0].SetXOffset(v);
                    _heatmaps[0].Refresh();
                }
            };

            // ズーム範囲を変えたら、全体のプロットの表示範囲を変える
            this.txtZoomX.TextChanged += (s, e) =>
            {
                if (double.TryParse(this.txtZoomX.Text, out double v))
                {
                    foreach (var hm in _heatmaps.Values)
                    {
                        hm.SetXZoom(v);
                        hm.Refresh();
                    }
                }
            };

            // スクロールバーの位置にプロットの表示範囲が同期するようにする
            this.hScrollBar.Minimum = 0;
            this.hScrollBar.Maximum = Constants.IMG_SIZE.Width;
            this.hScrollBar.LargeChange = this.hScrollBar.Maximum / 20;
            this.hScrollBar.Maximum = Constants.IMG_SIZE.Width + this.hScrollBar.LargeChange;
            this.hScrollBar.ValueChanged += (s, e) =>
            {
                //skControl1.Invalidate();
                var x = Math.Min(this.hScrollBar.Value, Constants.IMG_SIZE.Width - this.tlpPlots.ClientRectangle.Width);
                foreach (var hm in _heatmaps.Values)
                {
                    hm.SetXMin(x);
                    hm.Refresh();
                };
            };
            txtZoomX.Text = defaultZoom.ToString();
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
            c.DrawBitmap(_bmp, -Math.Min(this.hScrollBar.Value, Constants.IMG_SIZE.Width - this.ClientRectangle.Width), 0);
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