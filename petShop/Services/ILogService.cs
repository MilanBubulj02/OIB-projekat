using petShop.Model;

namespace petShop.Services
{
    public interface ILogService
    {
        void Log(LogType logType, string message);
    }
}
