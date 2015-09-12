using System;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    ///ValidateCode 的摘要描述
    /// </summary>
    public class ValidateCode : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            CreateCheckCodeImage(GenerateCheckCode(context), context);
         }

        private String GenerateCheckCode(HttpContext context)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                do
                {
                    number = random.Next(259);
                    if (number % 2 == 0)
                        code = (char)('0' + (char)(number % 10));
                    else
                        code = (char)('A' + (char)(number % 26));
                } while ((new char[] { '0', 'O', 'I' }).Any(x => x.Equals(code)));
                checkCode += code.ToString();
            }

            //儲存在cookie
            //context.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));

            //儲存在session

            String validName = context.Request.QueryString["vn"];
            if (!String.IsNullOrEmpty(validName))
            {
                context.Session[validName] = checkCode;
            }
            else {
                //context.Session["CheckCode"] = checkCode;
            }

            return checkCode;
        }

        private void CreateCheckCodeImage(string checkCode, HttpContext context)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 16.0)), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //
                Random random = new Random();

                //清空背景色
                g.Clear(Color.White);

                //製作照片的背景噪音，採用畫線方式
                for (int i = 0; i < 4; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.FromArgb(random.Next(64, 128), random.Next(64, 128), random.Next(64, 128))), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 16, (FontStyle.Regular));
                SolidBrush Sbrush = new SolidBrush(Color.FromArgb(128, 64, 64));
                //System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                //    new Rectangle(0, 0, image.Width, image.Height),
                //    Color.DarkGray,
                //    Color.DarkRed,
                //    1.2f, true);

                g.DrawString(checkCode, font, Sbrush, 0, 0);

                //前景噪音
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //框
                g.DrawRectangle(new Pen(Color.FromArgb(192, 192, 255)), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());

                font.Dispose();
                Sbrush.Dispose();
                ms.Dispose();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}