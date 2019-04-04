using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.UI.Controls
{
    public partial class TodoItem : UserControl
    {
        public event EventHandler<RemoveTodoTaskEventArgs> RemoveTodoTask;
        public event EventHandler<SaveTodoTaskEventArgs> SaveTodoTask;

        public int Id = 0;
        public PositionNames Position;

        public TodoItem()
        {
            InitializeComponent();

        }


        public void LoadData(Notepad.Dtos.TodoItem todoItem)
        {
            Id = todoItem.Id;
            this.txtName.Text = todoItem.Name;
            this.txtDescription.Text = todoItem.Description;
            Position = todoItem.Position;
        }



        public class RemoveTodoTaskEventArgs : EventArgs
        {
            public int Id { get; set; }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveTodoTask?.Invoke(this, new RemoveTodoTaskEventArgs { Id = this.Id });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTodoTask?.Invoke(this, new SaveTodoTaskEventArgs { Id = this.Id, Name = this.txtName.Text, Description = this.txtDescription.Text, Position = this.Position });
        }


        public string ItemName
        {
            get
            {
                return txtName.Text;
            }
            set
            {
                txtName.Text = value;
            }
        }

        public string Description => txtDescription.Text;
        public int ID => this.Id;
    }

    public class SaveTodoTaskEventArgs:EventArgs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PositionNames Position { get; set; }
    }
}
