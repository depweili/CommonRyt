using Common.Services;
using Common.Util;
using Common.Util.Extesions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Common.Web
{
    public class CustomErrorDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;
                HttpError error = null;
                if (response.TryGetContentValue<HttpError>(out error))
                {
                    //添加自定义错误处理
                    //error.Message = "Your Customized Error Message";

                    
                }

                if (error != null)
                {
                    StringBuilder reqinfo = new StringBuilder(request.RequestUri.ToString());
                    reqinfo.AppendLine(error.MessageDetail);
                    LogHelper.Monitor("CustomError:" + reqinfo.ToString());

                    LogHelper.Monitor(JsonConvert.SerializeObject(request.Headers));
                    LogHelper.Monitor(JsonConvert.SerializeObject(request.Content.ReadAsStringAsync().Result));
                    //获取抛出自定义异常，有拦截器统一解析
                    throw new HttpResponseException(new HttpResponseMessage()
                    {
                        //封装处理异常信息，返回指定JSON对象
                        Content = new StringContent(new ResponseBase("101", error.Message).ToJson()),
                        ReasonPhrase = "Exception"
                    });
                }
                else
                {
                    return response;
                }
            });
        }
    }
}