using LiveChart2ToFra.UpdateData.Controllers;
using LiveChart2ToFra.UpdateData.Models;
using LiveChart2ToFra.UpdateData.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveChart2ToFra
{
    public partial class Form1 : Form
    {
        //public class DataPoint : INotifyPropertyChanged
        //{
        //    public double _meter;
        //    private double _value;
        //    public double Value
        //    {
        //        get => _value;
        //        set { _value = value; OnPropertyChanged(); }
        //    }

        //    public double Meter
        //    {
        //        get => _meter;
        //        set { _meter = value; OnPropertyChanged(); }
        //    }
        //    public DataPoint(double t, double v) { Meter = t; _value = v; }
        //    public event PropertyChangedEventHandler? PropertyChanged;
        //    protected void OnPropertyChanged([CallerMemberName] string? p = null)
        //        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        //}

        //public ObservableCollection<DataPoint> dataList1 { get; set; }
        //public ObservableCollection<DataPoint> dataList2 { get; set; }
        //public ObservableCollection<DataPoint> dataList3 { get; set; }

        //public Form1()
        //{
        //    InitializeComponent();
        //    // 仅 X 轴缩放和平移
        //    cartesianChart1.ZoomMode = ZoomAndPanMode.ZoomX;
        //    cartesianChart1.ZoomMode = ZoomAndPanMode.PanX;

        //    //界面上显示几个点
        //    //例如，假设您正在绘制一个 LineSeries 在 X 轴上从 0 到 100，但您只需要向用户显示前 10 个点（ 分页 ），在这种情况下，您可以将 MinLimit 属性设置为 0，将 MaxLimit 设置为 10。
        //    var xAxis = new Axis
        //    {
        //        MaxLimit = 10,
        //        MinLimit = 0
        //    };
        //    cartesianChart1.XAxes = new List<Axis> { xAxis };
        //    //new Axis
        //    //{
        //    //    //设置坐标轴的名称
        //    //    Name = "X Axis",  
        //    //    //设置坐标轴名称的绘制方式。通过 SolidColorPaint 可以设置颜色，这里将 X 轴名称的颜色设置为黑色 (SKColors.Black)。
        //    //    NamePaint = new SolidColorPaint(SKColors.Black), 
        //    //    //设置坐标轴标签（即每个刻度值的文本）的绘制方式。使用 SolidColorPaint 设置颜色，X 轴的标签颜色为蓝色 (SKColors.Blue)。
        //    //    LabelsPaint = new SolidColorPaint(SKColors.Blue),
        //    //    //设置坐标轴标签的字体大小，这里为 10，意味着标签的文字大小。
        //    //    TextSize = 10,
        //    //    设置坐标轴分隔线（刻度线）的样式，通常是设置颜色和线条的粗细。这里将 X 轴的分隔线设置为 LightSlateGray（浅灰色），并且设置了 StrokeThickness 为 2，表示分隔线的粗细。
        //    //    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 }
        //    //}


        //    //cartesianChart1.YAxes = new Axis[]
        //    //{
        //    //    new Axis
        //    //    {
        //    //        Name = "Y Axis",
        //    //        NamePaint = new SolidColorPaint(SKColors.Red),
        //    //        LabelsPaint = new SolidColorPaint(SKColors.Green),
        //    //        TextSize = 20,
        //    //        /*
        //    //         * SeparatorsPaint 控制坐标轴的分隔线（或刻度线）的外观，可以对其进行样式定制。具体属性如下：
        //    //         * PathEffect：这个属性允许你为分隔线设置不同的路径效果，比如虚线或其他自定义样式。DashEffect 是一个常用的效果，用于创建虚线。new float[] { 3, 3 } 表示虚线的模式，即每 3 个单位画线，再跳过 3 个单位形成虚线的效果。
        //    //         * StrokeThickness：控制分隔线的粗细，设置为 2，表示线条相对较粗。
        //    //         */
        //    //        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
        //    //        {
        //    //            StrokeThickness = 2,
        //    //            PathEffect = new DashEffect(new float[] { 3, 3 })
        //    //        }
        //    //    }
        //    //};


        //    //图列
        //    cartesianChart1.LegendPosition = LiveChartsCore.Measure.LegendPosition.Left;  // 图例位置设置为顶部
        //    //cartesianChart1.LegendTextPaint = new SolidColorPaint
        //    //{
        //    //    Color = new SKColor(50, 50, 50),  // 深灰色字体
        //    //    //SKTypeface = SKTypeface.FromFamilyName("Courier New")  // 字体为 Courier New
        //    //};
        //    //cartesianChart1.LegendBackgroundPaint =  new SolidColorPaint(new SKColor(255, 228, 181));  // 浅灰色背景
        //    //cartesianChart1.LegendTextSize = 16;  // 图例文本大小

        //    cartesianChart1.Legend = new CustomLegend();


        //    //工具提示
        //    //cartesianChart1.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Bottom;

        //    cartesianChart1.Series = new ISeries[]
        //    {
        //        /*数据
        //         *  LineSeries<double>：表示一组双精度浮点数的线条。
        //            LineSeries<int, StarGeometry>：表示一组整数的数据点，并且数据点的几何形状是星形。
        //         */

        //        new LineSeries<DataPoint>   //new LineSeries<int, StarGeometry>    可以设置数据点的“形状”   StarGeometry 用来替代默认的圆形标记，作为每个数据点的显示方式
        //        {
        //             Fill = null,  // 填充颜色为空，表示线条没有填充
        //             GeometrySize = 10,  // 数据点的几何大小设置为10
        //            Name = "颗粒数",
        //            Values = dataList1,
        //            //Values = new DataPoint[]
        //            //{
        //            //    new DataPoint (1, 9),

        //            //},
        //            Mapping = (dp, point) => new
        //            ( point,  // X轴
        //              dp.Value    // Y轴
        //            ),


        //        },
        //        new LineSeries<DataPoint>
        //        {
        //            //DataPadding 属性控制数据点和坐标轴边界之间的最小距离
        //            /*包含两个坐标值 (X, Y)，其值范围从 0 到 1，表示从坐标轴的最小距离到轴刻度之间的比例。具体来说：
        //                0 表示没有任何间距。
        //                1 表示与坐标轴刻度对齐，刻度是每个标签或分隔符之间的间隔（即使它们不可见）。
        //             */
        //            DataPadding = new LvcPoint(0, 0),  // 没有额外的间距，直接与坐标轴边界对齐
        //            //DataLabelsSize = 20,  // 设置数据标签的字体大小
        //            //DataLabelsPaint = new SolidColorPaint(SKColors.Blue),  // 设置数据标签的字体颜色
        //            //DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,  // 设置数据标签的位置 (可以是 Top, Bottom, Left, Right)
        //            //DataLabelsFormatter = (point) => point.Coordinate.PrimaryValue.ToString("C2"),  // 格式化数据标签，这里将数据格式化为货币格式
        //            Fill = null,  // 填充颜色为空，表示线条没有填充
        //            GeometryFill = null,  // 不填充几何图形
        //            GeometryStroke = null,  // 不使用几何图形的描边
        //            //GeometryFill = new SolidColorPaint(SKColors.AliceBlue),  // 数据点的填充颜色为 AliceBlue
        //            //GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 }  // 数据点的边框颜色为灰色，宽度为4
        //            GeometrySize = 10,  // 数据点的几何大小设置为10
        //            LineSmoothness = 0 , // 0为直线（没有平滑）  1为 完全平滑的曲线   控制线条是否平滑。其值是一个介于 0 到 1 之间的 double，0 表示直线，1 表示完全平滑的曲线。

        //            Name = "上限",
        //            Values = dataList2,
        //            //Values = new DataPoint[]
        //            //{
        //            //    new DataPoint (1, 11),

        //            //},
        //            Mapping = (dp, point) => new
        //            ( point,  // X轴
        //              dp.Value    // Y轴
        //            ),


        //        },
        //        new LineSeries<DataPoint>
        //        {
        //             Fill = null,  // 填充颜色为空，表示线条没有填充
        //             GeometrySize = 10,  // 数据点的几何大小设置为10
        //            Name = "下限",
        //            //Values = new DataPoint[]
        //            //{
        //            //    new DataPoint (1, 8),
        //            //},
        //            Values = dataList3,
        //            Mapping = (dp, point) => new
        //            ( point,  // X轴
        //              dp.Value    // Y轴
        //            ),
        //        }
        //    };
        //}


        //private Random _random = new Random();
        //private void btnRand_Click(object sender, EventArgs e)
        //{
        //    dataList1 = new ObservableCollection<DataPoint>();
        //    dataList2 = new ObservableCollection<DataPoint>();
        //    dataList3 = new ObservableCollection<DataPoint>();
        //}


        //private Timer _timer;
        //private void btn_Start_Click(object sender, EventArgs e)
        //{
        //    // Set up the timer to add data every 10ms
        //    _timer = new Timer(10);
        //    _timer.Elapsed += OnTimedEvent;
        //    _timer.Start();
        //}

        //private void OnTimedEvent(object sender, ElapsedEventArgs e)
        //{
        //    // Ensure thread safety when updating ObservableCollection
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<object, ElapsedEventArgs>(OnTimedEvent), sender, e);
        //        return;
        //    }

        //    // Add a random value to dataList1 (between 80 and 100)
        //    dataList1.Add(new DataPoint(dataList1.Count, _random.Next(80, 101)));

        //    // Add fixed values to dataList2 and dataList3
        //    dataList2.Add(new DataPoint(dataList2.Count, 50)); // Fixed value for dataList2
        //    dataList3.Add(new DataPoint(dataList3.Count, 60)); // Fixed value for dataList3

        //    // Optionally, remove the first element to prevent the list from growing indefinitely
        //    if (dataList1.Count > 100)
        //        dataList1.RemoveAt(0);
        //    if (dataList2.Count > 100)
        //        dataList2.RemoveAt(0);
        //    if (dataList3.Count > 100)
        //        dataList3.RemoveAt(0);
        //}


        RealTimeChartView view;
        ChartController controller;
        ChartPropertyController controller2;
        public Form1()
        {
            InitializeComponent();

            ChartPropertyModel chartPropertyModel = new ChartPropertyModel();  // 创建控件属性实例
            ChartDataModel chartDataModel = new ChartDataModel();// 创建数据模型实例
            view = new RealTimeChartView(chartDataModel, chartPropertyModel);
            controller = new ChartController(view, chartDataModel);
            controller2 = new ChartPropertyController(view, chartPropertyModel);


            //view.OnStartRequested += controller.StartSampling;
            //view.OnStopRequested += controller.StopSampling;
            //view.OnOverallRequested += controller.OverallData;


            ////view.OnXMinLimitChangeRequested += controller2.

            // 这里不做事件订阅，等 view 完全初始化之后再进行
            // 设置 RealTimeChartView 的大小
            view.Size = new Size(600, 400);  // 根据需要调整大小
            Controls.Add(view);
        }
    }
}
