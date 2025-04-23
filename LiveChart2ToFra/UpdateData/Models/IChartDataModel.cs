using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Models
{
    public interface IChartDataModel
    {
        // 数据集合
        ObservableCollection<ObservablePoint> Data1 { get; }
       
        ISeries[] Series1 { get; } //图表的系列数据
        ObservableCollection<ObservablePoint> Data2 { get; }
        ISeries[] Series2 { get; }
        ObservableCollection<ObservablePoint> Data3 { get; }
        ISeries[] Series3 { get; }

        /// <summary>
        /// 表示图表的轴设置，通常用于控制图表的缩放和滚动。
        /// </summary>
        Axis[] ScrollableAxes { get; }

        /// <summary>
        /// 表示滚动条上的滑块，用来显示当前图表的可视区域。
        /// </summary>
        RectangularSection[] Thumbs { get; }

        Margin Margin { get; set; }

        // 坐标轴配置
        double CurrentMaxX { get; }

        // 数据操作
        void AddDataPoint();
        void ClearData();
        void RemoveOldestPoint();
        void ViewOverallData();

        /// <summary>
        /// 更新可见范围
        /// </summary>
        /// <param name="min">当前图表中显示的最小值</param>
        /// <param name="max">当前图表中显示的最大值</param>
        void UpdateVisibleRange(double min, double max);

        // 事件
        event EventHandler DataUpdated;
    }
}
