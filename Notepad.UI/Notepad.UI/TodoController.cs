using Notepad.Dtos;
using Notepad.Services;
using Notepad.TODO.Tests;

namespace Notepad.UI
{
    public class TodoController:ITodoController
    {
        private readonly ILoggingController _loggingController;
        private readonly ITodoService _service;
        private  TodoFrame _frame;

        public TodoController(ILoggingController loggingController, ITodoService service, TodoFrame frame)
        {
            _loggingController = loggingController;
            _service = service;
            _frame = frame;
            _frame.Closing += _frame_Closing;
        }

        private void _frame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _frame.Hide();
        }

        public void Show()
        {
            _frame.Show();
        }

        public void Add(string name)
        {
            _service.Create(new TodoItem(name));
        }
    }
}