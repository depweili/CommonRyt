using Common.Domain;
using Common.Services.Dtos;
using Common.Util;
using Common.Util.Extesions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    string keyord = queryParam["area"].ToString();
                    expression = expression.And(t => t.Area.Name == keyord);
                }

                var query = db.Set<Hospital>().Where(expression).OrderBy(t => t.Name);

                dblist = query.Take(100);
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

        public dynamic GetDoctors(string queryJson)
        {
            using (var db = base.NewDB())
            {
                var data = db.Set<Doctor>().OrderBy(t => t.Name);

                var res = data.MapToList<DoctorDto>();

                return res;
            }
        }
        

        public dynamic GetPatientDoctors(Guid PatientUid)
        {
            using (var db = base.NewDB())
            {
                var patient = db.Set<Patient>().Single(t => t.Uid == PatientUid);

                var data = db.Set<PatientDoctor>().Where(t=>t.Patient== patient).OrderByDescending(t => t.CreateTime).Select(t=>t.Doctor);

                var res = data.MapToList<DoctorDto>();

                return res;
            }
        }
    }
}
