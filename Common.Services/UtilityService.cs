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
using Common.Services.Dtos;
using Common.Domain;

namespace Common.Services
{
    public class UtilityService : ServiceBase
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

        public string SendSms(string mobile)
        {
            if (!ValidatorHelper.IsMobile(mobile))
            {
                return "手机号格式不正确";
            }

            string cacheKey = "SendSms" + mobile;

            string msg = string.Empty;
            try
            {
                var cache = CacheHelper.Get<SmsDto>(cacheKey);

                if (cache != null && cache.SendTime.AddSeconds(80) > DateTime.Now)
                {
                    msg = "请稍后再试";
                }
                else
                {
                    var sms = new SmsDto { MobilePhone=mobile, SmsCode=RandomHelper.GetNumRandomString(4) };

                    var res = Function.SendSms(sms);

                    if (string.IsNullOrEmpty(res))
                    {
                        sms.SendTime = DateTime.Now;

                        CacheHelper.Set(cacheKey, sms, DateTime.Now.AddMinutes(5));
                    }
                    else
                    {
                        msg = res;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        public dynamic Login(LoginDto login)
        {
            try
            {
                UserTokenDto userdto = null;
                string authid = string.Empty;
                string token = string.Empty;

                using (var db = base.NewDB())
                {
                    var data = db.Set<UserAuth>().FirstOrDefault(t => t.IdentityType == "mobile" && t.Identifier == login.MobilePhone);


                    if (data != null)
                    {
                        if (MD5Encrypt.GetStrMD5(login.PassWord) == data.User.Password)
                        {
                            authid = data.User.AuthID.ToString();
                        }
                        else
                        {
                            data.ErrorNum++;
                            db.SaveChanges();
                            throw new Exception("密码错误");
                        }

                    }
                    else
                    {
                        throw new Exception("用户不存在");
                    }

                    if (!authid.IsEmpty())
                    {
                        data.LastActiveTime = DateTime.Now;
                        data.ErrorNum = 0;
                        db.SaveChanges();

                        //token = Base64DEncrypt.Base64ForUrlEncode(authid + "#" + DateTime.Now.AddDays(7).Ticks.ToString());

                        token = TripleDESDEncrypt.Encrypt(authid + "#" + DateTime.Now.AddDays(7).Ticks.ToString());

                        userdto = new UserTokenDto
                        {
                            //id = dbitem.User.ID,
                            AuthId = authid,
                            Token = token
                        };
                    }
                }
                return userdto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public dynamic RegisterUser(RegisterDto register)
        {
            try
            {
                UserTokenDto userdto = null;
                string authid = string.Empty;
                string token = string.Empty;

                using (var db = base.NewDB())
                {
                    if (!register.MobilePhone.IsEmpty() && !register.VerifyCode.IsEmpty() && !register.PassWord.IsEmpty())
                    {
                        if (!CheckVerifyCode(register.MobilePhone, register.VerifyCode))
                        {
                            throw new Exception("验证码错误");
                        }
                        if (!db.Set<UserAuth>().Any(t => t.IdentityType == "mobile" && t.Identifier == register.MobilePhone))
                        {
                            User user = new User();

                            authid = user.AuthID.ToString();

                            user.UserName = register.MobilePhone;

                            user.Password = MD5Encrypt.GetStrMD5(register.PassWord);

                            user = db.Set<User>().Add(user);

                            UserProfile userpf = new UserProfile
                            {
                                User = user
                            };
                            db.Set<UserProfile>().Add(userpf);


                            UserAuth ua = new UserAuth
                            {
                                IdentityType = "mobile",
                                Identifier = register.MobilePhone,
                                User = user
                            };

                            db.Set<UserAuth>().Add(ua);

                            UserIntegral ui = new UserIntegral
                            {
                                User = user
                            };
                            db.Set<UserIntegral>().Add(ui);

                            Doctor d = new Doctor
                            {
                                User = user
                            };
                            db.Set<Doctor>().Add(d);

                            if (!string.IsNullOrEmpty(register.InvitationCode))
                            {
                                //邀请码
                            }

                            db.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("用户已存在");
                        }

                        //dbitem.LastActiveTime = DateTime.Now;

                        //var auth = db.Set<UserAuth>().FirstOrDefault(t => t.IdentityType == "mobile" && t.Identifier == register.MobilePhone);
                        if (!authid.IsEmpty())
                        {
                            token = TripleDESDEncrypt.Encrypt(authid + "#" + DateTime.Now.AddDays(7).Ticks.ToString());

                            userdto = new UserTokenDto
                            {
                                //id = dbitem.User.ID,
                                AuthId = authid,
                                Token = token
                            };
                        }



                        //var signin = db.Set<IntegralSignIn>().FirstOrDefault(t => t.UserIntegral.User.Id == dbitem.User.Id);

                        //if (signin != null && signin.LastTime != null && signin.LastTime.Value.Date == DateTime.Now.Date)
                        //{
                        //    userdto.issignin = true;
                        //}


                    }
                    
                }

                return userdto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckVerifyCode(string mobilePhone, string verifyCode)
        {
            string cacheKey = "SendSms" + mobilePhone;
            var cache = CacheHelper.Get<SmsDto>(cacheKey);

            if (cache != null&&cache.SmsCode== verifyCode)
            {
                return true;
            }
            return false;
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
