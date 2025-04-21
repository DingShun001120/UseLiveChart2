using LiveChart2ToFra.UpdateData.Models;
using LiveChart2ToFra.UpdateData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Controllers
{
    public class ChartController : IDisposable
    {
        private readonly RealTimeChartView _view;
        private readonly IChartDataModel _model;
        private readonly System.Windows.Forms.Timer _timer;

        public ChartController(RealTimeChartView view, IChartDataModel model)
        {
            _view = view;
            _model = model;

            //订阅视图中按钮点击事件
            _view.OnStartRequested += StartSampling;
            _view.OnStopRequested += StopSampling;
            _view.OnOverallRequested += OverallData;

            _timer = new System.Windows.Forms.Timer { Interval = 10 };
            _timer.Tick += (s, e) => _model.AddDataPoint();
        }

        public void StartSampling() => _timer.Start();
        public void StopSampling() => _timer.Stop();

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }


        // 添加清空方法
        public void ClearAllData()
        {
            _model.ClearData();
        }

        // 添加还原方法
        public void OverallData()
        {
            _model.ViewOverallData();
        }
    }
}
