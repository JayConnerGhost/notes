using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.UI
{
    public partial class TodoFrame : Form
    {
        public TodoFrame()
        {
            InitializeComponent();
        }

        public SplitterPanel GetPanelTodo()
        {
            return splitContainer1.Panel1;
        }

        public SplitterPanel GetPanelDoing()
        {
            return splitContainer2.Panel1;
        }

        public SplitterPanel GetPanelDone()
        {
            return splitContainer2.Panel2;
        }
    }
}
