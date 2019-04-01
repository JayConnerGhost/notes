using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Notepad.Dtos;
using Notepad.Repositories;
using Notepad.Services;
using Notepad.TODO.Tests;

namespace Notepad.UI
{
    public class TodoController:ITodoController
    {
        private readonly ILoggingController _loggingController;
        private readonly ITodoService _service;
        private readonly TodoFrame _frame;
        private readonly Dictionary<AreaNames, FlowLayoutPanel> _areas=new Dictionary<AreaNames, FlowLayoutPanel>();

        public TodoController(ILoggingController loggingController, ITodoService service, TodoFrame frame)
        {
            _loggingController = loggingController;
            _service = service;
            _frame = frame;
            _frame.Closing += _frame_Closing;
            CustomizePanels(_frame);
        }

        private void CustomizePanels(TodoFrame frame)
        {
            var panelTodo = frame.GetPanelTodo();
            var panelDoing = frame.GetPanelDoing();
            var panelDone = frame.GetPanelDone();
            //Set up colors - single scheme for now 
            panelDoing.BackColor=Color.BurlyWood;
            panelTodo.BackColor=Color.AntiqueWhite;
            panelDone.BackColor = Color.Gold;

            //add wrap panals (flowpanals) 
            var todoArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.AntiqueWhite};
            var doingArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.BurlyWood};
            var doneArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.Gold};
            panelDoing.Controls.Add(doingArea);
            panelTodo.Controls.Add(todoArea);
            panelDone.Controls.Add(doneArea);
            _areas.Add(AreaNames.Todo, todoArea);
            _areas.Add(AreaNames.Doing, doingArea);
            _areas.Add(AreaNames.Done, doneArea);
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

        public void Add(string name, string description)
        {
            _service.Create(new TodoItem(name,description));
        }

        public void GetAll()
        {
            var todoItems = _service.GetAll();
            _frame.TodoItems=todoItems;
        }
    }

    internal enum AreaNames
    {
        Todo,
        Doing,
        Done
    }
}