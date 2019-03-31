using Notepad.UI;

namespace Notepad.TODO.Tests
{
    public interface ILoggingController
    {
        void Log(MessageType information, string message);
    }
}