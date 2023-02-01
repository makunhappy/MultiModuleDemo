using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ILogService
    {
        void Debug(ServiceCategory serviceCategory,
            string message);
        void Info(ServiceCategory serviceCategory,
            string message);
        void Warn(ServiceCategory serviceCategory,
            string message);
        void Error(ServiceCategory serviceCategory,
            string message);
        void Error(ServiceCategory serviceCategory,
            string message, Exception e);
        void Fatal(ServiceCategory serviceCategory,
            string message);
    }    
}
