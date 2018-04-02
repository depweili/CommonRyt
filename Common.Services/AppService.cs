using Common.Domain;
using Common.Services.Dtos;
using Common.Util;
using Common.Util.Extesions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Services
{
    public class AppService : ServiceBase
    {
        

        public UserProfileDto GetUserProfile(Guid authid)
        {
            using (var db = base.NewDB())
            {
                var user = db.Set<User>().FirstOrDefault(u => u.AuthID == authid && u.IsValid == true);

                if (user != null)
                {
                    var userpf = user.UserProfile;

                    UserProfileDto userpfdto = new UserProfileDto
                    {
                        authid = authid,
                        nickname = userpf.NickName,
                        realname = userpf.RealName,
                        gender = userpf.Gender,
                        address = userpf.Address,
                        idcard = userpf.IDCard,
                        mobilephone = userpf.MobilePhone,
                        isverified = userpf.IsVerified ?? false,
                        birthday = userpf.BirthDay,
                        area = userpf.Area,
                        username=user.UserName
                    };

                    return userpfdto;
                }
                else
                {
                    return null;
                }
            }
        }

        public string PostComment(Guid authid, CommentDto comment)
        {
            string msg = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var user = db.Set<User>().FirstOrDefault(u => u.AuthID == authid && u.IsValid == true);

                    if (user != null)
                    {
                        Comment c = new Comment { Content = comment.Content, SubjectKey = comment.SubjectKey, User = user };
                        db.Set<Comment>().Add(c);

                        db.SaveChanges();
                    }
                    else
                    {
                        msg = "请登陆后评论";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return msg;
        }

        public List<CommentListDto> GetComments(Guid subjectKey, string queryJson)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var expression = LinqExtensions.True<Comment>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);
                    expression = expression.And(t => t.SubjectKey == subjectKey);

                    var query = db.Set<Comment>().Where(expression).OrderByDescending(t => t.CreateTime);

                    var list = Function.GetPageData(query, queryParam);

                    var res = list.MapToList<CommentListDto>();

                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public dynamic GetMedicineCategory()
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var data = db.Set<MedicineCategory>().OrderBy(t => t.Order).Select(t => new { t.Id, t.Name });

                    return data.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetMedicalRecords(string queryJson)
        {
            string cacheKey = "GetMedicalRecords" + queryJson;
            try
            {
                var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                if (res != null)
                {
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }

            using (var db = base.NewDB())
            {
                IEnumerable<ItemInformationDto> dblist = null;

                var expression = LinqExtensions.True<MedicalRecord>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                if (!queryParam["MedicineCategory"].IsEmpty() )
                {
                    int keyword = queryParam["MedicineCategory"].ToInt();

                    expression = expression.And(t => t.MedicineCategoryID == keyword);
                }

                if (!queryParam["FundProjectUid"].IsEmpty() )
                {
                    Guid keyword = queryParam["FundProjectUid"].ToString().ToGuid();

                    var expression1 = LinqExtensions.True<FundMedicalRecord>();
                    expression1 = expression1.And(t => t.FundProject.FundProjectUid == keyword);

                    var querypm = db.Set<FundMedicalRecord>().Where(expression1).Select(t => t.MedicalRecordID);

                    expression.And(t => querypm.Contains(t.Id));
                }

                var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                var query = db.Set<MedicalRecord>().Where(expression).OrderByDescending(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                {
                    Author = t.Doctor.Name,
                    Title = t.Title,
                    ClicksCount = t.ClicksCount,
                    CommentsCount = t.CommentsCount,
                    //FrontPic = string.IsNullOrEmpty(t.FrontPic) ? null : StaticPicUrlHost + t.FrontPic,
                    FrontPic = !string.IsNullOrEmpty(t.FrontPic) && !t.FrontPic.StartsWith("http") ? StaticPicUrlHost + t.FrontPic : t.FrontPic,

                    Score = t.Score,
                    Uid = t.MedicalRecordUid,
                    Type = "MedicalRecord",
                    CreateTime = t.CreateTime
                });


                var list = Function.GetPageData(query, queryParam);

                var res = list.ToList();

                CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                return res;
            }
        }


        public dynamic GetMyMedicalRecords(Guid uid, string queryJson)
        {
            string cacheKey = "GetMyMedicalRecords" +uid.ToString()+ queryJson;
            try
            {
                var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                if (res != null)
                {
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }

            using (var db = base.NewDB())
            {
                IEnumerable<ItemInformationDto> dblist = null;

                var expression = LinqExtensions.True<MedicalRecord>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true&&t.Doctor.User.AuthID== uid);

                if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                {
                    int keyword = queryParam["MedicineCategory"].ToInt();

                    expression = expression.And(t => t.MedicineCategoryID == keyword);
                }


                var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                var query = db.Set<MedicalRecord>().Where(expression).OrderByDescending(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                {
                    Author = t.Doctor.Name,
                    Title = t.Title,
                    ClicksCount = t.ClicksCount,
                    CommentsCount = t.CommentsCount,
                    FrontPic = string.IsNullOrEmpty(t.FrontPic) ? null : StaticPicUrlHost + t.FrontPic,
                    Score = t.Score,
                    Uid = t.MedicalRecordUid,
                    Type = "MedicalRecord",
                    CreateTime = t.CreateTime
                });


                var list = Function.GetPageData(query, queryParam);

                var res = list.ToList();

                CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                return res;
            }
        }

        public dynamic GetMedicalRecord(Guid uid)
        {
            string cacheKey = "GetMedicalRecord" + uid.ToString();
            try
            {
                
                try
                {
                    var res = CacheHelper.Get<MedicalRecordDto>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var data = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == uid);

                    var res = data.MapTo<MedicalRecordDto>();

                    var host = Function.GetHostAndApp();

                    res.Images = db.Set<ImageInfo>().Where(t => t.SubjectKey == uid).OrderBy(t=>t.ImageName).Select(t => host + "/" + t.ImagePath).ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string PostMedicalRecord(Guid authid, MedicalRecordDto mr)
        {
            string res = string.Empty;

            try
            {
                using (var db = base.NewDB())
                {
                    if (string.IsNullOrEmpty(mr.StrMedicalRecordUid))
                    {
                        var doctor = db.Set<Doctor>().FirstOrDefault(u => u.User.AuthID == authid && u.IsValid == true);

                        if (doctor != null)
                        {
                            MedicalRecord c = new MedicalRecord
                            {
                                MedicineCategoryID = mr.MedicineCategoryID,
                                MedicalHistory = mr.MedicalHistory,
                                Content = mr.Content,
                                Diagnosis = mr.Diagnosis,
                                Inspection = mr.Inspection,
                                PhysicalExamination = mr.PhysicalExamination,
                                Title = mr.Title,
                                DoctorID = doctor.Id
                            };
                            db.Set<MedicalRecord>().Add(c);

                            res = c.MedicalRecordUid.ToString();

                            if (!string.IsNullOrEmpty(mr.StrFundProjectUid))
                            {
                                var prjuid = mr.StrFundProjectUid.ToGuid();
                                var prj = db.Set<FundProject>().FirstOrDefault(t => t.FundProjectUid == prjuid);

                                if (prj != null)
                                {
                                    FundMedicalRecord fmr = new FundMedicalRecord
                                    {
                                        MedicalRecord = c,
                                        FundProject = prj
                                    };

                                    db.Set<FundMedicalRecord>().Add(fmr);
                                }
                            }

                        }
                        else
                        {
                            throw new Exception("用户验证失败");
                        }
                    }
                    else  //修改（未用）
                    {
                        
                        var mruid = mr.StrMedicalRecordUid.ToGuid();
                        var dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid==mruid);

                        dbitem = mr.MapTo(dbitem);

                        res = mr.StrMedicalRecordUid;
                    }

                    db.SaveChanges();

                    CacheHelper.Clear("GetMyMedicalRecords");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadImages(Guid uid,string type, Guid subjectUid, HttpRequestMessage request)
        {
            string res = string.Empty;
            try
            {
                if (request.Content.IsMimeMultipartContent())
                {
                    using (var db = base.NewDB())
                    {
                        EntityBase<int> dbitem;
                        switch (type.ToLower())
                        {
                            case "medicalrecord":
                                dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == subjectUid);
                                break;
                            default:
                                dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == subjectUid);
                                break;
                        }

                        var filelist = await Function.SaveImagesAsync(request, "MedicalRecord", subjectUid.ToString(), dbitem.CreateTime);

                        SaveImageInfo(db, filelist, subjectUid);

                        db.SaveChanges();
                    }
                    
                }
                else
                {
                    res = "没有文件";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return res;
        }

        public string UploadImages(HttpRequest httpRequest)
        {
            string res = string.Empty;
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    var subjectUid = httpRequest.Form["subjectUid"].ToGuid();
                    var type= httpRequest.Form["type"];

                    if (!string.IsNullOrEmpty(type) && subjectUid != default(Guid))
                    {
                        using (var db = base.NewDB())
                        {
                            EntityBase<int> dbitem;

                            var dir = string.Empty;
                            switch (type.ToLower())
                            {
                                case "medicalrecord":
                                    dir = "MedicalRecord";
                                    dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == subjectUid);
                                    break;
                                default:
                                    dir = "MedicalRecord";
                                    dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == subjectUid);
                                    break;
                            }

                            if (!string.IsNullOrEmpty(dir))
                            {
                                var filelist = Function.SaveImages(httpRequest, dir, subjectUid.ToString(), dbitem.CreateTime);

                                if (filelist.Count > 0)
                                {
                                    var host = Function.GetHostAndApp();

                                    var frontPic = host + "/" + Function.PathToRelativeUrl(filelist[0]);

                                    switch (type.ToLower())
                                    {
                                        case "medicalrecord":
                                            (dbitem as MedicalRecord).FrontPic = frontPic;
                                            break;
                                        default:
                                            (dbitem as MedicalRecord).FrontPic = frontPic;
                                            break;
                                    }

                                    SaveImageInfo(db, filelist, subjectUid);
                                }
                                

                                db.SaveChanges();
                            }
                            else
                            {
                                res = "未识别参数";
                            }
                            
                        }
                    }
                    else
                    {
                        res = "缺少参数";
                    }
                }
                else
                {
                    res = "没有文件";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return res;
        }

        



        //记录图片信息
        private void SaveImageInfo(DbContext db, List<string> files, Guid subjectUid)
        {
            try
            {
                foreach (var f in files)
                {
                    var image = new ImageInfo
                    {
                        ImageName = Path.GetFileName(f),
                        SubjectKey = subjectUid,
                        ImagePath = Function.PathToRelativeUrl(f),
                    };

                    db.Set<ImageInfo>().Add(image);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        public dynamic GetFundOverview(string queryJson)
        {
            try
            {
                string cacheKey = "GetFundProjects" + queryJson;
                try
                {
                    var res = CacheHelper.Get<List<FundDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {

                    var expression = LinqExtensions.True<Fund>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    //if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                    //{
                    //    int keyword = queryParam["MedicineCategory"].ToInt();

                    //    expression = expression.And(t => t.MedicineCategoryID == keyword);
                    //}

                    var fundlist = db.Set<Fund>().Where(expression).OrderBy(t => t.Order);

                    List<FundDto> list = new List<FundDto>();

                    var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    foreach (var f in fundlist)
                    {
                        var fdto = new FundDto { Name = f.Name, FundUid = f.FundUid };
                        var plst = db.Set<FundProject>().Where(t => t.FundID == f.Id).OrderBy(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t => new FundProjectDto
                        {
                            Name = t.Name,
                            CreateTime = t.CreateTime,
                            FundProjectUid = t.FundProjectUid,
                            FrontPic = string.IsNullOrEmpty(t.FrontPic) ? null : StaticPicUrlHost + t.FrontPic
                        }).ToList();
                        fdto.FundProjects = plst;
                        list.Add(fdto);
                    }

                    //var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    //var query = db.Set<MedicalRecord>().Where(expression).OrderByDescending(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                    //{
                    //    Author = t.Doctor.Name,
                    //    Title = t.Title,
                    //    ClicksCount = t.ClicksCount,
                    //    CommentsCount = t.CommentsCount,
                    //    FrontPic = string.IsNullOrEmpty(t.FrontPic) ? null : StaticPicUrlHost + t.FrontPic,
                    //    Score = t.Score,
                    //    Uid = t.MedicalRecordUid,
                    //    Type = "MedicalRecord",
                    //    CreateTime = t.CreateTime
                    //});


                    //var list = Function.GetPageData(query, queryParam);

                    //var res = list.ToList();

                    var res = list;

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public dynamic GetFundProject(Guid uid)
        {
            try
            {
                string cacheKey = "GetFundProject" + uid.ToString();
                try
                {
                    var res = CacheHelper.Get<FundProjectDto>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var expression = LinqExtensions.True<FundProject>();
                    expression = expression.And(t => t.FundProjectUid==uid);
                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    var dbitem = db.Set<FundProject>().FirstOrDefault(expression);

                    var res = dbitem.MapTo<FundProjectDto>();

                    return res;

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public dynamic GetConferences(string queryJson)
        {
            try
            {
                string cacheKey = "GetConferences" + queryJson;
                try
                {
                    var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {

                    var expression = LinqExtensions.True<Conference>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    //if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                    //{
                    //    int keyword = queryParam["MedicineCategory"].ToInt();

                    //    expression = expression.And(t => t.MedicineCategoryID == keyword);
                    //}

                    var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    var query = db.Set<Conference>().Where(expression).OrderByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                    {
                        //Author = t.Author,
                        Title = t.Title,
                        ClicksCount = t.ClicksCount,
                        CommentsCount = t.CommentsCount,
                        FrontPic = string.IsNullOrEmpty(t.FrontPic) ? null : StaticPicUrlHost + t.FrontPic,
                        Score = t.Score,
                        Uid = t.ConferenceUid,
                        Type = "Conference",
                        CreateTime = t.CreateTime,
                        Location = t.City,
                        BeginDate = t.BeginDate,
                        EndDate = t.EndDate
                    });


                    var list = Function.GetPageData(query, queryParam);

                    var res = list.ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public dynamic GetConference(Guid uid)
        {
            string cacheKey = "GetConference" + uid.ToString();
            try
            {

                try
                {
                    var res = CacheHelper.Get<ConferenceDto>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var data = db.Set<Conference>().Single(t => t.ConferenceUid == uid);

                    var res = data.MapTo<ConferenceDto>();

                    //var host = Function.GetHostAndApp();

                    //res.Images = db.Set<ImageInfo>().Where(t => t.SubjectKey == uid).OrderBy(t => t.ImageName).Select(t => host + "/" + t.ImagePath).ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string PostAttentionConference(Guid authid, Guid conferenceUid)
        {
            string msg = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var user = db.Set<User>().FirstOrDefault(u => u.AuthID == authid && u.IsValid == true);
                    if (user != null)
                    {
                        if (!db.Set<MyConference>().Any(t => t.Conference.ConferenceUid == conferenceUid && t.UserID == user.Id))
                        {
                            var c = db.Set<Conference>().Single(t => t.ConferenceUid == conferenceUid);
                            db.Set<MyConference>().Add(new MyConference { Conference = c, User = user });
                            db.SaveChanges();

                            CacheHelper.Clear("GetMyConferences" + authid.ToString());
                        }
                        else
                        {
                            msg = "已关注";
                        }

                    }
                    else
                    {
                        msg = "请登陆";
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return msg;
        }

        public dynamic GetMyConferences(Guid authid, string queryJson)
        {
            try
            {
                string cacheKey = "GetMyConferences" + authid.ToString() + queryJson;
                try
                {
                    var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {

                    var expression = LinqExtensions.True<MyConference>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true&&t.User.AuthID== authid);

                    //if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                    //{
                    //    int keyword = queryParam["MedicineCategory"].ToInt();

                    //    expression = expression.And(t => t.MedicineCategoryID == keyword);
                    //}

                    var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    var query = db.Set<MyConference>().Where(expression).OrderByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                    {
                        //Author = t.Author,
                        Title = t.Conference.Title,
                        FrontPic = string.IsNullOrEmpty(t.Conference.FrontPic) ? null : StaticPicUrlHost + t.Conference.FrontPic,
                        Uid = t.Conference.ConferenceUid,
                        Type = "Conference",
                        CreateTime = t.CreateTime,
                        Location = t.Conference.City,
                        BeginDate = t.Conference.BeginDate,
                        EndDate = t.Conference.EndDate
                    });


                    var list = Function.GetPageData(query, queryParam);

                    var res = list.ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string PostAttention(AttentionDto attentionDto, Guid authid)
        {
            string msg = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var user = db.Set<User>().FirstOrDefault(u => u.AuthID == authid && u.IsValid == true);
                    if (user != null)
                    {
                        if (!db.Set<Attention>().Any(t => t.Uid == attentionDto.Uid && t.UserID == user.Id))
                        {
                            db.Set<Attention>().Add(new Attention { Title = attentionDto.Title, Type = attentionDto.Type, Uid = attentionDto.Uid, PicUrl = attentionDto.PicUrl, User = user });
                            db.SaveChanges();
                        }
                        else
                        {
                            msg = "已关注";
                        }
                        
                    }
                    else
                    {
                        msg = "请登陆";
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return msg;
        }

        public dynamic GetMyAttentions(string type, Guid authid, string queryJson)
        {
            try
            {
                using (var db = base.NewDB())
                {

                    var expression = LinqExtensions.True<Attention>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    //if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                    //{
                    //    int keyword = queryParam["MedicineCategory"].ToInt();

                    //    expression = expression.And(t => t.MedicineCategoryID == keyword);
                    //}

                    var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    var query = db.Set<Attention>().Where(expression).OrderByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                    {
                        //Author = t.Author,
                        Title = t.Title,
                        FrontPic = string.IsNullOrEmpty(t.PicUrl) ? null : StaticPicUrlHost + t.PicUrl,//t.PicUrl,
                        Uid = t.Uid,
                        ArticleUid=t.ArticleUid,
                        Type = t.Type,
                        CreateTime = t.CreateTime
                    });


                    var list = Function.GetPageData(query, queryParam);

                    var res = list.ToList();

                    //CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public dynamic GetVideo(Guid uid)
        {
            string cacheKey = "GetVideo" + uid.ToString();
            try
            {

                try
                {
                    var res = CacheHelper.Get<VideoInfoDto>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var data = db.Set<VideoInfo>().Single(t => t.VideoUID == uid);

                    var res = data.MapTo<VideoInfoDto>();

                    //var host = Function.GetHostAndApp();

                    //res.Images = db.Set<ImageInfo>().Where(t => t.SubjectKey == uid).OrderBy(t => t.ImageName).Select(t => host + "/" + t.ImagePath).ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public dynamic GetVideos(string queryJson)
        {
            try
            {
                string cacheKey = "GetVideos" + queryJson;
                try
                {
                    var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {

                    var expression = LinqExtensions.True<VideoInfo>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    if (!queryParam["SeriesTitle"].IsEmpty())
                    {
                        string keyword = queryParam["SeriesTitle"].ToString();

                        expression = expression.And(t => t.VideoSeries.Title == keyword);
                    }

                    var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    var query = db.Set<VideoInfo>().Where(expression).OrderByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                    {
                        Author = t.Presenter,
                        Title = t.Title,
                        FrontPic = !string.IsNullOrEmpty(t.Snapshot)&& !t.Snapshot.StartsWith("http") ? StaticPicUrlHost + t.Snapshot : t.Snapshot,
                        Uid = t.VideoUID,
                        Type = "VideoInfo",
                        CreateTime = t.CreateTime,
                    });


                    var list = Function.GetPageData(query, queryParam);

                    var res = list.ToList();

                    CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetSurveys(Guid uid,string queryJson)
        {
            string cacheKey = "GetSurveys" + uid.ToString()+ queryJson;
            try
            {
                
                try
                {
                    var res = CacheHelper.Get<List<SurveyDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var expression = LinqExtensions.True<Survey>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    //var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                    var query = db.Set<Survey>().Where(expression).OrderBy(t => t.Order).ThenByDescending(t => t.CreateTime);
                    
                    var list = Function.GetPageData(query, queryParam);

                    var res = list.MapToList<SurveyDto>();

                    //CacheHelper.Set(cacheKey, res, new TimeSpan(0, _cacheslidingminutes, 0));

                    return res;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public dynamic GetSurveyQuestions(Guid uid,Guid SurveyUid,string queryJson)
        {
            string cacheKey = "GetSurveyQuestions" + SurveyUid.ToString();
            try
            {
                try
                {
                    var res = CacheHelper.Get<List<SurveyQuestionDto>>(cacheKey);

                    if (res != null)
                    {
                        return res;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                using (var db = base.NewDB())
                {
                    var expression = LinqExtensions.True<SurveyQuestion>();
                    var queryParam = queryJson.ToJObject();

                    expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                    expression = expression.And(t => t.Survey.SurveyUid == SurveyUid);

                    var query = db.Set<SurveyQuestion>().Where(expression).OrderBy(t => t.Number);

                    var list = Function.GetPageData(query, queryParam);

                    var res = list.MapToList<SurveyQuestionDto>();

                    CacheHelper.Set(cacheKey, res, new TimeSpan(0, _cacheslidingminutes, 0));

                    return res;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public string PostSurveyAnswer(Guid authid, Guid surveyUid, List<SurveyQuestionDto> answer)
        {
            string msg = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var user = db.Set<User>().FirstOrDefault(u => u.AuthID == authid && u.IsValid == true);

                    var options = db.Set<SurveyQuestionOption>().Where(t => t.SurveyQuestion.Survey.SurveyUid == surveyUid).AsEnumerable();

                    foreach (var q in answer)
                    {
                        var sr = new SurveyAnswer() { User = user, SurveyQuestionID = q.id };
                        if (q.type == 1)
                        {
                            sr.Answer = q.chooseitem;
                            options.Single(t => t.Value == q.chooseitem && t.SurveyQuestionID == q.id).SelectCount++;
                        }
                        else
                        {
                            var arr = q.items.Where(t => t.chosen == true).Select(t => t.value).ToArray();
                            sr.Answer = string.Join(",", arr);

                            foreach (var op in options.Where(t => t.SurveyQuestionID == q.id&&arr.Contains(t.Value)))
                            {
                                op.SelectCount++;
                            }
                        }

                        db.Set<SurveyAnswer>().Add(sr);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return msg;
        }



    }
}
