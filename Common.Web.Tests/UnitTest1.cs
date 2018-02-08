using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Common.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

        [TestMethod]
        public void TestAESDecrypt()
        {
            var AesIV = "DXZYgdrcYmlG0II2KCRq2w==";//"ZihTz2XxG5t+/Od/Meb2pg==";
            var AesKey = "ZihTz2XxG5t+/Od/Meb2pg==";//"DXZYgdrcYmlG0II2KCRq2w==";
            string text = "kv0LsHGLkguw9o9FO7pGUUM7X1EeIWrm+hKd/jD/VL2hMM1KSJ+Fa/GimHxdKh+8luxEx2209WIaIxG/GpaP62KXyv/BS1K9R2meLMqx07TuQk3WTfBd/5bnFaecSqEFL9MhRtqNHggjsTJmiHALKR6xDnsC8mGTG3iePkBd0XysoFKmLtxDAYHGZBUfr6pxJbfuqH8kfvV17VY/q2BIkpIUw0pimU7wOZtbbBn468UrcxQynw4kQ/d/YNu3t3HPMrFQOI2X9OrJTxrQ9Q/Qqeni9skYKzWTnVe1RuuLMhP7niWvY1ag01KZWwPFIpj33m77/405LFo32QINI0OQlDl4fq6C6pFykqf1vpWEWmpV+3lGrnue9Q0qJbDPaU8Hn7ehP19cTIm6q1uAKUGq6KSmx3kWhhtLFa6jMN5eNZeKZqpOx61e/BkJxd+X9P9klKenDFz63Xgf+aRpp/h04tybpz/tKTDoo70be4Z08Nc=";
            string ss = new WxService().AESDecrypt(text, AesKey, AesIV);

            Console.Write(ss);
        }

        [TestMethod]
        public void TestWxuser()
        {
            var ws = new WxService();

            dynamic res = new System.Dynamic.ExpandoObject();

            res.openid = "o3t1F4wqt_WlmlyrmWOC_YRz9g5I";
            res.session_key = "ChMraH9tzS5ZYV2B0r5ftQ==";
            //res.userinfo = @"o3t1F4wqt_WlmlyrmWOC_YRz9g5I";

            string sqlString = "{\n" +
            "  \"openId\": \"o3t1F4wqt_WlmlyrmWOC_YRz9g5I\",\n" +
            "  \"nickName\": \"刘元华\",\n" +
            "  \"gender\": 1,\n" +
            "  \"language\": \"zh_CN\",\n" +
            "  \"city\": \"Linyi\",\n" +
            "  \"province\": \"Shandong\",\n" +
            "  \"country\": \"China\",\n" +
            "  \"avatarUrl\": \"https://wx.qlogo.cn/mmopen/vi_32/DYAIOgq83eofQLC4xic5qg1JyibZAkktSLGVDdKB3sBljvGQCUTJdOgCmMWBfOyXiaLuu0XNicqLUYlGqpDjw2v7kQ/0\",\n" +
            "  \"watermark\": {\n" +
            "    \"timestamp\": 1518095239,\n" +
            "    \"appid\": \"wx845cc082612156f6\"\n" +
            "  }}";

            res.userinfo = (JObject)JsonConvert.DeserializeObject(sqlString);
            res.sharecode = null;

            var sss=ws.GetUser(res);

            Console.WriteLine("123");
        }
    }
}
