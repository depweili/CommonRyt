using Common.Services;
using Common.Services.Dtos;
using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Common.Web.ApiControllers
{
    public class AppApiController : ApiControllerBase
    {
        #region 用户信息
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [Route("api/App/SmsVerify")]
        [HttpGet]
        public IHttpActionResult GetSmsVerify(string mobile)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                string data = ws.SendSms(mobile);

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
        /// 重设密码
        /// </summary>
        /// <param name="reset"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/App/ResetPwd")]
        [HttpPost]
        public IHttpActionResult ResetPwd(RegisterDto reset)
        {
            var res = new ResponseBase();
            try
            {
                UtilityService ws = new UtilityService();

                string data = ws.ResetPwd(reset);

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
                res.resData = data ?? throw new Exception("未找到用户明细信息");
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
        /// 导航
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("api/App/Navigations")]
        [HttpGet]
        public IHttpActionResult GetNavigations(int type = 0)
        {
            var res = new ResponseBase();
            try
            {
                var service = new CommonService();
                var data = service.GetNavigations(type);

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
        /// 病历详情
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        [Route("api/App/MedicalRecord")]
        [HttpGet]
        public IHttpActionResult GetMedicalRecord(Guid Uid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetMedicalRecord(Uid);

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
        /// 我的病历集
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

        /// <summary>
        /// 上传图片（参数+Multipart）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subjectUid"></param>
        /// <returns></returns>
        //[AuthFilter]
        //[ApiMonitor]
        [Route("api/App/UploadImagesAsync")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadImages(string type, Guid subjectUid)
        {
            //LogHelper.Info("UploadImages"+ subjectUid.ToString());
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();

                var data =await service.UploadImages(uid,type, subjectUid, Request);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }
                res.resData = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("UploadImages", ex);
                res.code = "100";
                res.msg = ex.Message;
            }
            return Ok(res);
        }

        
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [Route("api/App/UploadImages")]
        [HttpPost]
        public IHttpActionResult UploadImages()
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();

                var httpRequest = HttpContext.Current.Request;

                var data = service.UploadImages(httpRequest);

                if (!string.IsNullOrEmpty(data))
                {
                    res.code = "100";
                    res.msg = data;
                }
                res.resData = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("UploadImages", ex);
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
        /// 会议详情
        /// </summary>
        /// <param name="ConferenceUid"></param>
        /// <returns></returns>
        [Route("api/App/Conference")]
        [HttpGet]
        public IHttpActionResult GetConference(Guid ConferenceUid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetConference(ConferenceUid);

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
        /// 关注会议（*）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/ConferenceAttention")]
        [HttpPost]
        public IHttpActionResult PostAttentionConference(dynamic obj)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var authid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                string strconferenceUid = Convert.ToString(obj.ConferenceUid);
                var conferenceUid = strconferenceUid.ToGuid();
                var data = service.PostAttentionConference(authid, conferenceUid);

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
        /// 我的会议（*）
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


        #region 视频
        /// <summary>
        /// 讲堂视频
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[Route("api/App/LectureOverView")]
        [Route("api/App/Videos")]
        [HttpGet]
        public IHttpActionResult GetVideos(string queryJson)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetVideos(queryJson);
                

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
        /// 视频详情
        /// </summary>
        /// <param name="videoUid"></param>
        /// <returns></returns>
        [Route("api/App/Video")]
        [HttpGet]
        public IHttpActionResult GetVideo(Guid videoUid)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var data = service.GetVideo(videoUid);

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

        #region 调研
        
        /// <summary>
        /// 调研列表(*)
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[AuthFilter]
        [Route("api/App/Surveys")]
        [HttpGet]
        public IHttpActionResult GetSurveys(string queryJson="")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetSurveys(uid, queryJson);

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
        /// 调研详情(*)
        /// </summary>
        /// <param name="SurveyUid"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[AuthFilter]
        [Route("api/App/SurveyDetail")]
        [HttpGet]
        public IHttpActionResult GetSurveyDetail(Guid SurveyUid,string queryJson="")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetSurveyQuestions(uid, SurveyUid, queryJson);

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
        /// 提交答案（*）
        /// {SurveyUid:xxxxx,Answer:data}
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AuthFilter]
        [Route("api/App/SurveyAnswer")]
        [HttpPost]
        public IHttpActionResult PostSurveyAnswer(dynamic obj)
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                string strSurveyUid = Convert.ToString(obj.SurveyUid);
                var SurveyUid = strSurveyUid.ToGuid();
                var Answer = JsonConvert.DeserializeObject<List<SurveyQuestionDto>>(Convert.ToString(obj.Answer));
                var data = service.PostSurveyAnswer(uid, SurveyUid, Answer);

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
        /// 调研结果
        /// </summary>
        /// <param name="SurveyUid"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[AuthFilter]
        [Route("api/App/SurveyStatistics")]
        [HttpGet]
        public IHttpActionResult GetSurveyStatistics(Guid SurveyUid,string queryJson = "")
        {
            var res = new ResponseBase();
            try
            {
                var service = new AppService();
                var uid = Thread.CurrentPrincipal.Identity.Name.ToGuid();
                var data = service.GetSurveyStatistics(SurveyUid, queryJson);

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
