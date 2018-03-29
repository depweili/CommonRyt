using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Web
{
    public class AuthFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
                //var authorization = actionContext.Request.Headers.Authorization;
                if (actionContext.ActionDescriptor.GetCustomAttributes<AuthFilterAttribute>().Any())
                {
                    if (actionContext.Request.Headers.Authorization != null && !actionContext.Request.Headers.Authorization.Parameter.IsEmpty())
                    {
                        var token = actionContext.Request.Headers.Authorization.Parameter;

                        string uid = string.Empty;

                        if (!ValidateToken(token, out uid))
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        }
                        else
                        {
                            var genericIdentity = new GenericIdentity(uid);
                            var principal = new GenericPrincipal(genericIdentity, null);

                            Thread.CurrentPrincipal = principal;

                            base.OnAuthorization(actionContext);
                        }
                    }
                    else
                    {
                        StringBuilder reqinfo = new StringBuilder(actionContext.Request.RequestUri.ToString());
                        reqinfo.Append(actionContext.Request.Headers);
                        LogHelper.Monitor("认证失败:"+ reqinfo.ToString());
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                
                //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
                //else
                //{
                    

                //    var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                //    bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //    if (isAnonymous)
                //    {
                //        base.OnAuthorization(actionContext);
                //    }
                //    else
                //    {
                //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                //    }
                //}

                //base.OnAuthorization(actionContext);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OnAuthorization", ex);
                throw ex;
            }
            
        }

        //校验 
        private bool ValidateToken(string Token,out string uid)
        {
            bool flag = false;
            uid = string.Empty;
            try
            {
                //获取数据库Token  
                //Dec.Models.TicketAuth model = Dec.BLL.TicketAuth.GetTicketAuthByToken(encryptToken);
                //if (model.Token == encryptToken) //存在  
                //{
                //    //未超时  
                //    flag = (DateTime.Now <= model.ExpireDate) ? true : false;
                //}
                //var decode = Base64DEncrypt.Base64ForUrlDecode(Token);

                var decode = TripleDESDEncrypt.Decrypt(Token);

                var paras = decode.Split('#');

                var p1 = paras[0].ToGuid();

                var p2 = new DateTime(long.Parse(paras[1]));

                if (p1 != default(Guid) && p2 > DateTime.Now)
                {
                    flag = true;
                    uid = p1.ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            return flag;
        }
    }


    public class ApiMonitorAttribute : ActionFilterAttribute
    {

        private const string Key = "_WebApiMonitor_";
        private bool _IsDebugLog = true;
        //ConfigHelper.GetConfigBool("IsFilterLog");

        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    if (_IsDebugLog)
        //    {
        //        Stopwatch stopWatch = new Stopwatch();

        //        actionContext.Request.Properties[Key] = stopWatch;

        //        string actionName = actionContext.ActionDescriptor.ActionName;

        //        //Debug.Print(Newtonsoft.Json.JsonConvert.SerializeObject(actionContext.ActionArguments));

        //        LogHelper.Monitor(JsonConvert.SerializeObject(actionContext, new JsonSerializerSettings()
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        }));
        //        LogHelper.Monitor(actionContext.Request.RequestUri.ToString());

        //        LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.ActionArguments));

        //        stopWatch.Start();
        //    }
        //    base.OnActionExecuting(actionContext);

        //}

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (_IsDebugLog)
                {
                    Stopwatch stopWatch = new Stopwatch();

                    actionContext.Request.Properties[Key] = stopWatch;

                    string actionName = actionContext.ActionDescriptor.ActionName;

                    //Debug.Print(Newtonsoft.Json.JsonConvert.SerializeObject(actionContext.ActionArguments));

                    //LogHelper.Monitor(JsonConvert.SerializeObject(actionContext, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //}));
                    LogHelper.Monitor(actionContext.Request.RequestUri.ToString());

                    LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.ActionArguments));

                    //LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.Request, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //}));

                    LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.Request.Content.ReadAsStringAsync().Result));
                    //LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.Request.Headers));
                    //LogHelper.Monitor(JsonConvert.SerializeObject(actionContext.Request.Properties));

                    stopWatch.Start();
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("OnActionExecutingAsync", ex);
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }


        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {

            try
            {
                if (_IsDebugLog)
                {
                    Stopwatch stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;

                    if (stopWatch != null)
                    {

                        stopWatch.Stop();

                        string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

                        string controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                        LogHelper.Monitor(actionExecutedContext.Response.Content.ReadAsStringAsync().Result);

                        LogHelper.Monitor(string.Format(@"[{0}/{1} 用时 {2}ms]", controllerName, actionName, stopWatch.Elapsed.TotalMilliseconds));
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("OnActionExecutedAsync", ex);
            }

            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}