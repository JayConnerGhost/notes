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
        private readonly Dictionary<PositionNames, FlowLayoutPanel> _areas=new Dictionary<PositionNames, FlowLayoutPanel>();
        IList<TodoItem> _todoItems;

        public TodoController(ILoggingController loggingController, ITodoService service, TodoFrame frame)
        {
            _loggingController = loggingController;
            _service = service;
            _frame = frame;
            _frame.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            _frame.Closing += _frame_Closing;
            CustomizePanels(_frame);
            LoadPanels();
        }

        private void CustomizePanels(TodoFrame frame)
        {
            var tableLayoutPanel = new TableLayoutPanel {Dock = DockStyle.Fill, RowCount = 1, ColumnCount = 3};
           

            _frame.Controls.Add(tableLayoutPanel);

            //add wrap panals (flowpanals) 
            var todoArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.AntiqueWhite, WrapContents = false,AutoScroll = true};
            var doingArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.BurlyWood,WrapContents = false, AutoScroll = true};
            var doneArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,BackColor = Color.Gold, WrapContents=false, AutoScroll = true};
            
            var columnStyle1=new ColumnStyle(SizeType.Percent,30f);
            var columnStyle2=new ColumnStyle(SizeType.Percent,30f);
            var columnStyle3=new ColumnStyle(SizeType.Percent,30f);

            tableLayoutPanel.ColumnStyles.Add(columnStyle1);
            tableLayoutPanel.ColumnStyles.Add(columnStyle2);
            tableLayoutPanel.ColumnStyles.Add(columnStyle3);

            tableLayoutPanel.Controls.Add(todoArea,0,0);
            tableLayoutPanel.Controls.Add(doingArea, 1,0);
            tableLayoutPanel.Controls.Add(doneArea, 2,0);
            
            _areas.Add(PositionNames.Todo, todoArea);
            _areas.Add(PositionNames.Doing, doingArea);
            _areas.Add(PositionNames.Done, doneArea);
        }

        private void LoadPanels()
        {
          
            Add("test ","test ");
            Add("test 2 ","test 2");
            Add("test 3","test 3");
            Add("test 4","test 4");
            Add("test 5","test 4");
            GetAll();
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
            _service.Create(new TodoItem(name, description));
        }

        public void GetAll()
        {
          _todoItems = _service.GetAll();
          PopulateBoard(_todoItems);
        }

        private void PopulateBoard(IList<TodoItem> todoItems)
        {
            foreach (var todoItem in todoItems)
            {
                var control = new Controls.TodoItem();
                control.LoadData((Notepad.Dtos.TodoItem) todoItem);
                _areas[todoItem.Position].Controls.Add(control);
            }
        }

    }
}