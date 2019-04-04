using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Notepad.Dtos;
using Notepad.Repositories;
using Notepad.Services;
using Notepad.TODO.Tests;
using Notepad.UI.Controls;

namespace Notepad.UI
{
    public class TodoController:ITodoController
    {
        private readonly ILoggingController _loggingController;
        private readonly ITodoService _service;
        private readonly TodoFrame _frame;
        private readonly Dictionary<PositionNames, FlowLayoutPanel> _areas=new Dictionary<PositionNames, FlowLayoutPanel>();
        IList<Dtos.TodoItem> _todoItems;
        private Dictionary<int,UI.Controls.TodoItem> todoControls=new Dictionary<int, Controls.TodoItem>();
        FlowLayoutPanel todoArea;
        FlowLayoutPanel doingArea;
        FlowLayoutPanel doneArea;
        public TodoController(ILoggingController loggingController, ITodoService service, TodoFrame frame)
        {
            _loggingController = loggingController;
            _service = service;
            _frame = frame;
            _frame.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            _frame.Closing += _frame_Closing;
            CustomizePanels(_frame);
            LoadPanels();
         //   populateTestData();
            _loggingController.Log(MessageType.information, "TODO Board Setup");
        }

        private void populateTestData()
        {
            //temp to be removed 
        
            Add("test dev 2", "test 2");
            Add("test dev 3", "test 3");
            Add("test dev 4", "test 4");
            Add("test dev 5", "test 4");
            _loggingController.Log(MessageType.information, "Loading DEVELOPMENT DATA");
        }

        private void CustomizePanels(TodoFrame frame)
        {
            var tableLayoutPanel = new TableLayoutPanel {Dock = DockStyle.Fill, RowCount = 1, ColumnCount = 3,HorizontalScroll = { Enabled = false, Visible = false}};
            _frame.Controls.Add(tableLayoutPanel);

            //add wrap panals (flowpanals) 
             todoArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false,AutoScroll = false,HorizontalScroll = { Visible = false,Enabled = false},Height = 576,BorderStyle = BorderStyle.FixedSingle, AllowDrop=true};
             doingArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,WrapContents = false, AutoScroll = false, HorizontalScroll = { Visible = false,Enabled=false }, Height = 576, BorderStyle = BorderStyle.FixedSingle , AllowDrop=true};
             doneArea = new FlowLayoutPanel {Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents=false, AutoScroll = false, Height = 576, HorizontalScroll = { Visible = false , Enabled=false}, BorderStyle = BorderStyle.FixedSingle , AllowDrop=true};


            todoArea.DragEnter += TodoArea_DragEnter;
            doingArea.DragEnter += DoingArea_DragEnter;
            doneArea.DragEnter += DoneArea_DragEnter;


            todoArea.DragDrop += TodoArea_DragDrop;
            doingArea.DragDrop += DoingArea_DragDrop;
            doneArea.DragDrop += DoneArea_DragDrop;
            
            // https://www.codeproject.com/Questions/571197/disableplustheplushorizontalplusscrollbarplusinplu
            todoArea.AutoScroll = true;
            doingArea.AutoScroll = true;
            doneArea.AutoScroll = true;

            var columnStyle1=new ColumnStyle(SizeType.Percent,30f);
            var columnStyle2=new ColumnStyle(SizeType.Percent,30f);
            var columnStyle3=new ColumnStyle(SizeType.Percent,30f);

