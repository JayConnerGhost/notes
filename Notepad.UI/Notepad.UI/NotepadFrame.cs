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
    public partial class NotepadFrame : Form
    {
        public MainController Controller { get; set; }

        public NotepadFrame()
        {
            InitializeComponent();
            
        }

        private void NotepadFrame_Load(object sender, EventArgs e)
        {
            AllowDrop = true;
        }
    }
}
