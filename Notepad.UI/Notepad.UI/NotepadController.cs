using System;
using System.Data.SqlTypes;
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
        private  RichTextBox text;
        public NotepadController(NotepadFrame notepadFrame)
        {
            _notepadFrame = notepadFrame;
            _notepadControlHolder = notepadFrame.pnlControlHolder;
            _notepadMainMenu = notepadFrame.menuMain;
            CompositionStepTwo_addMenu();
            CompositionStepOne_addText();

        }

        private void CompositionStepOne_addText()
        {

            text = new RichTextBox() { Dock = DockStyle.Fill, Multiline = true };

            _notepadControlHolder.Controls.Add(text);
        }

        private void CompositionStepTwo_addMenu()
        {
            var mnuFile =new ToolStripMenuItem("File");
            var mnuEdit = new ToolStripMenuItem("Edit");
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("New",null,new EventHandler(New_Click),Keys.Control | Keys.F));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Save", null, new EventHandler(Save_Click),Keys.Control | Keys.S));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Open", null, new EventHandler(Open_Click),Keys.Control | Keys.O));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Close", null, new EventHandler(Close_Click), Keys.Control | Keys.X));

            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Copy", null, new EventHandler(Copy_Click), Keys.Control | Keys.C));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Cut", null, new EventHandler(Cut_Click), Keys.Control | Keys.U));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Paste", null, new EventHandler(Paste_Click), Keys.Control | Keys.P));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Select All", null, new EventHandler(SelectAll_Click), Keys.Control | Keys.A));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Search", null, new EventHandler(Search_Click), Keys.Control | Keys.H));

            _notepadMainMenu.Items.Add(mnuFile);
            _notepadMainMenu.Items.Add(mnuEdit);
            _notepadMainMenu.Dock = DockStyle.Top;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            _notepadFrame.Close();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI

            int index = 0;
            String temp = text.Text;
            text.Text = "";
            text.Text = temp;

            var searchController=new SearchController();
           var searchTerm= searchController.ShowDialog();

           while (index < text.Text.LastIndexOf(searchTerm))
           {
               text.Find(searchTerm, index, text.TextLength, RichTextBoxFinds.None);
               text.SelectionBackColor = Color.Aqua;
               index = text.Text.IndexOf(searchTerm, index) + 1;
           }
        

        }


        private void SelectAll_Click(object sender, EventArgs e)
        {
            text.SelectAll();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
           text.Paste();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (text.SelectedText.Length > 0)
            {
                text.Cut();
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (text.SelectedText.Length > 0)
            {
                text.Copy();
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var openFileDialog1=new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                    System.IO.StreamReader(openFileDialog1.FileName);
            
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

        
        
    }
}