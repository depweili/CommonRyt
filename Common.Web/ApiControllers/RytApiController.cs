﻿using Common.Services;
using Common.Services.Dtos;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    /// <summary>
    /// ryt
    /// </summary>
    public class RytApiController : ApiControllerBase
    {
        /// <summary>
        /// 医院查询
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/Hospitals")]
        [HttpGet]
        public IHttpActionResult GetHospitals(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetHospitals(queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;

            }
            return Ok(res);
        }

        /// <summary>
        /// 科室
        /// </summary>
        /// <returns></returns>
        [Route("api/MedicineCategorys")]
        [HttpGet]
        public IHttpActionResult GetMedicineCategorys()
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetMedicineCategorys();

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;

            }
            return Ok(res);
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <returns></returns>
        [Route("api/Doctors")]
        [HttpGet]
        public IHttpActionResult GetDoctors(string patientuid="",string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                Guid patuid = patientuid.ToGuid();
                var data = service.GetDoctors(patuid, queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;

            }
            return Ok(res);
        }

        /// <summary>
        /// 医生详情
        /// </summary>
        /// <param name="patientuid"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/Doctor")]
        [HttpGet]
        public IHttpActionResult GetDoctor(string patientuid = "", string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                Guid patuid = patientuid.ToGuid();
                var data = service.GetDoctor(patuid,queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;

            }
            return Ok(res);
        }

        /// <summary>
        /// 绑定医生
        /// </summary>
        /// <param name="patientuid"></param>
        /// <param name="doctoruid"></param>
        /// <returns></returns>
        [Route("api/BindingDoctor")]
        [HttpPost]
        public IHttpActionResult BindingDoctor(Guid patientuid, Guid doctoruid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.BindingDoctor(patientuid, doctoruid);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }

                res.resData = null;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 取消绑定
        /// </summary>
        /// <param name="patientuid"></param>
        /// <param name="doctoruid"></param>
        /// <returns></returns>
        [Route("api/BindingDoctor/Cancel")]
        [HttpPost]
        public IHttpActionResult BindingDoctorCancel(Guid patientuid, Guid doctoruid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.BindingDoctorCancel(patientuid, doctoruid);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }

                res.resData = null;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }


        /// <summary>
        /// 患者的医生
        /// </summary>
        /// <param name="patientuid"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/MyDoctors")]
        [HttpGet]
        public IHttpActionResult GetPatientDoctors(Guid patientuid, string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetPatientDoctors(patientuid, queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }


        /// <summary>
        /// 保存慈善赠药
        /// </summary>
        /// <param name="charitydrugdto"></param>
        /// <returns></returns>
        [Route("api/CharityDrug/Save")]
        [HttpPost]
        public IHttpActionResult SaveCharityDrugApplication(CharityDrugApplicationDto charitydrugdto)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.SaveCharityDrugApplication(charitydrugdto);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }

                res.resData = null;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 删除慈善赠药申请
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("api/CharityDrug/Del/{uid}")]
        [HttpPost]
        public IHttpActionResult DeleteCharityDrugApplication(Guid uid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.DeleteCharityDrugApplication(uid);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 获取慈善赠药明细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("api/CharityDrug/{uid}")]
        [HttpGet]
        public IHttpActionResult GetCharityDrugApplication(Guid uid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetCharityDrugApplication(uid);
                if (data == null)
                {
                    throw new Exception("未找到申请信息");
                }

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 获取慈善赠药列表信息
        /// </summary>
        /// <param name="patientuid"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/CharityDrugApplications")]
        [HttpGet]
        public IHttpActionResult GetCharityDrugApplications(Guid patientuid, string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetCharityDrugApplications(queryJson, patientuid);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }


        /// <summary>
        /// 获取病历信息
        /// </summary>
        /// <param name="patientuid"></param>
        /// <returns></returns>
        [Route("api/PatientMedicalRecord/{patientuid}")]
        [HttpGet]
        public IHttpActionResult GetPatientMedicalRecord(Guid patientuid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetPatientMedicalRecord(patientuid);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        /// <summary>
        /// 保存病情
        /// </summary>
        /// <param name="medicalrecorddto"></param>
        /// <returns></returns>
        [Route("api/PatientMedicalRecord/Save")]
        [HttpPost]
        public IHttpActionResult SavePatientMedicalRecord(PatientMedicalRecordDto medicalrecorddto)
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.SavePatientMedicalRecord(medicalrecorddto);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }

                res.resData = null;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }
    }
}
