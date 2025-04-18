using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChart2ToFra.UpdateData.Models
{
    public interface IChartPropertyModel
    {
        int MaxLimit { get; set; }
        int MinLimit { get; set; }

        Font TitleFont { get; set; }
        void UpdateXRange(int xMin, int xMax);

        event EventHandler ConfigUpdated;
    }
}
