using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Models
{
    public class ChartPropertyModel : IChartPropertyModel
    {
        private int _minLimit = 0;
        private int _maxLimit = 10;
        private Font _titleFont = new Font("微软雅黑", 10);
        public int MaxLimit 
        { 
            get => _minLimit; 
            set
            {
                if(_minLimit.Equals(value)) return;
                _minLimit = value;
                //通知视图更新
                ConfigUpdated?.Invoke(this,EventArgs.Empty);
            }
        }
        public int MinLimit
        {
            get => _maxLimit;
            set
            {
                if (_maxLimit.Equals(value)) return;
                _maxLimit = value;
                ConfigUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
        public Font TitleFont
        {
            get => _titleFont;
            set
            {
                if (_titleFont.Equals(value)) return;
                _titleFont = value;
                ConfigUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ConfigUpdated;

        public void UpdateXRange(int xMin, int xMax)
        {
            MinLimit = xMin;
            MaxLimit = xMax;
            OnConfigUpdated();
        }

        protected virtual void OnConfigUpdated()
        {
            ConfigUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
