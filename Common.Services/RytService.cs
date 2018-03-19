using Common.Domain;
using Common.Services.Dtos;
using Common.Util;
using Common.Util.Extesions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Core.Objects;

namespace Common.Services
{
    public class RytService : ServiceBase
    {
        public string BindingDoctor(Guid PatientUid, Guid DoctorUid)
        {
            string res = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var dbitem = db.Set<PatientDoctor>().SingleOrDefault(t => t.Patient.Uid == PatientUid && t.Doctor.Uid == DoctorUid);

                    //if (!db.Set<PatientDoctor>().Any(t => t.Patient.Uid == PatientUid && t.Doctor.Uid == DoctorUid))
                    if (dbitem != null && dbitem.IsValid == true)
                    {
                        res = "您已经绑定此医生";

                    }
                    else
                    {
                        if (dbitem == null)
                        {
                            db.Set<PatientDoctor>().Add(new PatientDoctor
                            {
                                Patient = db.Set<Patient>().Single(t => t.Uid == PatientUid),
                                Doctor = db.Set<Doctor>().Single(t => t.Uid == DoctorUid),
                            });
                        }
                        else
                        {
                            dbitem.IsValid = true;
                        }

                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public string BindingDoctorCancel(Guid PatientUid, Guid DoctorUid)
        {
            string res = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    var dbitem = db.Set<PatientDoctor>().SingleOrDefault(t => t.Patient.Uid == PatientUid && t.Doctor.Uid == DoctorUid && t.IsValid == true);
                    if (dbitem!=null)
                    {
                        dbitem.IsValid = false;

                        db.SaveChanges();
                    }
                    else
                    {
                        res = "您未绑定此医生";
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public dynamic GetHospitals(string queryJson)
        {
            using (var db = base.NewDB())
            {
                db.Configuration.ProxyCreationEnabled = false;

                IEnumerable<Hospital> dblist = null;

                var expression = LinqExtensions.True<Hospital>();
                var queryParam = queryJson.ToJObject();

                if (!queryParam["area"].IsEmpty())
                {
                    string keyword = queryParam["area"].ToString();
                    expression = expression.And(t => t.Area.Name == keyword);
                }

                if (!queryParam["areaid"].IsEmpty()&& queryParam["areaid"].ToString() != "-1")
                {
                    string keyword = queryParam["areaid"].ToString();

                    var inlist = Function.GetColumnListByTree<int>(db, keyword, "Ryt_BaseArea");
                    expression = expression.And(t => inlist.Contains(t.AreaID??0));
                }

                var query = db.Set<Hospital>().Where(expression).OrderBy(t => t.Name);

                dblist = GetPageData(query, queryParam);
                //dblist = query.Take(100);
                //var data = db.Set<Hospital>().Take(100);

                var res = dblist.MapToList<HospitalDto>();

                return res;
            }
        }

        public dynamic GetMedicineCategorys()
        {
            using (var db = base.NewDB())
            {
                var data = db.Set<MedicineCategory>().OrderBy(t => t.Order);

                var res = data.MapToList<MedicineCategoryDto>();

                return res;
            }
        }

        public DoctorDto GetDoctor(Guid PatientUid,string queryJson)
        {
            using (var db = base.NewDB())
            {
                DoctorDto res = null;
                var expression = LinqExtensions.False<Doctor>();
                var queryParam = queryJson.ToJObject();

                if (!queryParam["qrcode"].IsEmpty())
                {
                    string keyword = queryParam["qrcode"].ToString();
                    expression = expression.Or(t => t.Code == keyword);
                }
                else
                {
                    if (!queryParam["uid"].IsEmpty())
                    {
                        Guid keyword = queryParam["uid"].ToString().ToGuid();
                        expression = expression.Or(t => t.Uid == keyword);
                    }
                }
                var query = db.Set<Doctor>().FirstOrDefault(expression);

                res = query.MapTo<DoctorDto>();

                if (PatientUid != default(Guid))
                {
                    res.IsConnect = db.Set<PatientDoctor>().Any(t => t.Patient.Uid == PatientUid && t.IsValid == true && t.DoctorID == res.Id);
                }

                return res;

            }
        }


        public dynamic GetDoctors(Guid PatientUid,string queryJson)
        {
            using (var db = base.NewDB())
            {
                IEnumerable<Doctor> dblist = null;

                var expression = LinqExtensions.True<Doctor>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted??false) == false && (t.IsValid??true) == true);

                if (!queryParam["areaid"].IsEmpty()&& queryParam["areaid"].ToString()!="-1")
                {
                    string keyword = queryParam["areaid"].ToString();

                    var inlist = Function.GetColumnListByTree<int>(db, keyword, "Ryt_BaseArea");
                    expression = expression.And(t => inlist.Contains(t.MedicineDepartment.Hospital.AreaID ?? 0));
                }

                if (!queryParam["hospitalid"].IsEmpty() && queryParam["hospitalid"].ToString()!="-1")
                {
                    int keyword = queryParam["hospitalid"].ToInt();
                    expression = expression.And(t => t.MedicineDepartment.HospitalID== keyword);
                }

                if (!queryParam["medicinecategoryid"].IsEmpty() && queryParam["medicinecategoryid"].ToString() != "-1")
                {
                    int keyword = queryParam["medicinecategoryid"].ToInt();
                    expression = expression.And(t => t.MedicineDepartment.MedicineCategoryID == keyword);
                }

                //if (!queryParam["doctorcode"].IsEmpty() && queryParam["doctorcode"].ToString() != "-1")
                //{
                //    string keyword = queryParam["doctorcode"].ToString();
                //    expression = expression.And(t => t.Code == keyword);
                //}

                //if (!queryParam["uid"].IsEmpty() && queryParam["uid"].ToString() != "-1")
                //{
                //    Guid keyword = queryParam["uid"].ToString().ToGuid();
                //    expression = expression.And(t => t.Uid == keyword);
                //}


                if (PatientUid == default(Guid))
                {
                    //var query = db.Set<Doctor>().Where(expression).OrderBy(t => t.Name);

                    //dblist = GetPageData(query, queryParam);

                    //var res = dblist.Select(t => new
                    //{
                    //    Name = t.Name,
                    //    Uid = t.Uid,
                    //    Avatar = t.Avatar,
                    //    DepartmentAlias = t.DepartmentAlias,
                    //    EduTitle = t.EduTitle,
                    //    Expert = t.Expert,
                    //    IsValid = t.IsValid,
                    //    Title = t.Title,
                    //    HospitalName = t.MedicineDepartment.Hospital.Name,
                    //    MedicineCategoryName = t.MedicineDepartment.MedicineCategory.Name,
                    //    IsVerified = t.IsVerified
                    //});

                    var query = db.Set<Doctor>().Where(expression).Select(t => new
                    {
                        Name = t.Name,
                        Uid = t.Uid,
                        Avatar = t.Avatar,
                        DepartmentAlias = t.DepartmentAlias,
                        EduTitle = t.EduTitle,
                        Expert = t.Expert,
                        IsValid = t.IsValid,
                        Title = t.Title,
                        HospitalName = t.MedicineDepartment.Hospital.Name,
                        MedicineCategoryName = t.MedicineDepartment.MedicineCategory.Name,
                        IsVerified = t.IsVerified
                    }).OrderBy(t => t.Name);

                    var list = GetPageData(query, queryParam);

                    var res = list.ToList();

                    return res;
                }

                else
                {
                    var query = from t in db.Set<Doctor>()
                                join b in db.Set<PatientDoctor>().Where(t => t.Patient.Uid == PatientUid&&t.IsValid==true) on t.Id equals b.DoctorID into temp
                                from c in temp.DefaultIfEmpty()
                                select new
                                {
                                    Name = t.Name,
                                    Uid = t.Uid,
                                    Avatar = t.Avatar,
                                    DepartmentAlias = t.DepartmentAlias,
                                    EduTitle = t.EduTitle,
                                    Expert = t.Expert,
                                    IsValid = t.IsValid,
                                    Title = t.Title,
                                    HospitalName = t.MedicineDepartment.Hospital.Name,
                                    MedicineCategoryName = t.MedicineDepartment.MedicineCategory.Name,
                                    IsVerified = t.IsVerified,
                                    IsConnect = c.PatientID.HasValue
                                };


                    var list = GetPageData(query.OrderBy(t => t.Name), queryParam);

                    var res = list.ToList();

                    return res;
                }
                


                /////////////////////////////////////////////

                //var query = db.Set<Doctor>().Where(expression).Select(t => new
                //{
                //    Name = t.Name,
                //    Uid = t.Uid,
                //    Avatar = t.Avatar,
                //    DepartmentAlias = t.DepartmentAlias,
                //    EduTitle = t.EduTitle,
                //    Expert = t.Expert,
                //    IsValid = t.IsValid,
                //    Title = t.Title,
                //    HospitalName = t.MedicineDepartment.Hospital.Name,
                //    MedicineCategoryName = t.MedicineDepartment.MedicineCategory.Name,
                //    IsVerified = t.IsVerified
                //}).OrderBy(t => t.Name);


                //var query = db.Set<Doctor>().Where(expression).Join(db.Set<PatientDoctor>().Where(t=>t.Patient.Uid == PatientUid), a => a.Id, b => b.DoctorID, (a, b) => new { a, b }).DefaultIfEmpty().Select(t => new
                //{
                //    Name = t.a.Name,
                //    Uid = t.a.Uid,
                //    Avatar = t.a.Avatar,
                //    DepartmentAlias = t.a.DepartmentAlias,
                //    EduTitle = t.a.EduTitle,
                //    Expert = t.a.Expert,
                //    IsValid = t.a.IsValid,
                //    Title = t.a.Title,
                //    HospitalName = t.a.MedicineDepartment.Hospital.Name,
                //    MedicineCategoryName = t.a.MedicineDepartment.MedicineCategory.Name,
                //    IsVerified = t.a.IsVerified,
                //    IsConnect = t.b.PatientID.HasValue
                //}).OrderBy(t => t.Name);

                //var query = db.Set<Doctor>().Join(db.Set<PatientDoctor>(), a => a.Id, b => b.DoctorID, (a, b) => new { a, b }).Where(t=>t.b.Patient.Uid == PatientUid).DefaultIfEmpty().Select(t => new
                //{
                //    Name = t.a.Name,
                //    Uid = t.a.Uid,
                //    Avatar = t.a.Avatar,
                //    DepartmentAlias = t.a.DepartmentAlias,
                //    EduTitle = t.a.EduTitle,
                //    Expert = t.a.Expert,
                //    IsValid = t.a.IsValid,
                //    Title = t.a.Title,
                //    HospitalName = t.a.MedicineDepartment.Hospital.Name,
                //    MedicineCategoryName = t.a.MedicineDepartment.MedicineCategory.Name,
                //    IsVerified = t.a.IsVerified,
                //    IsConnect = t.b.PatientID.HasValue
                //}).OrderBy(t => t.Name);

                
                ////////////////////////////////////////////

                //var query2=query.Select(t => new 
                //{
                //    Uid = t.Uid,
                //    Avatar = t.Avatar,
                //    DepartmentAlias = t.DepartmentAlias,
                //    EduTitle = t.EduTitle,
                //    Expert = t.Expert,
                //    IsValid = t.IsValid,
                //    Title = t.Title,
                //    MedicineDepartment = t.MedicineDepartment,
                //    IsVerified = t.IsVerified
                //});

                //string sql2 = query2.ToString();

                //ObjectQuery<Doctor> oq = query2 as ObjectQuery<Doctor>;
                //string sql = oq.ToTraceString();
                //db.Database.Log= Console.WriteLine;

                //var data = db.Set<Doctor>().OrderBy(t => t.Name);
                //dblist = query.Take(100);

                //var res = dblist.MapToList<DoctorDto>();


            }
        }

        

        public dynamic SavePatient(Guid puid, PatientDto patient)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<T> GetPageData<T>(IQueryable<T> query, JObject queryParam)
        {
            int pageNum = queryParam["pageNum"].IsEmpty() ? 1 : queryParam["pageNum"].ToString().ToInt();
            int pageSize = queryParam["pageSize"].IsEmpty() ? 20 : queryParam["pageSize"].ToString().ToInt();

            return query.Skip(pageSize * (pageNum - 1)).Take(pageSize);
        }

        public dynamic GetPatientDoctors(Guid PatientUid, string queryJson)
        {
            using (var db = base.NewDB())
            {
                IEnumerable<Doctor> dblist = null;

                //var patient = db.Set<Patient>().Single(t => t.Uid == PatientUid);

                //var query = db.Set<PatientDoctor>().Where(t=>t.Patient.Uid== PatientUid).OrderByDescending(t => t.CreateTime).Select(t=>t.Doctor);


                var query = db.Set<PatientDoctor>().Where(t => t.Patient.Uid == PatientUid&&t.IsValid==true).Select(t => new
                {
                    Id=t.Doctor.Id,
                    Name = t.Doctor.Name,
                    Uid = t.Doctor.Uid,
                    Avatar = t.Doctor.Avatar,
                    DepartmentAlias = t.Doctor.DepartmentAlias,
                    EduTitle = t.Doctor.EduTitle,
                    Expert = t.Doctor.Expert,
                    IsValid = t.Doctor.IsValid,
                    Title = t.Doctor.Title,
                    HospitalName = t.Doctor.MedicineDepartment.Hospital.Name,
                    MedicineCategoryName = t.Doctor.MedicineDepartment.MedicineCategory.Name,
                    IsVerified = t.Doctor.IsVerified,
                    t.CreateTime

                }).OrderByDescending(t => t.CreateTime);

                var queryParam = queryJson.ToJObject();

                var res = GetPageData(query, queryParam);

                //var res = dblist.MapToList<DoctorDto>();

                return res.ToList();
            }
        }

        public string SaveCharityDrugApplication(CharityDrugApplicationDto charitydrugdto)
        {
            string res = string.Empty;
            try
            {
                CharityDrugApplication charityDrug = null;
                using (var db = base.NewDB())
                {
                    if (charitydrugdto.StrCharityDrugUid.IsEmpty())
                    {
                        charityDrug = new CharityDrugApplication();

                        charityDrug = charitydrugdto.MapTo(charityDrug);

                        charityDrug.Patient = db.Set<Patient>().Single(t => t.Uid == charitydrugdto.PatientUid);

                        if (charityDrug.Patient != null)
                        {
                            db.Set<CharityDrugApplication>().Add(charityDrug);
                        }
                        else
                        {
                            res = "非法用户";
                        }
                        
                    }
                    else
                    {
                        var uid = charitydrugdto.StrCharityDrugUid.ToGuid();

                        charityDrug = db.Set<CharityDrugApplication>().Single(t => t.CharityDrugUid == uid);

                        charityDrug = charitydrugdto.MapTo(charityDrug);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public dynamic GetCharityDrugApplication(Guid uid)
        {
            using (var db = base.NewDB())
            {
                CharityDrugApplicationDto res = null;
                var dbitem = db.Set<CharityDrugApplication>().FirstOrDefault(t => t.CharityDrugUid == uid);

                res = dbitem.MapTo<CharityDrugApplicationDto>();
                return res;
            }
        }

        public string DeleteCharityDrugApplication(Guid uid)
        {
            string res = string.Empty;
            try
            {
                using (var db = base.NewDB())
                {
                    CharityDrugApplication charityDrug = db.Set<CharityDrugApplication>().Single(t=>t.CharityDrugUid== uid);

                    if (charityDrug.State <= 1)
                    {
                        charityDrug.IsDeleted = true;
                        //db.Set<CharityDrugApplication>().Remove(charityDrug);
                    }
                    else
                    {
                        res = "审核通过无法删除";
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public dynamic GetCharityDrugApplications(string queryJson = "",Guid patientuid=default(Guid))
        {
            using (var db = base.NewDB())
            {
                IEnumerable<CharityDrugApplication> dblist = null;

                var expression = LinqExtensions.True<CharityDrugApplication>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted??false) == false);

                if (patientuid!= default(Guid))
                {
                    expression = expression.And(t => t.Patient.Uid == patientuid);
                }

                //if (!queryParam["areaid"].IsEmpty() && queryParam["areaid"].ToString() != "-1")
                //{
                //    string keyword = queryParam["areaid"].ToString();

                //    var inlist = Function.GetColumnListByTree<int>(db, keyword, "Ryt_BaseArea");
                //    expression = expression.And(t => inlist.Contains(t.MedicineDepartment.Hospital.AreaID ?? 0));
                //}

                //if (!queryParam["hospitalid"].IsEmpty() && queryParam["hospitalid"].ToString() != "-1")
                //{
                //    int keyword = queryParam["hospitalid"].ToInt();
                //    expression = expression.And(t => t.MedicineDepartment.HospitalID == keyword);
                //}

                //if (!queryParam["medicinecategoryid"].IsEmpty() && queryParam["medicinecategoryid"].ToString() != "-1")
                //{
                //    int keyword = queryParam["medicinecategoryid"].ToInt();
                //    expression = expression.And(t => t.MedicineDepartment.MedicineCategoryID == keyword);
                //}

                var query = db.Set<CharityDrugApplication>().Where(expression).OrderByDescending(t => t.CreateTime);
                
                var list = GetPageData(query, queryParam);

                var res = list.MapToList<CharityDrugApplicationDto>();

                return res;
            }
        }

        public dynamic GetPatientMedicalRecord(Guid patientuid)
        {
            using (var db = base.NewDB())
            {
                PatientMedicalRecordDto res = null;
                var dbitem = db.Set<PatientMedicalRecord>().FirstOrDefault(t => t.Patient.Uid == patientuid);

                res = dbitem.MapTo<PatientMedicalRecordDto>();
                return res;
            }
        }

        public dynamic SavePatientMedicalRecord(PatientMedicalRecordDto dto)
        {
            string res = string.Empty;
            try
            {
                PatientMedicalRecord dbitem = null;
                using (var db = base.NewDB())
                {
                    if (!dto.PatientUid.IsEmpty())
                    {
                        dbitem = db.Set<PatientMedicalRecord>().FirstOrDefault(t => t.Patient.Uid == dto.PatientUid);
                        if (dbitem!=null)
                        {
                            dbitem.Content = dto.Content;
                        }
                        else
                        {
                            dbitem = new PatientMedicalRecord();

                            dbitem = dto.MapTo(dbitem);

                            dbitem.Patient = db.Set<Patient>().FirstOrDefault(t => t.Uid == dto.PatientUid);

                            if (dbitem.Patient != null)
                            {
                                db.Set<PatientMedicalRecord>().Add(dbitem);
                            }
                            else
                            {
                                res = "非法用户";
                            }
                        }

                    }
                    else
                    {
                        res = "未知用户";
                    }
                    //else
                    //{
                    //    var uid = dto.PatientUid;//dto.PatientUid.ToGuid();

                    //    dbitem = db.Set<PatientMedicalRecord>().Single(t => t.Patient.Uid == uid);

                    //    dbitem = dto.MapTo(dbitem);
                    //}

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

    }
}
