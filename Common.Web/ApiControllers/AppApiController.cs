using Common.Services;
using Common.Services.Dtos;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    public class AppApiController : ApiControllerBase
    {
        #region 用户信息

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="Register"></param>
        /// <returns></returns>
        [Route("api/App/Register")]
        [HttpPost]
        public IHttpActionResult RegisterUser(RegisterDto Register)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                dynamic data = ws.RegisterUser(Register);

                if (data == null)
                {
                    throw new Exception("注册失败");
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
        /// 登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/App/Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginDto login)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                dynamic data = ws.Login(login);

                if (data == null)
                {
                    throw new Exception("登陆失败");
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
        /// 用户信息（*）
        /// </summary>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/UserProfile")]
        [HttpGet]
        public IHttpActionResult GetUserProfile()
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetUserProfile(uid);
                if (data == null)
                {
                    throw new Exception("未找到用户明细信息");
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
        #endregion


        #region 文章
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/App/Articles")]
        [HttpGet]
        public IHttpActionResult GetArticles(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new CommonService();
                var data = service.GetArticles(queryJson);

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
        /// 文章详情
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("api/App/GetArticle")]
        [HttpGet]
        public IHttpActionResult GetArticle(Guid uid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new CommonService();
                var data = service.GetArticle(uid);

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
        /// 发表评论（*）
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/Comment")]
        [HttpPost]
        public IHttpActionResult PostComment(CommentDto comment)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.PostComment(uid, comment);

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
        /// 评论列表
        /// </summary>
        /// <param name="SubjectKey"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/App/Comments")]
        [HttpGet]
        public IHttpActionResult GetComments(Guid SubjectKey, string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetComments(SubjectKey, queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        #endregion

        #region 病历

        /// <summary>
        /// 科室字典
        /// </summary>
        /// <returns></returns>
        [Route("api/App/MedicineCategory")]
        [HttpGet]
        public IHttpActionResult GetMedicineCategory()
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetMedicineCategory();

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
        /// 病历信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/App/MedicalRecords")]
        [HttpGet]
        public IHttpActionResult GetMedicalRecords(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetMedicalRecords(queryJson);

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
        /// 
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/MyMedicalRecords")]
        [HttpGet]
        public IHttpActionResult GetMyMedicalRecords(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetMyMedicalRecords(uid,queryJson);

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
        /// 上传病历
        /// </summary>
        /// <param name="medicalRecordDto"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/PostMedicalRecord")]
        [HttpPost]
        public IHttpActionResult PostMedicalRecord(MedicalRecordDto medicalRecordDto)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();

                var data = service.PostMedicalRecord(uid, medicalRecordDto);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }


        [AuthFilter]
        [Route("api/App/UploadImages")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadImages(Guid subjectUid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data =await service.UploadImages(uid, subjectUid, Request);

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
        #endregion

        #region 基金会
        /// <summary>
        /// 基金项目总览
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/App/FundOverview")]
        [HttpGet]
        public IHttpActionResult GetFundOverview(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetFundOverview(queryJson);

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
        /// 项目详情
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("api/App/FundProject")]
        [HttpGet]
        public IHttpActionResult GetFundProject(Guid uid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetFundProject(uid);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }
        
        #endregion

        #region 会议
        /// <summary>
        /// 会议信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [Route("api/App/Conferences")]
        [HttpGet]
        public IHttpActionResult GetConferences(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetConferences(queryJson);

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
        /// 我的会议
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/MyConferences")]
        [HttpGet]
        public IHttpActionResult GetMyConferences(string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var authid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetMyConferences(authid, queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }
        #endregion

        #region 关注
        /// <summary>
        /// 关注(*)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/Attention")]
        [HttpPost]
        public IHttpActionResult PostAttention(AttentionDto dto)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var authid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.PostAttention(dto, authid);

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
        /// 用户关注信息(*)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/MyAttentions")]
        [HttpGet]
        public IHttpActionResult GetMyAttentions(string type="", string queryJson="")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var authid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetMyAttentions(type,authid, queryJson);

                res.resData = data;
            }
            catch (Exception ex)
            {
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }


        

        #endregion
    }
}
