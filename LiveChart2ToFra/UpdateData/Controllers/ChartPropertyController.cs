using LiveChart2ToFra.UpdateData.Models;
using LiveChart2ToFra.UpdateData.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Controllers
{
    public class ChartPropertyController
    {
        private readonly RealTimeChartView _view;
        private readonly IChartPropertyModel _model;
        public ChartPropertyController(RealTimeChartView view, IChartPropertyModel chartProperty) 
        {
            _view = view;
            _model = chartProperty;

            //订阅模型更新事件，确保视图得到更新
            _model.ConfigUpdated += OnModelConfigUpdated;

            //订阅视图中按钮点击事件
            _view.OnXMinLimitChangeRequested += UpdateXMinLimit;
            _view.OnXMaxLimitChangeRequested += UpdateXMaxLimit;
            _view.OnFontChangeRequested += UpdateFont;
        }

        // 添加配置修改命令
        private void OnModelConfigUpdated(object sender, EventArgs e)
        {
            // 更新视图（例如更新图表的 X 和 Y 范围）
            _view.UpdateChart(_model.MinLimit, _model.MaxLimit);
        }


        private void UpdateXMinLimit(int minValue)
        {
            _model.MinLimit = minValue;
        }

        private void UpdateXMaxLimit(int maxValue)
        {
            _model.MaxLimit = maxValue;
        }

        private void UpdateFont(Font font)
        {
            _model.TitleFont = font;
        }
    }
}
