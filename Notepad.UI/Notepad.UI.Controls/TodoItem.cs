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
    public partial class TodoItem: UserControl
    {
        public event EventHandler<RemoveTaskEventArgs> RemoveTask;
        
        public int Id=0;
        public TodoItem()
        {
            InitializeComponent();
        }


        public void LoadData(Notepad.Dtos.TodoItem todoItem)
        {
            Id = todoItem.Id;
            this.txtName.Text = todoItem.Name;
            this.txtDescription.Text = todoItem.Description;
        }

    

        public class RemoveTaskEventArgs : EventArgs
        {
            public int Id{get;set;}
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveTask?.Invoke(this, new RemoveTaskEventArgs {Id = this.Id});
        }
    }
}
