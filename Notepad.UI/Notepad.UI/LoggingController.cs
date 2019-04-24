using System;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class LoggingController:ILoggingController
    {
        private TextBox logView;
        private readonly TabPage _loggingArea;

        public LoggingController(TabPage loggingArea)
        {
            _loggingArea = loggingArea;
            ComposeLoggingArea();
        }

        public void Log(MessageType messageType, string message)
        {
            logView.Text = logView.Text + Environment.NewLine + messageType.ToString() + " - " + message;
        }

        private void ComposeLoggingArea()
        {
            
            logView = new TextBox()
            {
                ReadOnly = true,
                Multiline = true,
                Dock=DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical
            };
            _loggingArea.Controls.Add(logView);
        }
    }

    public enum MessageType
    {
        Error,
        information
    }
}