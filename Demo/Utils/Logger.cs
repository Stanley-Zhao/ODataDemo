using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Demo.Utils
{
    public class Logger
    {
        #region Members
        private static Logger _logger;
        private static object syncRoot = new object();
        private string logFileName = string.Empty;
        #endregion

        #region Constant Values
        private const string FILE_NAME_FORMAT = @"DEMO_{0}.log";
        private const string TIME_FORMAT_IN_FILE_NAME = "yyyyMMdd";
        private const string TIME_FORMAT_IN_LOG = "yyyyMMdd hh:mm:ss.ff";
        private const string LOG_FOLDER_NAME = "logs";        
        private const string LOG_TYPE_INFO = "INFO";
        private const string LOG_TYPE_WARNING = "WARNING";
        private const string LOG_TYPE_ERROR = "ERROR";
        // {0} - INFO / WARNING / ERROR 
        // {1} - Time
        // {2} - suffix for info / warning / error log, like: >>>>>>>>>>>>>> or ************
        private const string LOG_FORMAT = "[{0} {1}]{2}\r\n{3}\r\n";

        #endregion

        #region Private Methods
        private Logger()
        {
            logFileName = string.Format(FILE_NAME_FORMAT, DateTime.Now.ToString(TIME_FORMAT_IN_FILE_NAME));

            // TODO - FIX ME, hard coded path.            
            string logFolder = Path.Combine(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), LOG_FOLDER_NAME));
            if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);

            logFileName = Path.Combine(logFolder, logFileName);

            //if (!File.Exists(logFileName)) { File.Create(logFileName);}

            Write("============================\r\nLogger Created\r\n============================\r\n");
        }

        private void WriteLog(string logType, string msg)
        {
            string logTime = DateTime.Now.ToString(TIME_FORMAT_IN_LOG);
            string suffixSign = string.Empty;
            switch (logType)
            {
                case LOG_TYPE_WARNING:
                    suffixSign = "********";
                    break;
                case LOG_TYPE_ERROR:
                    suffixSign = ">>>>>>>>";
                    break;
            }

            Write(string.Format(LOG_FORMAT, logType, logTime, suffixSign, msg));
        }

        private void Write(string str)
        {
            lock(syncRoot)
            {
                File.AppendAllText(logFileName, str + Environment.NewLine);
            }
        }
        #endregion

        #region Public Methods
        public static Logger Instance
        {
            get
            {
                if (_logger == null)
                {
                    lock (syncRoot)
                    {
                        if (_logger == null)
                            _logger = new Logger();
                    }
                }
                return _logger;
            }
        }

        public void Info(string info)
        {
            WriteLog(LOG_TYPE_INFO, info);
        }

        public void Warning(string info)
        {
            WriteLog(LOG_TYPE_WARNING, info);
        }

        public void Error(string info)
        {
            WriteLog(LOG_TYPE_ERROR, info);
        }
        #endregion
    }
}