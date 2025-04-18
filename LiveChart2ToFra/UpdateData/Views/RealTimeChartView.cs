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

namespace LiveChart2ToFra.UpdateData.Views
{
    public class RealTimeChartView : UserControl
    {
        private CartesianChart _chart;
        private readonly IChartDataModel _chartDataModel;
        private readonly IChartPropertyModel _chartPropertyModel;

        public event Action OnStartRequested;
        public event Action OnStopRequested;
        public event Action OnOverallRequested;

        //给ChartPropertyController 订阅使用
        public event Action<int> OnXMinLimitChangeRequested;
        public event Action<int> OnXMaxLimitChangeRequested;
        public event Action<Font> OnFontChangeRequested;

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
                XAxes = new[] { new Axis { Name = "米数(m)" } },
                YAxes = new[] { new Axis { Name = "颗粒数" } },
                Dock = DockStyle.Fill,
                // 仅 X 轴平移
                ZoomMode = ZoomAndPanMode.X,
                // 图例位置设置为顶部
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Top,
            };
        }

        private void SetupLayout()
        {
            // 创建 TableLayoutPanel
            var layoutPanel = new TableLayoutPanel
            {
                RowCount = 5,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 15),  // 控制按钮区大小
                    new RowStyle(SizeType.Percent, 15),   // 控制配置输入框区的大小
                    new RowStyle(SizeType.Percent, 15),   // 控制配置输入框区的大小
                    new RowStyle(SizeType.Percent, 15),   // 控制字体设置按钮区的大小
                    new RowStyle(SizeType.Percent, 65),  // 控制图表区域的大小
                },
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 50),  // 控制输入框和按钮的横向大小
                    new ColumnStyle(SizeType.Percent, 50),  // 控制输入框和按钮的横向大小
                    new ColumnStyle(SizeType.Percent, 50),  // 控制输入框和按钮的横向大小
                },
            };
            // 将图表放入第5行，跨越2列
            layoutPanel.Controls.Add(_chart, 0, 4);
            layoutPanel.SetColumnSpan(_chart, 2); // 让图表占满两列

            AddButton(layoutPanel, "开始", OnStartRequested, 0, 0);
            AddButton(layoutPanel, "停止", OnStopRequested, 1, 0);
            AddButton(layoutPanel, "全局", OnOverallRequested, 2, 0);

            var Xmin = AddTextBox(layoutPanel, "0", 0, 1);
            var Xmax = AddTextBox(layoutPanel, "10", 1, 1);
            AddButton(layoutPanel, "修改X轴起始点", () => OnXMinLimitChangeRequested?.Invoke(Convert.ToInt32(Xmin.Text)), 0, 2);
            AddButton(layoutPanel, "修改X轴终点", () => OnXMaxLimitChangeRequested?.Invoke(Convert.ToInt32(Xmax.Text)), 1, 2);

            AddButton(layoutPanel, "修改标题字体", () =>
            {
                using var dialog = new FontDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    OnFontChangeRequested?.Invoke(dialog.Font);
            }, 0, 3);

            Controls.Add(layoutPanel); // 将layoutPanel加到UserControl的控件集合中
        }

        #region 添加控件方法
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
    }
}
