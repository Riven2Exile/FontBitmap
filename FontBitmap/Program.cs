using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontBitmap
{
    class Program
    {
        static int BMP_WIDTH = 0;
        static int BMP_HEIGHT = 0;

        static int size = 60;
        static string fontName = "楷体";    //字体名字
        static bool isBold = false;     //是否加粗
        static string saveFileName = "f.bmp";


        static void Main(string[] args)
        {
            ////// 设置字体基本信息 

            BMP_WIDTH = 0xFF * size;
            BMP_HEIGHT = (0xFF - 0x80) * size;

            if (BMP_WIDTH % 1024 != 0)
                BMP_WIDTH = (BMP_WIDTH / 1024 + 1) * 1024;
            if (BMP_HEIGHT % 1024 != 0)
                BMP_HEIGHT = (BMP_HEIGHT / 1024 + 1) * 1024;
            //////


            Bitmap fontBmp = new Bitmap(BMP_WIDTH, BMP_HEIGHT, PixelFormat.Format32bppArgb);

            Console.WriteLine("正在生成字体 ...");
            GenerateBitmap(fontBmp);
            Console.WriteLine("正在保存字体图片文件 ...");
            fontBmp.Save(saveFileName);

            Console.WriteLine("完成.");
        }



        static void GenerateBitmap(Bitmap fontBmp)
        {
            Graphics g = Graphics.FromImage(fontBmp);

            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, BMP_WIDTH, BMP_HEIGHT));

            byte[] gbks = new byte[2];
            gbks[0] = 0x7F;
            gbks[1] = 0x00;

            for (int i = 0; i < 0x80; i++)
            {
                for (int j = 0; j < 0xFF; j++)
                {

                    Font font = new Font(fontName, size, GraphicsUnit.Pixel);
                    string str = Encoding.GetEncoding("GBK").GetString(gbks);

                    if (gbks[0] <= 0x80)
                        str = str.Substring(1, 1);

                    int x = j * (gbks[0] >= 0x80 ? size : (size / 2));
                    int y = i * size;

                    g.DrawString(str, font, Brushes.White, new PointF(x - 3, y - 1));

                    gbks[1]++;
                }

                gbks[0]++;
                gbks[1] = 0x00;
            }

            g.Dispose();
        }
    }
}
