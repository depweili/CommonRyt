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
                    if (!db.Set<PatientDoctor>().Any(t => t.Patient.Uid == PatientUid && t.Doctor.Uid == DoctorUid))
                    {
                        db.Set<PatientDoctor>().Add(new PatientDoctor
                        {
                            Patient = db.Set<Patient>().Single(t => t.Uid == PatientUid),
                            Doctor = db.Set<Doctor>().Single(t => t.Uid == DoctorUid),
                        });

                        db.SaveChanges();
                    }
                    else
                    {
                        res = "您已经绑定此医生";
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

        public DoctorDto GetDoctor(string queryJson)
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

                //var query = db.Set<Doctor>().Where(expression).OrderBy(t => t.Name);

                //dblist = GetPageData(query, queryParam);

                //var res = dblist.Select(t => new 
                //{
                //    Uid=t.Uid,
                //    Avatar=t.Avatar,
                //    DepartmentAlias=t.DepartmentAlias,
                //    EduTitle=t.EduTitle,
                //    Expert=t.Expert,
                //    IsValid=t.IsValid,
                //    Title=t.Title,
                //    HospitalName = t.MedicineDepartment.Hospital.Name,
                //    MedicineCategoryName= t.MedicineDepartment.MedicineCategory.Name,
                //    IsVerified =t.IsVerified
                //});


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

                var query = from t in db.Set<Doctor>()
                            join b in db.Set<PatientDoctor>().Where(t => t.Patient.Uid == PatientUid) on t.Id equals b.DoctorID into temp
                            from c in temp.DefaultIfEmpty()
                            select new {
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


                var list = GetPageData(query.OrderBy(t=>t.Name), queryParam);

                var res = list.ToList();
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

                return res;
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


                var query = db.Set<PatientDoctor>().Where(t => t.Patient.Uid == PatientUid).Select(t => new
                {
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
    }
}
