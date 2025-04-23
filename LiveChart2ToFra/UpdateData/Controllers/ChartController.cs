using LiveChart2ToFra.UpdateData.Models;
using LiveChart2ToFra.UpdateData.Views;
using LiveChartsCore.Kernel.Sketches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveChart2ToFra.UpdateData.Controllers
{
    public class ChartController : IDisposable
    {
        private readonly RealTimeChartView _view;
        private readonly IChartDataModel _model;
        private readonly System.Windows.Forms.Timer _timer;

        private bool _isDragging;

        public ChartController(RealTimeChartView view, IChartDataModel model)
        {
            _view = view;
            _model = model;

            // 配置主图表
            //_view._chart.Series = _model.Series;
            _view._chart.XAxes = _model.ScrollableAxes;

            // 配置滚动条图表
            //_view._scrollbarChart.Series = _model.Series;
            _view._scrollbarChart.Sections = _model.Thumbs;

            //订阅视图中按钮点击事件
            _view.OnStartRequested += StartSampling;
            _view.OnStopRequested += StopSampling;
            _view.OnOverallRequested += OverallData;


            _view.ScrollbarMouseDown += OnScrollbarMouseDown;
            _view.ScrollbarMouseMove += OnScrollbarMouseMove;
            _view.ScrollbarMouseUp += OnScrollbarMouseUp;
            _view._chart.UpdateStarted += OnMainChartUpdated;

            _timer = new System.Windows.Forms.Timer { Interval = 100 };
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


        /// <summary>
        /// 主图表更新时（例如缩放或平移），更新模型的可见范围。
        /// </summary>
        /// <param name="chart"></param>
        private void OnMainChartUpdated(IChartView chart)
        {
            var xAxis = ((ICartesianChartView)chart).XAxes.First();
            // 添加空值保护
            if (xAxis.MinLimit == null || xAxis.MaxLimit == null) return;

            _model.UpdateVisibleRange(xAxis.MinLimit.Value, xAxis.MaxLimit.Value);

        }

        /// <summary>
        /// 滚动条鼠标按下，更新滚动条滑块位置，并更新图表的可见区域。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollbarMouseDown(object sender, MouseEventArgs e)
        {
            _isDragging = true;
            UpdateScrollPosition(e.Location);
        }

        /// <summary>
        /// 滚动条鼠标移动，更新滚动条滑块位置，并更新图表的可见区域。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollbarMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            UpdateScrollPosition(e.Location);
        }

        /// <summary>
        /// 滚动条鼠标抬起，更新滚动条滑块位置，并更新图表的可见区域。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollbarMouseUp(object sender, MouseEventArgs e) => _isDragging = false;

        /// <summary>
        /// 根据滚动条的位置更新可见范围，计算新的最小值和最大值，并通过模型更新图表的显示区域。
        /// </summary>
        /// <param name="location"></param>
        private void UpdateScrollPosition(System.Drawing.Point location)
        {
            var position = _view._scrollbarChart.ScalePixelsToData(new(location.X, location.Y));
            var currentRange = _model.Thumbs[0].Xj - _model.Thumbs[0].Xi;
            var newMin = position.X - currentRange / 2;
            var newMax = position.X + currentRange / 2;
            _model.UpdateVisibleRange((double)newMin, (double)newMax);
        }
    }
}