            //var todoHeader = new Label{Text = "TODO",TextAlign = ContentAlignment.MiddleCenter, Width = 320, BackColor = Color.AntiqueWhite,Anchor = AnchorStyles.Top};
            var todoHeader = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Height = 20, Width=320 ,BackColor= Color.AntiqueWhite };
            var AddButton=new Button { Text = "+", Width = 20, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.AntiqueWhite, Anchor = AnchorStyles.Top, Margin = Padding.Empty, FlatStyle = FlatStyle.Flat };
            AddButton.FlatAppearance.BorderSize = 0;
            AddButton.Click += AddButton_Click;
            todoHeader.Controls.Add(AddButton);
            todoHeader.Controls.Add(new Label { Text = "TODO", TextAlign = ContentAlignment.MiddleCenter, Width = 320, BackColor = Color.AntiqueWhite, Anchor = AnchorStyles.Top });

            

            var doingHeader = new Label{Text = "Doing", TextAlign = ContentAlignment.MiddleCenter, Width = 320, BackColor = Color.BurlyWood , Anchor = AnchorStyles.Top};
           
            var doneHeader = new Label{Text = "Done",TextAlign = ContentAlignment.MiddleCenter, Width = 320, BackColor = Color.Gold, Anchor=AnchorStyles.Top};
           

            tableLayoutPanel.ColumnStyles.Add(columnStyle1);
            tableLayoutPanel.ColumnStyles.Add(columnStyle2);
            tableLayoutPanel.ColumnStyles.Add(columnStyle3);

            tableLayoutPanel.Controls.Add(todoHeader,0,0);
            tableLayoutPanel.Controls.Add(doingHeader,1,0);
            tableLayoutPanel.Controls.Add(doneHeader,2,0);

            tableLayoutPanel.Controls.Add(todoArea,0,1);
            tableLayoutPanel.Controls.Add(doingArea, 1,1);
            tableLayoutPanel.Controls.Add(doneArea, 2,1);

            _areas.Add(PositionNames.Todo, todoArea);
            _areas.Add(PositionNames.Doing, doingArea);
            _areas.Add(PositionNames.Done, doneArea);
            _loggingController.Log(MessageType.information, "TODO Panals Setup");
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Add(string.Empty, string.Empty);
        }

        private void DoneArea_DragDrop(object sender, DragEventArgs e)
        {
            var t = ((Controls.TodoItem)e.Data.GetData(typeof(Controls.TodoItem)));
            doneArea.Controls.Add((UserControl)t);
            UpdatePosition(PositionNames.Done,t);
        }
        private void DoingArea_DragDrop(object sender, DragEventArgs e)
        {
            var t = ((Controls.TodoItem)e.Data.GetData(typeof(Controls.TodoItem)));
            doingArea.Controls.Add((UserControl)t);
            UpdatePosition(PositionNames.Doing,t);
        }


        private void TodoArea_DragDrop(object sender, DragEventArgs e)
        {
            var t = ((Controls.TodoItem)e.Data.GetData(typeof(Controls.TodoItem)));
            todoArea.Controls.Add((UserControl)t);
            UpdatePosition(PositionNames.Todo,t);
        }

        private void UpdatePosition(PositionNames newPosition, Controls.TodoItem target)
        {
            _areas[target.Position].Controls.Remove(target);
            target.Position = newPosition;
            _areas[newPosition].Controls.Add(target);
            _service.Update(target.ID, newPosition,target.Description ,target.ItemName);
        }

        private void DoneArea_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TodoArea_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DoingArea_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        

        private void LoadPanels()
        {
            GetAll();
        }

        private void _frame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _frame.Hide();
            _loggingController.Log(MessageType.information, "Hiding TODO Frame");
        }

        public void Show()
        {
            _frame.Show();
            _loggingController.Log(MessageType.information, "Showing TODO Frame");
        }

        public void Add(string name, string description)
        {
            var todo = new Dtos.TodoItem(name, description) { Position=PositionNames.Todo};
            todo.Id = _service.Create(todo);

            AddNewControl(todo);
            _loggingController.Log(MessageType.information, "Added TODO Item");
        }

        private void AddNewControl(Dtos.TodoItem todo)
        {
            var control = new Controls.TodoItem();
            control.RemoveTodoTask += ControlRemoveTodoTask;
            control.SaveTodoTask += Control_SaveTodoTask;
            control.LoadData(todo);

            control.MouseDown += Control_MouseDown;

            todoControls.Add(todo.Id, control);
            _areas[todo.Position].Controls.Add(control);
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left && e.Clicks == 1)
            {
                var target = (Control)sender;
                target.DoDragDrop(sender, DragDropEffects.Move);
            }

        }

        private void Control_SaveTodoTask(object sender, Controls.SaveTodoTaskEventArgs e)
        {

           var controlsCollection=_areas[e.Position].Controls;
           Controls.TodoItem targetControl;

           foreach (Controls.TodoItem control in controlsCollection)
           {
               if (control.Id == e.Id)
               {
                   targetControl = control;
                   break;
               }
           }

           _service.Update(e.Id, e.Position, e.Description, e.Name);

        }

        private void ControlRemoveTodoTask(object sender, Controls.TodoItem.RemoveTodoTaskEventArgs e)
        {
            todoControls[e.Id].Dispose();
            todoControls.Remove(e.Id);
            _service.Delete(e.Id);
            _loggingController.Log(MessageType.information, "Removing TODO Item");

        }

        public void GetAll()
        {
          _todoItems = _service.GetAll();
          PopulateBoard(_todoItems);
            _loggingController.Log(MessageType.information, "Loading saved TODO Items to Board ");
        }

        private void PopulateBoard(IList<Dtos.TodoItem> todoItems)
        {
            foreach (var todoItem in todoItems)
            {
                AddNewControl(todoItem);
            }
        }

    }
}