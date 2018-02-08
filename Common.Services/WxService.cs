using Common.Domain;
using Common.Services.Dtos;
using Common.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class WxService : ServiceBase
    {
        public dynamic GetWxUser(string code, string iv, string encryptedData)
        {
            string Appid = Base64DEncrypt.Base64ForUrlDecode(ConfigHelper.GetConfig("Wx_Appid"));//"wx12a7e3bf7a31d815";
            string Secret = ConfigHelper.GetConfig("Wx_Secret"); //ConfigurationManager.AppSettings["Wx_Secret"]; //"b4722d8a6a5629ed7717e58d1af431ba"; 
            string grant_type = "authorization_code";


            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid  
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Appid + "&secret=" + Secret + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";

            string html = HtmlHelper.GetHtml(url, type);//获取微信服务器返回字符串 

            //将字符串转换为json格式 
            JObject jo = (JObject)JsonConvert.DeserializeObject(html);

            dynamic res = new System.Dynamic.ExpandoObject();

            try
            {
                //微信服务器验证成功 
                res.openid = jo["openid"].ToString();
                res.session_key = jo["session_key"].ToString();
            }
            catch (Exception)
            {
                //微信服务器验证失败 
                res.errcode = jo["errcode"].ToString();
                res.errmsg = jo["errmsg"].ToString();
            }
            if (((IDictionary<string, object>)res).ContainsKey("openid") && !string.IsNullOrEmpty(res.openid))
            {
                var decryptres = AESDecrypt(encryptedData, res.session_key, iv);

                JObject userInfo = (JObject)JsonConvert.DeserializeObject(decryptres);

                res.userinfo = userInfo;
            }

            return res;
        }

        public string AESDecrypt(string inputdata, string AesKey, string AesIV)
        {
            try
            {
                AesIV = AesIV.Replace(" ", "+");
                AesKey = AesKey.Replace(" ", "+");
                inputdata = inputdata.Replace(" ", "+");

                byte[] encryptedData = Convert.FromBase64String(inputdata);

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(AesKey); // Encoding.UTF8.GetBytes(AesKey); 
                rijndaelCipher.IV = Convert.FromBase64String(AesIV);// Encoding.UTF8.GetBytes(AesIV); 
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);

                return result;
            }
            catch (Exception)
            {
                return null;

            }
        }

        public UserDto GetUser(dynamic wxuser)
        {
            if (((IDictionary<string, object>)wxuser).ContainsKey("openid") && !string.IsNullOrEmpty(wxuser.openid))
            {
                using (var db = base.NewDB())
                {
                    string openid = wxuser.openid;

                    var dbitem = db.Set<UserAuth>().FirstOrDefault(t => t.IdentityType == "wx" && t.Identifier == openid);

                    if (dbitem == null)
                    {
                        User user = new User();
                        user = db.Set<User>().Add(user);
                        //db.SaveChanges();

                        UserProfile userpf = new UserProfile
                        {
                            Id = user.Id,
                            NickName = wxuser.userinfo["nickName"].ToString(),
                            AvatarUrl = wxuser.userinfo["avatarUrl"].ToString()
                        };
                        db.Set<UserProfile>().Add(userpf);


                        UserAuth ua = new UserAuth
                        {
                            IdentityType = "wx",
                            Identifier = openid,
                            User = user
                        };

                        db.Set<UserAuth>().Add(ua);

                        UserIntegral ui = new UserIntegral
                        {
                            User = user
                        };
                        db.Set<UserIntegral>().Add(ui);

                        Patient p = new Patient
                        {
                            User = user
                        };
                        db.Set<Patient>().Add(p);

                        try
                        {
                            if (!string.IsNullOrEmpty(wxuser.sharecode))
                            {
                                
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        db.SaveChanges();

                        dbitem = db.Set<UserAuth>().FirstOrDefault(t => t.IdentityType == "wx" && t.Identifier == openid);
                    }

                    dbitem.LastActiveTime = DateTime.Now;
                    db.SaveChanges();
                    //dbitem = db.UserAuths.FirstOrDefault(t => t.IdentityType == "wx" && t.Identifier == openid);

                    UserDto userdto = new UserDto
                    {
                        //id = dbitem.User.ID,
                        authid = dbitem.User.AuthID,
                        //agentcode = dbitem.User.UserProfile.AgentCode, 
                        //agentcode = dbitem.User.Agent.Code, 
                        //isagentvalid = dbitem.User.Agent.IsValid,
                        isvalid = dbitem.User.IsValid,
                        isverified = dbitem.User.UserProfile.IsVerified
                    };

                    if (dbitem.User.UserProfile.NickName != wxuser.userinfo["nickName"].ToString())
                    {
                        dbitem.User.UserProfile.NickName = wxuser.userinfo["nickName"].ToString();
                    }

                    if (dbitem.User.UserProfile.AvatarUrl != wxuser.userinfo["avatarUrl"].ToString())
                    {
                        dbitem.User.UserProfile.AvatarUrl = wxuser.userinfo["avatarUrl"].ToString();
                    }

                    if (db.Entry(dbitem.User.UserProfile).State == EntityState.Modified)
                    {
                        db.SaveChanges();
                    }

                    var signin = db.Set<IntegralSignIn>().FirstOrDefault(t => t.UserIntegral.User.Id == dbitem.User.Id);

                    if (signin != null && signin.LastTime != null && signin.LastTime.Value.Date == DateTime.Now.Date)
                    {
                        userdto.issignin = true;
                    }

                    //return new ArticleDto { content = dbitem == null ? null : dbitem.Content };
                    return userdto;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
