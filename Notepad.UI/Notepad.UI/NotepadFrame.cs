using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkinFramework;

namespace Notepad.UI
{
    public partial class NotepadFrame : Form
    {
        public MainController Controller { get; set; }

        public NotepadFrame()
        {
            InitializeComponent();
            
        }

        public void SetSkinLight()
        {
            DefaultSkin skin = DefaultSkin.Office2007Luna;
            skinningManager1.DefaultSkin = skin;
        }

        public void SetSkinBlack()
        {
            DefaultSkin skin = DefaultSkin.Office2007Obsidian;
            skinningManager1.DefaultSkin = skin;
        }

        private void NotepadFrame_Load(object sender, EventArgs e)
        {
            
            AllowDrop = true;
        }
    }
}
