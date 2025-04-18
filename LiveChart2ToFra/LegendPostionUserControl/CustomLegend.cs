using LiveChartsCore.Drawing.Layouts;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Layouts;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.SKCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveChartsCore;


namespace LiveChart2ToFra.LegendPostionUserControl
{
    //自定义图例布局
    public class CustomLegend : SKDefaultLegend
    {
        protected override Layout<SkiaSharpDrawingContext> GetLayout(Chart chart)
        {
            var stackLayout = new StackLayout
            {
                Orientation = ContainerOrientation.Vertical,  // 垂直方向排列
                Padding = new LiveChartsCore.Drawing.Padding(15, 4),  // 设置内边距
                HorizontalAlignment = Align.Start,  // 水平方向居左对齐
                VerticalAlignment = Align.Middle,  // 垂直方向居中对齐
            };

            // 通过 chart.Series 获取所有图表的系列数据，并根据是否显示在图例中（IsVisibleAtLegend）添加到布局中
            foreach (var series in chart.Series.Where(x => x.IsVisibleAtLegend))
                stackLayout.Children.Add(new LegendItem(series));  // 将每个系列的图例项加入布局

            return stackLayout;  // 返回自定义的布局
        }
    }


    /*************************************************使用示例**********************************************
     * public partial class View : UserControl
    {
        private readonly CartesianChart cartesianChart;

        public View()
        {
            InitializeComponent();
            Size = new System.Drawing.Size(50, 50);

            var viewModel = new ViewModel();

            cartesianChart = new CartesianChart
            {
                Series = viewModel.Series,  // 绑定数据系列
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,  // 图例位置在右侧
                Legend = new CustomLegend(),  // 使用自定义图例

                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(50, 50),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };

            Controls.Add(cartesianChart);
        }
    }

     * 
     */


}
