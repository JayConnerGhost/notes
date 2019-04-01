using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Notepad.Dtos;

namespace Notepad.UI
{
    public partial class TodoFrame : Form
    {
        public TodoFrame()
        {
            InitializeComponent();
        }

        public IList<TodoItem> TodoItems { get; set; }
    }
}
