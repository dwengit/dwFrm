using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Dw.Framework.Infrastructure.Utility
{
    public class ValidateCodeHelper
    {
        /// <summary>
        /// 生成随机验证码Code
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateVaildateCode(int len)
        {
            //设置允许出现的字符
            string chars = "abcdefghijklmnopqrstuvwxyz23456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //通过随机数生成验证码
            Random random = new Random();
            string validateString = "";
            for (int i = 0; i < len; i++)
            {
                validateString += chars[random.Next(chars.Length)];
            }
            return validateString;
        }
        /// <summary>
        /// 根据验证码Code绘制图片
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public static byte[] CreateValidateImgByCode(string validateCode)
        {
            int height = 40;
            int width = 140;
            //创建画布
            Bitmap bitmap = new Bitmap(width, height);
            //创建画笔
            Graphics graphics = Graphics.FromImage(bitmap);
            //给画布涂上背景色
            graphics.Clear(Color.White);
            //设置颜料板和调色刷
            RectangleF rf = new RectangleF(0, 0, bitmap.Width, bitmap.Height);
            LinearGradientBrush brush = new LinearGradientBrush(rf, Color.Red, Color.DarkBlue, 1.2f, true);
            //设置需要画到图中文字的格式（字体，大小，是否加粗，斜体）
            Font font = new Font("Consolas", 26, FontStyle.Bold | FontStyle.Italic);
            //将字画到画板中
            Random random = new Random();
            SizeF curCharSizeF;
            SizeF totalSizeF = graphics.MeasureString(validateCode, font);
            PointF startPointF = new PointF(0, (height - totalSizeF.Height) / 2);
            for (int i = 0; i < validateCode.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                graphics.DrawString(validateCode[i].ToString(), font, brush, startPointF);
                curCharSizeF = graphics.MeasureString(validateCode[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }

            //画图片的干扰线 
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);
                graphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //画图片的前景干扰点 
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            MemoryStream memoryStream = new MemoryStream();
            //将图片存到缓冲区中
            bitmap.Save(memoryStream, ImageFormat.Jpeg);

            //释放画板
            bitmap.Dispose();
            //返回byte[]
            return memoryStream.ToArray();
        }
    }
}
