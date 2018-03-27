using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace Common.Services
{
    public class ReNameMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        //关联对象纳入命名
        public string Uid { get; set; }

        public int Tag { get; set; }

        public ReNameMultipartFormDataStreamProvider(string path, string uid) : base(path)
        {
            Uid = uid;

            Tag = 1;
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            //修改图片名称并返回  
            string newFileName = string.Empty;
            newFileName = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//获取后缀名  
            //newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(0, 99999) + newFileName;
            newFileName = Uid + "_" + Tag.ToString() + newFileName;
            Tag++;
            return newFileName;
        }
    }
}
