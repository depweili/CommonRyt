using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{
    public class Log
    {
        private ILog logger;
        public Log(ILog log)
        {
            this.logger = log;
        }
        public void Debug(object message)
        {
            this.logger.Debug(message);
        }
        public void Error(object message)
        {
            this.logger.Error(message);
        }
        public void Info(object message)
        {
            this.logger.Info(message);
        }
        public void Warn(object message)
        {
            this.logger.Warn(message);
        }
        public void Fatal(object message)
        {
            this.logger.Fatal(message);
        }


        public void Debug(object message, Exception ex)
        {
            this.logger.Debug(message, ex);
        }
        public void Error(object message, Exception ex)
        {
            this.logger.Error(message, ex);
        }
        public void Info(object message, Exception ex)
        {
            this.logger.Info(message, ex);
        }
        public void Warn(object message, Exception ex)
        {
            this.logger.Warn(message, ex);
        }
        public void Fatal(object message, Exception ex)
        {
            this.logger.Fatal(message, ex);
        }
    }

    public class LogFactory
    {
        static LogFactory()
        {
            
            //FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("~/Configs/log4net.config"));

            FileInfo configFile = new FileInfo(PathHelper.MapPath("~/Configs/log4net.config"));

            if (!configFile.Exists)
            {
                throw new Exception("log4net.config Error:" + configFile.FullName);
            }

            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static Log GetLogger(Type type)
        {
            return new Log(LogManager.GetLogger(type));
        }
        public static Log GetLogger(string str)
        {
            return new Log(LogManager.GetLogger(str));
        }
    }

    public class LogHelper
    {
        private static readonly Log logerror = LogFactory.GetLogger("logerror");
        private static readonly Log loginfo = LogFactory.GetLogger("loginfo");
        private static readonly Log logmonitor = LogFactory.GetLogger("logmonitor");

        public static void Error(string ErrorMsg, Exception ex = null)
        {
            logerror.Error(ErrorMsg, ex);
        }

        public static void Info(string Msg)
        {
            loginfo.Info(Msg);
        }

        public static void Monitor(string Msg)
        {
            logmonitor.Info(Msg);
        }
    }
}
