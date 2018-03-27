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
                        area = userpf.Area
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

                if (!queryParam["MedicineCategory"].IsEmpty() && queryParam["MedicineCategory"].ToString() != "-1")
                {
                    int keyword = queryParam["MedicineCategory"].ToInt();

                    expression = expression.And(t => t.MedicineCategoryID == keyword);
                }

                if (!queryParam["FundProjectUid"].IsEmpty() && queryParam["FundProjectUid"].ToString() != "-1")
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


        public dynamic GetMyMedicalRecords(Guid uid, string queryJson)
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
                        }
                        else
                        {
                            throw new Exception("用户验证失败");
                        }
                    }
                    else
                    {
                        
                        var mruid = mr.StrMedicalRecordUid.ToGuid();
                        var dbitem = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid==mruid);

                        dbitem = mr.MapTo(dbitem);

                        res = mr.StrMedicalRecordUid;
                    }

                    db.SaveChanges();

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadImages(Guid uid, Guid subjectUid, HttpRequestMessage request)
        {
            string res = string.Empty;
            try
            {
                if (request.Content.IsMimeMultipartContent())
                {
                    using (var db = base.NewDB())
                    {
                        var mr = db.Set<MedicalRecord>().Single(t => t.MedicalRecordUid == subjectUid);

                        var filelist = await Function.SaveImagesAsync(request, "MedicalRecord", subjectUid.ToString(), mr.CreateTime);

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

                    foreach (var f in fundlist)
                    {
                        var fdto = new FundDto { Name = f.Name, FundUid = f.FundUid };
                        var plst = db.Set<FundProject>().Where(t => t.FundID == f.Id).OrderBy(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t=>new FundProjectDto{ Name=t.Name, CreateTime=t.CreateTime,FundProjectUid=t.FundProjectUid}).ToList();
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

        public dynamic GetMyConferences(Guid authid, string queryJson)
        {
            try
            {
                string cacheKey = "GetMyConferences" + queryJson;
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
                        FrontPic = t.PicUrl,
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

    }
}
