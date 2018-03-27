using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
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

                throw;
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
                var decode = Base64DEncrypt.Base64ForUrlDecode(Token);

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
}