using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Common.Domain;
using System.Linq;

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


        [TestMethod]
        public void TestTreeQuery()
        {
            //var data1 = GetSonID(110000).ToList();
            var data = GetSon(110000).ToList();

            var ss = data.Select(t=>t.Id.ToString()).Aggregate((x, y) => x + "," + y);

            var dd = data.Select(t => t.Id.ToString()).ToArray();

            int num = data.Count();
        }

        public IEnumerable<BaseArea> GetSonID(int p_id)
        {
            using (var db = new CommonContext())
            {
                db.Database.Log = Console.WriteLine;
                var query = from c in db.Set<BaseArea>()
                            where c.Pid == p_id
                            select c;

                return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
            }
        }

        public IEnumerable<BaseArea> GetSon(int p_id)
        {
            using (var db = new CommonContext())
            {
                db.Database.Log = Console.WriteLine;

                var data = db.Database.SqlQuery<BaseArea>($@"WITH temp
AS
(
SELECT * FROM Ryt_BaseArea  WHERE ID = {p_id}
UNION ALL
SELECT m.* FROM Ryt_BaseArea  AS m
INNER JOIN temp AS child ON m.PID = child.ID
)
SELECT * FROM temp");

                return data.ToList();

            }
        }


        [TestMethod]
        public void InitData_art()
        {
            using (var db = new CommonContext())
            {
                db.Set<Article>().AddRange(new List<Article>() {
                    new Article{ Type=-1, Title="404", Content=@"<p><img src='{host}/api/Image/NDA0LmpwZw2'></img></p>" },
                    new Article{ Type=0, Title="文章标题1", Content=@"<p>测试内容1</p>" },
                    new Article{ Type=0, Title="文章标题2", Content=@"<p>测试内容2</p>" },
                    new Article{ Type=0, Title="文章标题3", Content=@"<p>测试内容3</p>" },
                    new Article{ Type=0, Title="文章标题4", Content=@"<p>测试内容4</p>" },
                    new Article{ Type=0, Title="文章标题5", Content=@"<p>测试内容5</p>" },
                    new Article{ Type=0, Title="文章标题6", Content=@"<p>测试内容6</p>" },
                    new Article{ Type=0, Title="文章标题7", Content=@"<p>测试内容7</p>" },
                    new Article{ Type=0, Title="文章标题8", Content=@"<p>测试内容8</p>" }
                });

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void InitData_fund()
        {
            using (var db = new CommonContext())
            {
                //db.Set<Fund>().AddRange(new List<Fund>() {
                //    new Fund{ Name="中华基金会",Introduction="中华基金会介绍"},
                //    new Fund{ Name="医学基金会",Introduction="医学基金会介绍"},
                //});

                var fund1 = new Fund { Name = "中华基金会", Introduction = "中华基金会介绍" };
                db.Set<Fund>().Add(fund1);

                var fund2 = new Fund { Name = "医学基金会", Introduction = "医学基金会介绍" };
                db.Set<Fund>().Add(fund2);

                db.Set<FundProject>().AddRange(new List<FundProject>() {
                    new FundProject{  Name="项目1", Introduction="介绍1", Fund=fund1},
                    new FundProject{  Name="项目2", Introduction="介绍2", Fund=fund1},

                    new FundProject{  Name="项目3", Introduction="介绍3", Fund=fund2},
                    new FundProject{  Name="项目4", Introduction="介绍4", Fund=fund2},
                });

                db.SaveChanges();
            }
        }


        [TestMethod]
        public void InitData_mrecord()
        {
            using (var db = new CommonContext())
            {

                db.Set<MedicalRecord>().AddRange(new List<MedicalRecord>() {
                    new MedicalRecord{  Title="病历1", Content="内容1", FrontPic="Desert.jpg", MedicineCategoryID=1, Doctor=null},
                    new MedicalRecord{  Title="病历2", Content="内容2", FrontPic="Desert.jpg", MedicineCategoryID=1, Doctor=null},
                    new MedicalRecord{  Title="病历3", Content="内容3", FrontPic="Desert.jpg", MedicineCategoryID=2, Doctor=null},
                    new MedicalRecord{  Title="病历4", Content="内容4", FrontPic="Desert.jpg", MedicineCategoryID=2, Doctor=null},
                    new MedicalRecord{  Title="病历5", Content="内容5", FrontPic="Desert.jpg", MedicineCategoryID=3, Doctor=null},
                    new MedicalRecord{  Title="病历6", Content="内容6", FrontPic="Desert.jpg", MedicineCategoryID=3, Doctor=null},
                });

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void InitData_Conference()
        {
            using (var db = new CommonContext())
            {

                db.Set<Conference>().AddRange(new List<Conference>() {
                    new Conference{  Title="会议1", Content="内容1", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                    new Conference{  Title="会议2", Content="内容2", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                    new Conference{  Title="会议3", Content="内容3", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                    new Conference{  Title="会议4", Content="内容4", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                    new Conference{  Title="会议5", Content="内容5", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                    new Conference{  Title="会议6", Content="内容6", FrontPic="Desert.jpg", Address="北京", Country="中国", BeginDate=DateTime.Now.AddDays(10),EndDate=DateTime.Now.AddDays(20)},
                });

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void InitData_Video()
        {
            using (var db = new CommonContext())
            {

                db.Set<VideoInfo>().AddRange(new List<VideoInfo>() {
                    new VideoInfo{  Title="测试1", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                    new VideoInfo{  Title="测试2", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                    new VideoInfo{  Title="测试3", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                    new VideoInfo{  Title="测试4", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                    new VideoInfo{  Title="测试5", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                    new VideoInfo{  Title="测试6", Presenter="张三",Source="http://www.hexieyinan.com/SxcWebApi/Video/1huishijichu.mp4", Snapshot="http://www.hexieyinan.com/SxcWebApi/Video/img/1huishijichu.jpg",VideoSeriesID=1},
                });

                db.SaveChanges();
            }
        }


        [TestMethod]
        public void InitData_Survey()
        {
            using (var db = new CommonContext())
            {
                var s = new Survey { Title = "测试特殊3", BeginDate = DateTime.Now, Description = "测试33333333", Type=1 };

                var sq = new List<SurveyQuestion>() {
                    new SurveyQuestion(){  Survey=s, Number=1, Title="问题5", Type=1,
                        items =new List<SurveyQuestionOption>(){
                            new SurveyQuestionOption(){  Value="A", Content="aaaaaaa"},
                            new SurveyQuestionOption(){  Value="B", Content="bbbbbbb"},
                            new SurveyQuestionOption(){  Value="C", Content="ccccccc"},
                            new SurveyQuestionOption(){  Value="D", Content="dddddddd"},
                        }
                    },
                    new SurveyQuestion(){  Survey=s, Number=2, Title="问题6", Type=2,
                        items =new List<SurveyQuestionOption>(){
                            new SurveyQuestionOption(){  Value="A", Content="aaaaaaa"},
                            new SurveyQuestionOption(){  Value="B", Content="bbbbbbb"},
                            new SurveyQuestionOption(){  Value="C", Content="ccccccc"},
                            new SurveyQuestionOption(){  Value="D", Content="dddddddd"},
                        }
                    }
                };

                db.Set<Survey>().Add(s);
                db.Set<SurveyQuestion>().AddRange(sq);

                s = new Survey { Title = "测试特殊4", BeginDate = DateTime.Now, Description = "测试444444444",Type=1 };

                sq = new List<SurveyQuestion>() {
                    new SurveyQuestion(){  Survey=s, Number=1, Title="问题7", Type=1,
                        items =new List<SurveyQuestionOption>(){
                            new SurveyQuestionOption(){  Value="A", Content="aaaaaaa"},
                            new SurveyQuestionOption(){  Value="B", Content="bbbbbbb"},
                            new SurveyQuestionOption(){  Value="C", Content="ccccccc"},
                            new SurveyQuestionOption(){  Value="D", Content="dddddddd"},
                        }
                    },
                    new SurveyQuestion(){  Survey=s, Number=2, Title="问题8", Type=2,
                        items =new List<SurveyQuestionOption>(){
                            new SurveyQuestionOption(){  Value="A", Content="aaaaaaa"},
                            new SurveyQuestionOption(){  Value="B", Content="bbbbbbb"},
                            new SurveyQuestionOption(){  Value="C", Content="ccccccc"},
                            new SurveyQuestionOption(){  Value="D", Content="dddddddd"},
                        }
                    }
                };

                db.Set<Survey>().Add(s);
                db.Set<SurveyQuestion>().AddRange(sq);

                db.SaveChanges();
            }
        }
    }
}
