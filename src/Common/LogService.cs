using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum ServiceCategory
    {
        Common,
        TubeWarmUp,
        AutoCali,
    }
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
    }
    public class LogService : ILogService
    {
        private const string NAMESPACEPREFIX = "NV.CT.Service.";
        private readonly Dictionary<ServiceCategory, string> _categoryNames;
        //private static readonly Lazy<LogService> _instance =
        //    new Lazy<LogService>(() => new LogService());
        //public static LogService Instance => _instance.Value;
        public LogService()
        {
            _categoryNames = new Dictionary<ServiceCategory, string>();
            foreach (var category in Enum.GetValues<ServiceCategory>())
            {
                _categoryNames.Add(category, $"{NAMESPACEPREFIX}{category}");
            }
        }
        public void Debug(ServiceCategory serviceCategory,
            string message)
        {
            Write(serviceCategory, LogType.Debug, message);
            WriteLogServer(serviceCategory, LogType.Debug, message);
        }
        public void Info(ServiceCategory serviceCategory,
            string message)
        {
            Write(serviceCategory, LogType.Info, message);
            WriteLogServer(serviceCategory, LogType.Info, message);
        }
        public void Warn(ServiceCategory serviceCategory,
            string message)
        {
            Write(serviceCategory, LogType.Warning, message);
            WriteLogServer(serviceCategory, LogType.Warning, message);
        }
        public void Error(ServiceCategory serviceCategory,
            string message)
        {
            Error(serviceCategory, message, null);
        }
        public void Error(ServiceCategory serviceCategory,
            string message, Exception e)
        {
            Write(serviceCategory, LogType.Error, $"{message}{ExtractStackInfo(e)}");
            WriteLogServer(serviceCategory, LogType.Error, $"{message}{ExtractStackInfo(e)}");
        }
        public void Fatal(ServiceCategory serviceCategory,
            string message)
        {
            Write(serviceCategory, LogType.Fatal, message);
            WriteLogServer(serviceCategory, LogType.Fatal, message);
        }
        private string ExtractStackInfo(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }
            return stringBuilder.ToString();
        }
        private string GetCategoryName(ServiceCategory serviceCategory)
        {
            string categoryName = string.Empty;
            if (_categoryNames.TryGetValue(serviceCategory, out var category))
            {
                categoryName = category;
            }
            else
            {
                categoryName = serviceCategory.ToString();
            }
            return categoryName;
        }
        private void Write(ServiceCategory serviceCategory,
            LogType type, string msg)
        {
            var categoryName = GetCategoryName(serviceCategory);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {categoryName} {type} {msg}");
        }
        private void WriteLogServer(ServiceCategory serviceCategory,
            LogType logType, string msg)
        {
            var categoryName = GetCategoryName(serviceCategory);
            switch (logType)
            {
                case LogType.Debug:
                    //NV.CT.Logging.LogManager.Instance.Debug(categoryName, msg);
                    break;
                case LogType.Info:
                    //NV.CT.Logging.LogManager.Instance.Information(categoryName, msg);
                    break;
                case LogType.Warning:
                    //NV.CT.Logging.LogManager.Instance.Warning(categoryName, msg);
                    break;
                case LogType.Error:
                    //NV.CT.Logging.LogManager.Instance.Error(categoryName, msg);
                    break;
                case LogType.Fatal:
                    //NV.CT.Logging.LogManager.Instance.Fatal(categoryName, msg);
                    break;
                default:
                    break;
            }
        }
    }
}
