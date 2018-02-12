using Common.Services;
using Common.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
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
        public IHttpActionResult GetDoctors(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetDoctors(queryJson);

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
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/Doctor")]
        [HttpGet]
        public IHttpActionResult GetDoctor(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new RytService();
                var data = service.GetDoctor(queryJson);

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

        public IHttpActionResult GetPatientInfo(Guid patientuid, string queryJson = "")
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
    }
}
