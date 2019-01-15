using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class NotepadController
    {
        private readonly NotepadFrame _notepadFrame;
        private readonly Panel _notepadControlHolder;
        private readonly MenuStrip _notepadMainMenu;
        TextBox text;
        public NotepadController(NotepadFrame notepadFrame)
        {
            _notepadFrame = notepadFrame;
            _notepadControlHolder = notepadFrame.pnlControlHolder;
            _notepadMainMenu = notepadFrame.menuMain;
            CompositionStepTwo_addMenu();
            CompositionStepOne_addText();

        }

        private void CompositionStepTwo_addMenu()
        {
            var mnuFile =new ToolStripMenuItem("file");
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("New",null,new EventHandler(New_Click)));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Save", null, new EventHandler(Save_Click)));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Open", null, new EventHandler(Open_Click)));
            _notepadMainMenu.Items.Add(mnuFile);
            _notepadMainMenu.Dock = DockStyle.Top;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var openFileDialog1=new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                    System.IO.StreamReader(openFileDialog1.FileName);
               // MessageBox.Show(sr.ReadToEnd());
               text.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog {Filter = "txt|*.txt", Title = "Save a Text File"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                string data = text.Text;
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info,0,info.Length);
                byte[] btData = new byte[] { 0x0 };
                fs.Write(btData, 0, btData.Length);
                
                fs.Close();
                fs = null;
            }
        }

        private void New_Click(object sender, EventArgs e)
        {
            text.Text = null;
        }

        private void CompositionStepOne_addText()
        {
           
            text = new TextBox {Dock = DockStyle.Fill, Multiline = true};

            _notepadControlHolder.Controls.Add(text);
        }
        
    }
}