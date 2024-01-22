using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace EComm_Web_App.Filter
{
    public class Exception : ExceptionFilterAttribute,IExceptionFilter
    {
        public override void OnException(ExceptionContext context)
        {
            var exceptionMessage = context.Exception.Message;
            var stackTrace = context.Exception.StackTrace;
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var Message = "Date :" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:tt") + "Error Message:" + exceptionMessage + Environment.NewLine + "Stack Trace:" + stackTrace;
            
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string logFilePath = "0";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("dd-mm-yyyy") + ".txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.Write("Log Created At-"+DateTime.Now.ToString("dd-mm-yyyy hh:mm:tt")+"Message :-"+Message);
            log.Close();
            base.OnException(context);
        }

    }
}
