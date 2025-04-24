using LiveChart2ToFra.UpdateData.Models;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using LiveChartsCore.Kernel.Events;
using System.Reflection;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace LiveChart2ToFra.UpdateData.Views
{
    public class RealTimeChartView : UserControl
    {
        public CartesianChart _chart;
        public CartesianChart _scrollbarChart;
        private SplitContainer splitContainer;
        private  TextBox txtPage;
        private  Button btnGo;
        private  Button btnSeeAll;
        private Label lbTotalpage;

        private readonly IChartDataModel _chartDataModel;
        private readonly IChartPropertyModel _chartPropertyModel;

        public event Action OnStartRequested;
        public event Action OnStopRequested;
        public event Action OnOverallRequested;
        // 暴露控件事件和图表操作方法
        public event EventHandler GoToPageRequested;
        public event EventHandler ViewAllRequested;

        //给ChartPropertyController 订阅使用
        public event Action<int> OnXMinLimitChangeRequested;
        public event Action<int> OnXMaxLimitChangeRequested;
        public event Action<Font> OnFontChangeRequested;

        //给ChartController  订阅使用
        // 暴露事件给控制器
        public event EventHandler<MouseEventArgs> ScrollbarMouseDown;
        public event EventHandler<MouseEventArgs> ScrollbarMouseMove;
        public event EventHandler<MouseEventArgs> ScrollbarMouseUp;


        public RealTimeChartView(IChartDataModel chartModel,IChartPropertyModel chartPropertyModel)
        {
            _chartDataModel = chartModel;
            _chartPropertyModel = chartPropertyModel;
            InitializeChart();
            SetupLayout();
            SubscribeModelEvents();
        }

        private void InitializeChart()
        {
            _chart = new CartesianChart
            {
                Series = new ObservableCollection<ISeries>
                {
                    new LineSeries<ObservablePoint>
                    {
                        Values = _chartDataModel.Data1,
                        Name = "颗粒数",
                        // 0为直线（没有平滑）  1为 完全平滑的曲线   控制线条是否平滑。其值是一个介于 0 到 1 之间的 double，0 表示直线，1 表示完全平滑的曲线。
                        LineSmoothness = 0,
                        // 填充颜色为空，表示线条没有填充
                        Fill = null,  
                        // 数据点的几何大小设置为10
                        GeometrySize = 1,
                    },
                    // 类似添加数据2、3
                    new LineSeries<ObservablePoint>
                    {
                        Values = _chartDataModel.Data2,
                        Name = "上限",
                        // 0为直线（没有平滑）  1为 完全平滑的曲线   控制线条是否平滑。其值是一个介于 0 到 1 之间的 double，0 表示直线，1 表示完全平滑的曲线。
                        LineSmoothness = 0,
                        // 填充颜色为空，表示线条没有填充
                        Fill = null,  
                        // 数据点的几何大小设置为10
                        GeometrySize = 0,
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = _chartDataModel.Data3,
                        Name = "下线",
                        // 0为直线（没有平滑）  1为 完全平滑的曲线   控制线条是否平滑。其值是一个介于 0 到 1 之间的 double，0 表示直线，1 表示完全平滑的曲线。
                        LineSmoothness = 0,
                        // 填充颜色为空，表示线条没有填充
                        Fill = null,  
                        // 数据点的几何大小设置为10
                        GeometrySize = 0,
                    },
                },
                // 确保标签区域有足够空间
                //XAxes = new[]
                //{ 
                //    new Axis 
                //    { 
                //        Name = "米数(m)",
                //        NamePaint = new SolidColorPaint(SKColors.Black), // 明确设置名称颜色
                //        NameTextSize = 16,       // 名称字体大小
                //        NamePadding = new (0, 0, 0, 20), // 底部增加 20 像素间距
                //    } 
                //},
                YAxes = new[] { new Axis { Name = "颗粒数" } },
                //Size = new Size(600, 400),  // 自定义尺寸
                Dock = DockStyle.Fill,
                // 仅 X 轴平移
                ZoomMode = ZoomAndPanMode.X,
                // 图例位置设置为顶部
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Top,
            };

            // 滚动条图表配置
            _scrollbarChart = new CartesianChart
            {
                // 1. 数据系列 - 使用与主图表相同的数据源（但可定制样式）
                Series = new ObservableCollection<ISeries>{
                    new LineSeries<ObservablePoint>
                    {
                        Values = _chartDataModel.Data1,
                        GeometryStroke = null,
                        GeometryFill = null,
                        DataPadding = new(0, 1)
                    },
                },
                // 2. 边距控制 - 保持与主图表一致的绘制边距
                DrawMargin = _chartDataModel.Margin,
                // 3. 矩形区域 - 用于显示当前可见范围的"滑块"
                Sections = _chartDataModel.Thumbs,
                // 4. 隐藏X轴 - 使滚动条图表不显示坐标轴
                XAxes = [new Axis { IsVisible = false }],      // 通常配置为 IsVisible = false 的轴
                YAxes = [new Axis { IsVisible = false }],      // 同上
                // 5. 隐藏工具提示 - 避免鼠标悬停时出现提示
                TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Hidden,
                // 6. 控件位置 - 在容器中的起始坐标
                Location = new System.Drawing.Point(0, 0),
                // 7. 初始尺寸 - 虽然设置为50x50，但Dock填充会覆盖该值
                Size = new System.Drawing.Size(30, 30),
                // 8. 停靠方式 - 充满父容器（实际生效的尺寸控制）
                Dock = DockStyle.Fill
            };

            // 布局配置
            splitContainer = new SplitContainer
            {
                Orientation = Orientation.Horizontal,
                //Dock = DockStyle.Fill,
                //Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                // 通过调整 SplitterDistance 控制分隔条的初始位置
                //SplitterDistance = 100, // 这个值可以根据需要调整（例如 400 表示主图表高度占 400，滚动条占剩余的高度）
                SplitterWidth = 25,       // 分割条宽度
                SplitterDistance = 500, // 主图表初始高度
                Panel2MinSize = 50,       // 滚动条最小高度
                //Panel2Collapsed =true,
                IsSplitterFixed = true,
                Height = 200,

            };
            // Panel2 高度 = TotalHeight - 100 - SplitterWidth
            // 添加内容到 Panel1 和 Panel2
            //splitContainer.Panel1.BackColor = Color.LightBlue;
            //splitContainer.Panel2.BackColor = Color.LightGreen;

            splitContainer.FixedPanel = FixedPanel.Panel2; // 锁定 Panel2 的尺寸

            splitContainer.Panel1.Controls.Add(_chart);// 主图表在上
            splitContainer.Panel2.Controls.Add(_scrollbarChart); // 滚动条在下

            var auto = LiveChartsCore.Measure.Margin.Auto;
            _chartDataModel.Margin = new(100, auto, 70, auto);
            _chart.DrawMargin = _chartDataModel.Margin;
            _scrollbarChart.DrawMargin = _chartDataModel.Margin;
        }

        private void SetupLayout()
        {
            //// 创建 TableLayoutPanel
            //var layoutPanel = new TableLayoutPanel
            //{
            //    RowCount = 5,
            //    ColumnCount = 3,
            //    Dock = DockStyle.Fill,
            //    AutoSize = true,
            //    RowStyles =
            //    {
            //        //new RowStyle(SizeType.Percent, 5),  // 控制按钮区大小
            //        //new RowStyle(SizeType.Percent, 5),   // 控制配置输入框区的大小
            //        //new RowStyle(SizeType.Percent, 5),   // 控制配置输入框区的大小
            //        //new RowStyle(SizeType.Percent, 5),   // 控制字体设置按钮区的大小
            //        //new RowStyle(SizeType.Percent, 80),  // 控制图表区域的大小
            //        new RowStyle(SizeType.Absolute, 30),  // 第一行固定高度 50
            //        new RowStyle(SizeType.Absolute, 30),  // 第二行固定高度 50
            //        new RowStyle(SizeType.Absolute, 30),  // 第三行固定高度 50
            //        new RowStyle(SizeType.Absolute, 50),  // 第四行固定高度 50
            //        new RowStyle(SizeType.Absolute, 100), // 第五行固定高度 200
            //    },
            //    ColumnStyles =
            //    {
            //        new ColumnStyle(SizeType.Percent, 20),  // 控制输入框和按钮的横向大小
            //        new ColumnStyle(SizeType.Percent, 20),  // 控制输入框和按钮的横向大小
            //        new ColumnStyle(SizeType.Percent, 20),  // 控制输入框和按钮的横向大小
            //    },
            //};
            //// 创建并设置第五行的背景色
            //var backgroundPanel = new Panel
            //{
            //    BackColor = Color.LightBlue,  // 设置背景色
            //    Dock = DockStyle.Fill
            //};
            //layoutPanel.Controls.Add(backgroundPanel, 0, 4);
            //layoutPanel.SetColumnSpan(backgroundPanel, 3); // 让背景填满整行

            //// 将图表放入第5行，跨越2列
            //layoutPanel.Controls.Add(splitContainer, 0, 4);
            //layoutPanel.SetColumnSpan(splitContainer, 2); // 让图表占满两列

            //AddButton(layoutPanel, "开始", () => OnStartRequested?.Invoke(), 0, 0);
            //AddButton(layoutPanel, "停止", () => OnStopRequested?.Invoke(), 1, 0);
            //AddButton(layoutPanel, "全局", () => OnOverallRequested?.Invoke(), 2, 0);

            //var Xmin = AddTextBox(layoutPanel, "0", 0, 1);
            //var Xmax = AddTextBox(layoutPanel, "10", 1, 1);
            //AddButton(layoutPanel, "修改X轴起始点", () => OnXMinLimitChangeRequested?.Invoke(Convert.ToInt32(Xmin.Text)), 0, 2);
            //AddButton(layoutPanel, "修改X轴终点", () => OnXMaxLimitChangeRequested?.Invoke(Convert.ToInt32(Xmax.Text)), 1, 2);

            //AddButton(layoutPanel, "修改标题字体", () =>
            //{
            //    using var dialog = new FontDialog();
            //    if (dialog.ShowDialog() == DialogResult.OK)
            //        OnFontChangeRequested?.Invoke(dialog.Font);
            //}, 0, 3);

            //Controls.Add(layoutPanel); // 将layoutPanel加到UserControl的控件集合中



            // 主布局表格（3行1列）
            var layoutPanel = new TableLayoutPanel
            {
                RowCount = 5,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 40),   // 控制按钮行
                    new RowStyle(SizeType.Absolute, 40),   // X轴设置行
                    new RowStyle(SizeType.Absolute, 40),    // 字体设置行
                    new RowStyle(SizeType.Percent, 150),     // 图表区域
                    new RowStyle(SizeType.Percent, 50)     // 图表区域
                },
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100) }
            };

            // 控制按钮Panel（第一行）
            var controlPanel = new Panel { Dock = DockStyle.Fill };
            var flowControl = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            CreateButton(flowControl, "开始", () => OnStartRequested?.Invoke());
            CreateButton(flowControl, "停止", () => OnStopRequested?.Invoke());
            CreateButton(flowControl, "全局", () => OnOverallRequested?.Invoke());
            //CreateButton(flowControl, "Go", () => GoToPageRequested?.Invoke());
            //CreateButton(flowControl, "View All", () => ViewAllRequested?.Invoke());
            

            controlPanel.Controls.Add(flowControl);
            layoutPanel.Controls.Add(controlPanel, 0, 0);


            // 创建控件
            var panel = new Panel { Height = 30, Dock = DockStyle.Top };
            Label label1 = new Label { Width = 70, Text = "当前页：", Location = new Point(10, 5) };
            txtPage = new TextBox { Width = 50, Location = new Point(80, 5) };
            Label label2 = new Label { Width = 50, Text = "总页：", Location = new Point(140, 5) };
            lbTotalpage = new Label { Width = 50, Text ="", BackColor = Color.AliceBlue, Location = new Point(190, 5) };
            btnGo = new Button
            {
                Text = "Go",
                Location = new Point(250, 5),
                AutoSize = true
            };
            btnSeeAll = new Button
            {
                Text = "View All",
                Location = new Point(330, 5),
                AutoSize = true
            };

            // 添加控件
            panel.Controls.AddRange(new Control[] { label1,txtPage, label2, lbTotalpage, btnGo, btnSeeAll });
            layoutPanel.Controls.Add(panel, 0, 4);





            // X轴设置Panel（第二行）
            var xAxisPanel = new Panel { Dock = DockStyle.Fill };
            var xAxisTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 25),
                    new ColumnStyle(SizeType.Percent, 25),
                    new ColumnStyle(SizeType.Percent, 25),
                    new ColumnStyle(SizeType.Percent, 25)
                }
            };
            var txtXmin = CreateTextBox("0");
            var txtXmax = CreateTextBox("10");
            xAxisTable.Controls.Add(txtXmin, 0, 0);
            xAxisTable.Controls.Add(txtXmax, 1, 0);
            CreateButton(xAxisTable, "修改X轴起点",
                () => OnXMinLimitChangeRequested?.Invoke(Convert.ToInt32(txtXmin.Text)), 2, 0);
            CreateButton(xAxisTable, "修改X轴终点",
                () => OnXMaxLimitChangeRequested?.Invoke(Convert.ToInt32(txtXmax.Text)), 3, 0);

            xAxisPanel.Controls.Add(xAxisTable);
            layoutPanel.Controls.Add(xAxisPanel, 0, 1);

            // 字体设置Panel（第三行）
            var fontPanel = new Panel { Dock = DockStyle.Fill };
            var btnFont = new Button
            {
                Text = "修改标题字体",
                Dock = DockStyle.Left,
                Width = 150,
                Visible = false,
            };
            btnFont.Click += (s, e) =>
            {
                using var dialog = new FontDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    OnFontChangeRequested?.Invoke(dialog.Font);
            };
            fontPanel.Controls.Add(btnFont);
            layoutPanel.Controls.Add(fontPanel, 0, 2);

            // 图表区域（第四行）
            var chartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                //BackColor = Color.LightBlue
            };
            chartPanel.Controls.Add(splitContainer);
            splitContainer.Dock = DockStyle.Fill;
            //chartPanel.Controls.Add(_chart);
            //_chart.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(chartPanel, 0, 3);

            Controls.Add(layoutPanel);

            // 通过按钮点击事件调整 Panel2 位置
            Button btnMoveDown = new Button
            {
                Text = "下移 Panel2",
                Dock = DockStyle.Top,
                Visible = false,
            };
            btnMoveDown.Click += (sender, e) =>
            {
                // 每次点击增加 SplitterDistance，使 Panel2 下移
                if (splitContainer.SplitterDistance < splitContainer.Height - splitContainer.Panel2MinSize - splitContainer.SplitterWidth)
                {
                    splitContainer.SplitterDistance += 20;
                }
            };
            this.Controls.Add(btnMoveDown);

        }

        #region 添加控件方法
        private void CreateButton(Control container, string text, Action action, int? col = null, int? row = null)
        {
            var btn = new Button { Text = text, Margin = new Padding(5) };
            btn.Click += (s, e) => action?.Invoke();

            if (container is TableLayoutPanel tbl && col.HasValue && row.HasValue)
                tbl.Controls.Add(btn, col.Value, row.Value);
            else
                container.Controls.Add(btn);
        }

        private TextBox CreateTextBox(string defaultValue)
        {
            return new TextBox
            {
                Text = defaultValue,
                Margin = new Padding(5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
        }
        private void AddButton(TableLayoutPanel layoutPanel, string text, Action onClick, int column, int row)
        {
            var button = new Button { Text = text };
            button.Click += (s, e) => onClick?.Invoke();
            layoutPanel.Controls.Add(button, column, row);
        }

        private TextBox AddTextBox(TableLayoutPanel layoutPanel, string text, int column, int row)
        {
            var textBox = new TextBox { Text = text };
            layoutPanel.Controls.Add(textBox, column, row);
            return textBox;
        }
        #endregion

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // RealTimeChartView
            // 
            Name = "RealTimeChartView";
            Size = new Size(584, 489);
            ResumeLayout(false);
        }

        private void SubscribeModelEvents()
        {
            _chartDataModel.DataUpdated += (s, e) =>
            {
                _chart.XAxes.First().MaxLimit = _chartDataModel.CurrentMaxX;
                _chart.XAxes.First().MinLimit = _chartDataModel.CurrentMaxX - 10;
            };
        }



        ////如果在非UI线程操作图表，确保通过Control.Invoke更新：
        //private void UpdateChart()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new Action(UpdateChart));
        //        return;
        //    }

        //    // 安全更新图表
        //    _chart.Series.Values = _model.Data1;
        //}

        public void UpdateChart(double xMin, double xMax)
        {
            // 更新图表的 X 范围
            _chart.XAxes.First().MinLimit = xMin;
            _chart.XAxes.First().MaxLimit = xMax;
        }

        protected override void OnLoad(EventArgs e)
        {
            _scrollbarChart.MouseDown += (s, e) => ScrollbarMouseDown?.Invoke(s, e);
            _scrollbarChart.GetDrawnControl().MouseMove += (s, e) => ScrollbarMouseMove?.Invoke(s, e);
            _scrollbarChart.MouseUp += (s, e) => ScrollbarMouseUp?.Invoke(s, e);
        }

        public int GetRequestedPage() => int.TryParse(txtPage.Text, out var page) ? page : -1;
        public void SubscribeEvents(Action goHandler, Action viewAllHandler)
        {
            btnGo.Click += (s, e) => goHandler();
            btnSeeAll.Click += (s, e) => viewAllHandler();
        }

        public void UpdateStatus(IChartDataModel model)
        {
            int start = (model.CurrentPage - 1) * model.PageSizePoints + 1;
            int end = Math.Min(model.CurrentPage * model.PageSizePoints, model.Data1.Count);
            //txtPage.Text = $"显示点位: {start}-{end} (共{model.Data1.Count}点) | 页数: {model.CurrentPage}/{model.TotalPages}";
            txtPage.Text = $"{model.CurrentPage}";
            lbTotalpage.Text =  $"{model.TotalPages}";
            if(model.ScrollableAxes[0].MinLimit == null)
            {
                txtPage.Text = $"0";
            }
        }
    }
}
