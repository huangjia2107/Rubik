using System;

using log4net;
using log4net.Config;

namespace Rubik.Service.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger
    {
        //获取配置
        private static readonly ILog _main_logger = LogManager.GetLogger("main_logger");
        private static readonly ILog _demo_Logger = LogManager.GetLogger("demo_logger");
        private static readonly ILog _module_Logger = LogManager.GetLogger("module_logger");
        private static readonly ILog _webapi_Logger = LogManager.GetLogger("webapi_logger");
        
        //单例
        public static readonly Logger Instance = new Logger();

        private Logger()
        {
            //设置配置
            XmlConfigurator.Configure(new Uri(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
        }

        /// <summary>
        /// 宿主程序 Log 记录器
        /// </summary>
        public ILog Main => _main_logger;

        /// <summary>
        /// Demo Log 记录器
        /// </summary>
        public ILog Demo => _demo_Logger;

        /// <summary>
        /// Module Log 记录器
        /// </summary>
        public ILog Module => _module_Logger;

        /// <summary>
        /// WebApi Log 记录器
        /// </summary>
        public ILog WebApi => _webapi_Logger;
    }
}
