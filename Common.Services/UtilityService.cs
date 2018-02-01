using Common.Util;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Services
{
    public class UtilityService
    {
        public byte[] GetImageByEncrypt(string encodeimg, out string mime)
        {
            try
            {
                //var imgDir = @"D:\code\Projects\速星创\server\SXC\SXC.WebApi\Images\";
                var imgDir = Function.GetImageDirectory();//ConfigHelper.GetSetting("ImagesPhysicalPath");

                var imgname = Base64DEncrypt.Base64ForUrlDecode(encodeimg);

                mime = MimeMapping.GetMimeMapping(imgname);

                var imgPath = imgDir + imgname;

                //var ext = Path.GetExtension(imgPath);

                if (!File.Exists(imgPath))
                {
                    imgPath = imgDir + "404Pic.png";

                    mime = MimeMapping.GetMimeMapping(imgPath);
                }

                var imgByte = File.ReadAllBytes(imgPath);

                //var imgStream = new MemoryStream(File.ReadAllBytes(imgPath));

                return imgByte;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GetGetQrCode(string content)
        {
            try
            {
                using (MemoryStream imgStream = new MemoryStream())
                {
                    QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);

                    QrCode qr;

                    int ModuleSize = 12;//大小

                    QuietZoneModules QuietZones = QuietZoneModules.Two;  //空白区域


                    if (qrEncoder.TryEncode(content, out qr))//对内容进行编码，并保存生成的矩阵  
                    {
                        //Brush b = new SolidBrush(Color.FromArgb(20, Color.Gray));
                        var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones), Brushes.Black, Brushes.Transparent);
                        //render.WriteToStream(qr.Matrix, ImageFormat.Png, imgStream);

                        //logo
                        DrawingSize dSize = render.SizeCalculator.GetSize(qr.Matrix.Width);
                        Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
                        Graphics g = Graphics.FromImage(map);
                        render.Draw(g, qr.Matrix);

                        //logo
                        //Image img = resizeImage(Image.FromFile(@"D:\code\learn\asp.net\0-part\webapi\WebApiTest\webapi1\Images\logo1.png"), new Size(100, 100));
                        //img.Save(@"D:\code\learn\asp.net\0-part\webapi\WebApiTest\webapi1\Images\qrlogo.png", ImageFormat.Png);

                        //Image img = Image.FromFile(@"D:\code\Projects\速星创\server\SXC\SXC.WebApi\Images\qrlogo.png");

                        Image img = Image.FromFile(Function.GetImagePath("qrlogo.png"));

                        Point imgPoint = new Point((map.Width - img.Width) / 2, (map.Height - img.Height) / 2);
                        g.DrawImage(img, imgPoint.X, imgPoint.Y, img.Width, img.Height);

                        map.Save(imgStream, ImageFormat.Png);

                        byte[] imgByte = imgStream.GetBuffer();

                        img.Dispose();
                        g.Dispose();
                        map.Dispose();

                        return imgByte;
                    }
                    else
                    {
                        throw new Exception("二维码生成失败");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
