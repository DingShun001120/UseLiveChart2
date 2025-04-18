using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Drawing.Layouts;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using Padding = LiveChartsCore.Drawing.Padding;

namespace LiveChart2ToFra.LegendPostionUserControl
{
    public class LegendItem : StackLayout
    {
        public LegendItem(ISeries series)
        {
            // 设置布局方向为水平，图例项的边距
            Orientation = ContainerOrientation.Horizontal;
            Padding = new Padding(12, 6);
            VerticalAlignment = Align.Middle;
            HorizontalAlignment = Align.Middle;

            // 设置透明度，依据系列的可见性
            Opacity = series.IsVisible ? 1 : 0.5f;

            // 获取该系列的迷你图形（例如线条、图标等）
            var miniature = (IDrawnElement<SkiaSharpDrawingContext>)series.GetMiniatureGeometry(null);
            if (miniature is BoundedDrawnGeometry bounded)
                bounded.Height = 40;  // 设置迷你图形的高度

            //// 将迷你图形和标签添加到图例项中
            //Children = [
            //    miniature, // 迷你图形
            //    new LabelGeometry
            //    {
            //        Text = series.Name ?? "?",  // 如果没有设置名称，使用默认值
            //        TextSize = 20,  // 设置标签文字大小
            //        Paint = new SolidColorPaint(new SKColor(30, 30, 30)),  // 设置文本颜色
            //        Padding = new Padding(8, 2, 0, 2),  // 设置文本的内边距
            //        VerticalAlign = Align.Start,  // 文本垂直对齐方式
            //        HorizontalAlign = Align.Start  // 文本水平对齐方式
            //    }
            //];

            // 假设 Children 是一个 UIElementCollection
            Children.Add(miniature);
            Children.Add(new LabelGeometry
            {
                Text = series.Name ?? "?",
                TextSize = 20,
                Paint = new SolidColorPaint(new SKColor(30, 30, 30)),
                Padding = new Padding(8, 2, 0, 2),
                VerticalAlign = Align.Start,
                HorizontalAlign = Align.Start
            });
        }

    }
}
