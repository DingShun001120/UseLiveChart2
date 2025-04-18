using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveChart2ToFra
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //关于LiveCharts配置 ： 可以全局设置图表主题、字体、RTL 支持、数据映射器等
            LiveCharts.Configure(config =>
               config
            // you can override the theme 
            //作用：切换为 LiveCharts 提供的深色主题。  默认是浅色（LightTheme）。
            // .AddDarkTheme()   

            // In case you need a non-Latin based font, you must register a typeface for SkiaSharp
            //作用：在 SkiaSharp 渲染时注册一套支持中文（或其他语言）字符的字体。
            //如果你要显示中文、日文、韩文、阿拉伯文、俄文等，必须指定对应字体，否则文字会无法显示或乱码。
            //MatchCharacter('汉') 会自动寻找本地支持该字符的字体。
            .HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')) // <- Chinese 
            //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('あ')) // <- Japanese 
            //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('헬')) // <- Korean 
            //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('Ж'))  // <- Russian 

            //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('أ'))  // <- Arabic 
            //如果你的界面是右到左语言（例如阿拉伯语或希伯来语），可以启用这个设置，LiveCharts 将自动调整提示文字方向。
            //.UseRightToLeftSettings() // Enables right to left tooltips

            // finally register your own mappers
            // you can learn more about mappers at:
            // https://livecharts.dev/docs/winforms/2.0.0-rc2/Overview.Mappers

            // here we use the index as X, and the population as Y 
            //自定义数据类型的映射器（Mapper）
            //作用：告诉 LiveCharts 如何把自定义的数据类映射到图表的点坐标。
            //这个例子里定义了一个 City 类型，它有 Name 和 Population 两个字段。
            //index 作为 X 值，city.Population 作为 Y 值，用于图表上绘图。
            //.HasMap<City>((city, index) => new(index, city.Population))
            //.HasMap<DataPoint>((data , index) => new(data.Meter, data.Value))
            // .HasMap<Foo>( .... ) 
            // .HasMap<Bar>( .... ) 

            /****自定义数据类型的映射器示例*****/
            /*
             * public record City(string Name, double Population);
             * 
             * *****自己的类***********
             * public class SensorData
                {
                    public DateTime Time { get; set; }
                    public double Temperature { get; set; }
                }

             * *****使用*************
             * .HasMap<SensorData>((data, index) => new(data.Time.Ticks, data.Temperature))
             */
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
