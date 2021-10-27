using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WmsPrism
{
    public class Logger
    {
        static readonly object locker = new object();
        NLog.Logger _logger;

        private Logger(NLog.Logger logger)
        {
            _logger = logger;
        }

        public Logger(string name) : this(LogManager.GetLogger(name))
        {

        }

        public static Logger Default { get; private set; }
        static Logger()
        {
            Default = new Logger(NLog.LogManager.GetCurrentClassLogger());
        }

        public static void WriteLog(string name, string msg)
        {
            LogModels lm = new LogModels();
            lm.Name = name;
            lm.msg = msg;

            ParameterizedThreadStart tStart = new ParameterizedThreadStart(ThreadWriteLog);
            Thread thread = new Thread(tStart);
            thread.Start(lm);//传递参数 
        }

        public static void ThreadWriteLog(object arg)
        {
            lock (locker)
            {
                LogModels lm = (LogModels)arg;
                LogManager.Configuration.Variables["LogDir"] = lm.Name;
                Logger logger = new Logger("ESDLog");
                logger.Info(lm.msg);
            }
        }

        #region Debug
        public void Debug(string msg, params object[] args)
        {
            _logger.Debug(msg, args);
        }

        public void Debug(string msg, Exception err)
        {
            _logger.Debug(err, msg);
        }
        #endregion

        #region Info
        public void Info(string msg, params object[] args)
        {
            _logger.Info(msg, args);
        }

        public void Info(string msg, Exception err)
        {
            _logger.Info(err, msg);
        }
        #endregion
        #region Custom
        #endregion

    }

    public class MyLogEventInfo : LogEventInfo
    {
        public MyLogEventInfo() { }
        public MyLogEventInfo(LogLevel level, string loggerName, string message) : base(level, loggerName, message)
        { }

        public override string ToString()
        {
            //Message format
            //Log Event: Logger='XXX' Level=Info Message='XXX' SequenceID=5
            return FormattedMessage;
        }
    }

    public class LogModels
    {
        public string Name { set; get; }
        public string msg { set; get; }
    }
}
