using LiveChartsCore.Defaults;
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
        ObservableCollection<ObservablePoint> Data2 { get; }
        ObservableCollection<ObservablePoint> Data3 { get; }

        // 坐标轴配置
        double CurrentMaxX { get; }

        // 数据操作
        void AddDataPoint();
        void ClearData();
        void RemoveOldestPoint();
        void ViewOverallData();

        // 事件
        event EventHandler DataUpdated;
    }
}
