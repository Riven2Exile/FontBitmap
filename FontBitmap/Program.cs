using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 字体大小 pt 跟像素 px 的转化  http://blog.csdn.net/gui597651737/article/details/7897439

namespace FontBitmap
{
    class Program
    {
        static int BMP_WIDTH = 0;
        static int BMP_HEIGHT = 0;

        static int x_edge = 1; //x_edge个像素做边缘
        static int y_edge = 0;

        static int size = 12;
        static string fontName = "宋体";    //字体名字 比如 楷体 24
        static bool isBold = false;     //是否加粗
        static string saveFileName = "f.bmp";
        static bool bDiffRender = false;    //使用不同的渲染方式

        static void Main(string[] args)
        {
            ////// 设置字体基本信息 

            BMP_WIDTH = 0xFF * size;
            BMP_HEIGHT = (/*0xFF -*/ 0x80) * size;

            // 先注释掉
            //             if (BMP_WIDTH % 1024 != 0)
            //                 BMP_WIDTH = (BMP_WIDTH / 1024 + 1) * 1024;
            //             if (BMP_HEIGHT % 1024 != 0)
            //                 BMP_HEIGHT = (BMP_HEIGHT / 1024 + 1) * 1024;
            //////

            FontFamily[] fontFamilies = { new FontFamily("楷体"), new FontFamily("宋体"), new FontFamily("微软雅黑"), new FontFamily("黑体") };  //定义一个装载字体信息的字体类数组
            int[] font_size = { 16
                                  //, 18, 24, 36, 48, 60, 72
                              };

            TextRenderingHint[] textRender = { TextRenderingHint.SystemDefault, TextRenderingHint.SingleBitPerPixelGridFit, TextRenderingHint.SingleBitPerPixel,
                                             TextRenderingHint.AntiAliasGridFit, TextRenderingHint.AntiAlias, TextRenderingHint.ClearTypeGridFit};

            //通过InstalledFontCollection类的Families属性来获取系统安装的所有字体
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            // Get the array of FontFamily objects.
            //fontFamilies = installedFontCollection.Families;   //【枚举系统字体!!!】

            //for (int i = 0; i < fontFamilies.Length; ++i)
            {
                //fontName = fontFamilies[i].Name;

                // 计算bitmap 的 size ,
                //for (int j = 0; j < font_size.Length; ++j)
                {
                    //size = font_size[j];
                    BMP_WIDTH = 0xFF * size + x_edge * 2;
                    BMP_HEIGHT = (/*0xFF -*/ 0x80) * size + y_edge * 2;

                    Bitmap fontBmp = new Bitmap(BMP_WIDTH, BMP_HEIGHT, PixelFormat.Format32bppArgb);

                    Console.WriteLine("正在生成字体 ... " + fontName + " " + size);
                    GenerateBitmap(fontBmp);
                    Console.WriteLine("正在保存字体图片文件 ...");
                    saveFileName = "font_gen/" + fontName + "_" + size + ".bmp";
                    fontBmp.Save(saveFileName);
                }

            }


//             Bitmap fontBmp = new Bitmap(BMP_WIDTH, BMP_HEIGHT, PixelFormat.Format32bppArgb);
// 
//             Console.WriteLine("正在生成字体 ...");
//             GenerateBitmap(fontBmp);
//             Console.WriteLine("正在保存字体图片文件 ...");
//             fontBmp.Save(saveFileName);

            Console.WriteLine("完成.");
        }



        static void GenerateBitmap(Bitmap fontBmp)
        {
            try
            {
                Graphics g = Graphics.FromImage(fontBmp);
                g.Clear(Color.FromArgb(0x00ffffff));

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; 

                // g.FillRectangle(Brushes.Black, new Rectangle(0, 0, BMP_WIDTH, BMP_HEIGHT));

                byte[] gbks = new byte[2];
                gbks[0] = 0x7F;
                gbks[1] = 0x00;

                Font font = new Font(fontName, size, GraphicsUnit.Pixel); //Point 字号 pt
                Console.WriteLine(font.Size.ToString() + " " + font.Style.ToString());

                //Console.WriteLine("Bold:" + font.Bold.ToString());
                //SolidBrush b = new SolidBrush(Color.Black);
                for (int i = 0; i < 0x80; i++)
                {
                    for (int j = 0; j < 0xFF; j++)
                    {
                        string str = Encoding.GetEncoding("GBK").GetString(gbks);

                        if (gbks[0] <= 0x80)
                            str = str.Substring(1, 1);

                        int x = j * (gbks[0] >= 0x80 ? size : (size / 2));
                        int y = i * size;

                        g.DrawString(str, font, Brushes.White, new PointF(x + x_edge, y + y_edge));

                        gbks[1]++;
                    }

                    gbks[0]++;
                    gbks[1] = 0x00;
                }

                g.Dispose();
            }
            catch (System.ArgumentException e)
            {

            }

        }



        //
        static void GenBitmap(Bitmap fontBmp, Graphics g)
        {
            try
            {
                g.Clear(Color.FromArgb(0x00ffffff));
                byte[] gbks = new byte[2];
                gbks[0] = 0x7F;
                gbks[1] = 0x00;

                Font font = new Font(fontName, size, GraphicsUnit.Pixel); //Point 字号 pt
                Console.WriteLine(font.Size.ToString() + " " + font.Style.ToString());

                for (int i = 0; i < 0x80; i++)
                {
                    for (int j = 0; j < 0xFF; j++)
                    {
                        string str = Encoding.GetEncoding("GBK").GetString(gbks);

                        if (gbks[0] <= 0x80)
                            str = str.Substring(1, 1);

                        int x = j * (gbks[0] >= 0x80 ? size : (size / 2));
                        int y = i * size;

                        g.DrawString(str, font, Brushes.Black, new PointF(x - 3, y - 1));

                        gbks[1]++;
                    }

                    gbks[0]++;
                    gbks[1] = 0x00;
                }

                g.Dispose();
            }
            catch (System.ArgumentException e)
            {

            }
        } // end GenBitmap

    }
}
