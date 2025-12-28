using petShop.Model;
using System;
using System.IO;

namespace petShop.Services
{
    public class FileLogService : ILogService
    {
        private const string LogFile = "log.txt";

        public void Log(LogType logType, string message)
        {
            File.AppendAllText(LogFile,
                $"{DateTime.Now}: [{logType}] {message}{Environment.NewLine}");
        }
    }
}
