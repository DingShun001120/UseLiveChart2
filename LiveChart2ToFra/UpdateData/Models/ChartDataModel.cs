using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Models
{
    public class ChartDataModel : IChartDataModel
    {
        private readonly Random _random = new();
        private double _lastX;
        private double _currentMaxX = 10;

        public ObservableCollection<ObservablePoint> Data1 { get; } = new();
        public ObservableCollection<ObservablePoint> Data2 { get; } = new();
        public ObservableCollection<ObservablePoint> Data3 { get; } = new();
        public double CurrentMaxX => _currentMaxX;

        public event EventHandler DataUpdated;

        public void AddDataPoint()
        {
            var y = _random.Next(40, 100);
            var x = _lastX + 0.1;
            _lastX = x;

            Data1.Add(new ObservablePoint(x, y));
            Data2.Add(new ObservablePoint(x, 80));
            Data3.Add(new ObservablePoint(x, 50));

            // 保持数据量
            if (Data1.Count > 10000) Data1.RemoveAt(0);
            if (Data2.Count > 10000) Data2.RemoveAt(0);
            if (Data3.Count > 10000) Data3.RemoveAt(0);

            // 更新X轴范围
            //if (x > _currentMaxX)
            //{
            //    _currentMaxX = x + 1;
            //    DataUpdated?.Invoke(this, EventArgs.Empty);
            //}
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
    }
}
