using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.Measure;
using System.Xml.Linq;

namespace LiveChart2ToFra.UpdateData.Models
{
    public class ChartDataModel : IChartDataModel
    {
        private readonly Random _random = new();
        private double _lastX;
        private double _currentMaxX = 10;
        public int PageSizePoints { get; set; } = 10; // 每页显示10个数据点

        public int CurrentPage { get; private set; }  // 这里已经是接口要求的get访问器
        public int TotalPages { get; set; }

        public ObservableCollection<ObservablePoint> Data1 { get; } = new();
        public ISeries[] Series1 { get; private set; }
        public ObservableCollection<ObservablePoint> Data2 { get; } = new();
        public ISeries[] Series2 { get; private set; }
        public ObservableCollection<ObservablePoint> Data3 { get; } = new();
        public ISeries[] Series3 { get; private set; }

        //表示图表的轴设置，通常用于控制图表的缩放和滚动。    图标上用到的轴和滚动条上的是同一个
        public Axis[] ScrollableAxes { get; } =
        { 
            new Axis() 
            {
                Name = "米数(m)",
                //NamePaint = new SolidColorPaint(SKColors.Black), // 明确设置名称颜色
                //NameTextSize = 16,       // 名称字体大小
                //NamePadding = new (0, 0, 0, 20), // 底部增加 20 像素间距
            }
        };

        //表示滚动条上的滑块，用来显示当前图表的可视区域。
        public RectangularSection[] Thumbs { get; } =
        {
            new RectangularSection
            {
                //Stroke = new SolidColorPaint(SKColors.Gray, 1),
                Fill = new SolidColorPaint(new SKColor(255, 205, 210, 100)),
                
            }
        };


        private Margin _margin = new Margin();
        public Margin Margin
        {
            get => _margin;
            set
            {
                _margin = value;
            }
        }
        public double CurrentMaxX => _currentMaxX;

        public event EventHandler DataUpdated;

        public ChartDataModel()
        {
            
        }

        public void AddDataPoint()
        {
            var y = _random.Next(40, 100);
            var x = _lastX + 0.1;
            _lastX = x;

            Data1.Add(new ObservablePoint(x, y));
            Data2.Add(new ObservablePoint(x, 80));
            Data3.Add(new ObservablePoint(x, 50));

            // 保持数据量
            if (Data1.Count > 3000) Data1.RemoveAt(0);
            if (Data2.Count > 3000) Data2.RemoveAt(0);
            if (Data3.Count > 3000) Data3.RemoveAt(0);

            // 更新X轴范围
            //if (x > _currentMaxX)
            //{
            //    _currentMaxX = x + 1;
            //    DataUpdated?.Invoke(this, EventArgs.Empty);
            //}
            TotalPages = (int)Math.Ceiling(Data1.Count / (double)PageSizePoints);
        }

        // 在非UI线程更新数据时需要使用Invoke
        //public void AddDataPoint(double x, double y)
        //{
        //    if (Data1 is ISynchronizeInvoke syncObj && syncObj.InvokeRequired)
        //    {
        //        syncObj.Invoke(() => Data1.Add(new ObservablePoint(x, y)));
        //    }
        //    else
        //    {
        //        Data1.Add(new ObservablePoint(x, y));
        //    }
        //}

        public void ClearData()
        {
            Data1.Clear();
            Data2.Clear();
            Data3.Clear();
            _lastX = 0;
            _currentMaxX = 10;

            // 触发坐标轴重置
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveOldestPoint()
        {
            if (Data1.Count > 0) Data1.RemoveAt(0);
            // 其他数据集同理...
        }

        /// <summary>
        /// 查看全局数据
        /// </summary>
        public void ViewOverallData()
        {

        }

        /// <summary>
        /// 更新可见范围
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void UpdateVisibleRange(double min, double max)
        {
            ScrollableAxes[0].MinLimit = min;
            ScrollableAxes[0].MaxLimit = max;
            Thumbs[0].Xi = min;
            Thumbs[0].Xj = max;
        }

        public void GoToPage(int page)
        {
            if (page < 1 || page > TotalPages) return;

            CurrentPage = page;
            UpdateAxisLimits2();
        }

        public void SeeAll()
        {
            ScrollableAxes[0].MinLimit = null;
            ScrollableAxes[0].MaxLimit = null;
            Thumbs[0].Xi = null;
            Thumbs[0].Xj = null;
        }

        private void UpdateAxisLimits()
        {
            var axis = ScrollableAxes[0];
            axis.MinLimit = (CurrentPage - 1) * PageSizePoints - 0.5;
            axis.MaxLimit = CurrentPage * PageSizePoints - 0.5;
        }

        private void UpdateAxisLimits2()
        {
            var axis = ScrollableAxes[0];
            int startIndex = (CurrentPage - 1) * PageSizePoints;
            int endIndex = Math.Min(startIndex + PageSizePoints - 1, Data1.Count - 1);

            // 添加边距保证第一个和最后一个点完整显示
            double margin = (double)((Data1[endIndex].X - Data1[startIndex].X) * 0.05);

            axis.MinLimit = Data1[startIndex].X- margin;
            axis.MaxLimit = Data1[endIndex].X + margin;
        }
    }
}
