using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSpell.SpellChecker;
using Notepad.UI;

namespace Notepad.UI
{
    public class NotepadController
    {
        private readonly NotepadFrame _notepadFrame;
        private readonly SplitterPanel _notepadControlHolderPanel1;
        private readonly MenuStrip _notepadMainMenu;
        private  RichTextBox _text;
        private readonly FormState _formState = new FormState();
        internal Spelling SpellChecker;
        private readonly SplitterPanel _notepadControlHolderPanel2;
        private FileBrowserController _fileBrowserController;

        public NotepadController(NotepadFrame notepadFrame)
        {
            _notepadFrame = notepadFrame;
            _notepadControlHolderPanel1 = notepadFrame.splitControlArea.Panel1;
            _notepadControlHolderPanel2 = notepadFrame.splitControlArea.Panel2;
            _fileBrowserController=new FileBrowserController(_notepadControlHolderPanel1);
            _fileBrowserController.OpenFile+=OpenFile;
            _notepadMainMenu = notepadFrame.menuMain;
            CompositionStepTwo_addMenu();
            CompositionStepOne_addText();
            CompositionStepThree_setStyle();
            CompositionStepFour_setUpSpelling();
            CompositionStepFive_setUpContextMenu();

        }

        private void OpenFile(object sender, EventArgs e)
        {
            var eventArgs = (ListViewItemSelectionChangedEventArgs) e;

            // MessageBox.Show((string)eventArgs.Item.Tag);
            openFile((string) eventArgs.Item.Tag);

        }

        private void CompositionStepFive_setUpContextMenu()
        {
            var contextMenu=new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Cut", Cut_Click));
            contextMenu.MenuItems.Add(new MenuItem("Copy", Copy_Click));
            contextMenu.MenuItems.Add(new MenuItem("Paste", Paste_Click));
            _text.ContextMenu = contextMenu;
        }

        private void CompositionStepFour_setUpSpelling()
        {
            //http://www.loresoft.com/The-NetSpell-project
            SpellChecker = new Spelling();
            SpellChecker.ShowDialog = true;
            SpellChecker.MisspelledWord += new Spelling.MisspelledWordEventHandler(SpellChecker_MisspelledWord);
            SpellChecker.EndOfText += new Spelling.EndOfTextEventHandler(SpellChecker_EndOfText);
            SpellChecker.DoubledWord += new Spelling.DoubledWordEventHandler(SpellChecker_DoubledWord);
        }

        private void CompositionStepThree_setStyle()
        {
            _text.BackColor = Color.White;
            _text.ForeColor = Color.Black;
            _text.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private void CompositionStepOne_addText()
        {

            _text = new RichTextBox()
            {
                Dock = DockStyle.Fill, Multiline = true, AcceptsTab = true, AllowDrop = true,
                BorderStyle = BorderStyle.None
            };

            _notepadControlHolderPanel2.Controls.Add(_text);
        }

        private void CompositionStepTwo_addMenu()
        {
            var mnuFile =new ToolStripMenuItem("File");
            var mnuEdit = new ToolStripMenuItem("Edit");
            var mnuView = new ToolStripMenuItem("View");
            var mnuTools = new ToolStripMenuItem("Tools");
            ComposeFileMenu(mnuFile);
            ComposeEditMenu(mnuEdit);
            ComposeViewMenu(mnuView);
            ComposeToolsMenu(mnuTools);

            _notepadMainMenu.Items.Add(mnuFile);
            _notepadMainMenu.Items.Add(mnuEdit);
            _notepadMainMenu.Items.Add(mnuView);
            _notepadMainMenu.Items.Add(mnuTools);
            _notepadMainMenu.Dock = DockStyle.Top;
        }

        private void ComposeToolsMenu(ToolStripMenuItem mnuTools)
        {
            mnuTools.DropDownItems.Add(new ToolStripMenuItem("Spell Check", null, new EventHandler(SpellCheck_Click),
                Keys.Alt | Keys.S));
        }

        private void ComposeViewMenu(ToolStripMenuItem mnuView)
        {
            mnuView.DropDownItems.Add(new ToolStripMenuItem("High Contrast", null, new EventHandler(HighContrast_Click),
                Keys.Alt | Keys.C));
            mnuView.DropDownItems.Add(new ToolStripMenuItem("Regular", null, new EventHandler(RegularContrast_Click),
                Keys.Alt | Keys.R));
            mnuView.DropDownItems.Add(new ToolStripMenuItem("Hacker", null, new EventHandler(HackerContrast_Click),
                Keys.Alt | Keys.H));
            mnuView.DropDownItems.Add(new ToolStripMenuItem("Distraction Free", null, new EventHandler(DistractionFree_Click),
                Keys.Alt | Keys.D));
            mnuView.DropDownItems.Add(new ToolStripMenuItem("Normal Mode", null, new EventHandler(NormalMode_Click),
                Keys.Alt | Keys.U));
        }

        private void ComposeEditMenu(ToolStripMenuItem mnuEdit)
        {
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Copy", null, new EventHandler(Copy_Click), Keys.Control | Keys.C));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Cut", null, new EventHandler(Cut_Click), Keys.Control | Keys.U));
            mnuEdit.DropDownItems.Add(
                new ToolStripMenuItem("Paste", null, new EventHandler(Paste_Click), Keys.Control | Keys.P));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Select All", null, new EventHandler(SelectAll_Click),
                Keys.Control | Keys.A));
            mnuEdit.DropDownItems.Add(new ToolStripMenuItem("Search", null, new EventHandler(Search_Click),
                Keys.Control | Keys.H));
        }

        private void ComposeFileMenu(ToolStripMenuItem mnuFile)
        {
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("New", null, new EventHandler(New_Click), Keys.Control | Keys.F));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Save", null, new EventHandler(Save_Click), Keys.Control | Keys.S));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Open", null, new EventHandler(Open_Click), Keys.Control | Keys.O));
            mnuFile.DropDownItems.Add(
                new ToolStripMenuItem("Close", null, new EventHandler(Close_Click), Keys.Control | Keys.X));
        }

        private void SpellCheck_Click(object sender, EventArgs e)
        {
            SpellChecker.Text = _text.Text;
            SpellChecker.SpellCheck();
        }

        private void SpellChecker_DoubledWord(object sender, SpellingEventArgs args)
        {
            // update text  
            _text.Text = SpellChecker.Text;
        }

        private void SpellChecker_EndOfText(object sender, EventArgs args)
        {
            // update text  
            _text.Text = SpellChecker.Text;
        }

        private void SpellChecker_MisspelledWord(object sender, SpellingEventArgs args)
        {
            // update text  
            _text.Text = SpellChecker.Text;
        }



        private void NormalMode_Click(object sender, EventArgs e)
        {
            _formState.Restore(_notepadFrame);
        }

        private void DistractionFree_Click(object sender, EventArgs e)
        {
          _formState.Maximize(_notepadFrame);
        }

        private void HackerContrast_Click(object sender, EventArgs e)
        {
            _text.BackColor = Color.Black;
            _text.ForeColor = Color.ForestGreen;
            _text.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private void RegularContrast_Click(object sender, EventArgs e)
        {
            CompositionStepThree_setStyle();
        }

        private void HighContrast_Click(object sender, EventArgs e)
        {
            _text.BackColor=Color.Black;
            _text.ForeColor=Color.Yellow;
            _text.Font=new Font(FontFamily.GenericSerif, 20,FontStyle.Regular,GraphicsUnit.Pixel);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            _notepadFrame.Close();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI

            int index = 0;
            String temp = _text.Text;
            _text.Text = "";
            _text.Text = temp;

            var searchController=new SearchController();
           var searchTerm= searchController.ShowDialog();

           while (index < _text.Text.LastIndexOf(searchTerm))
           {
               _text.Find(searchTerm, index, _text.TextLength, RichTextBoxFinds.None);
               _text.SelectionBackColor = Color.Aqua;
               index = _text.Text.IndexOf(searchTerm, index) + 1;
           }
        

        }


        private void SelectAll_Click(object sender, EventArgs e)
        {
            _text.SelectAll();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
           _text.Paste();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (_text.SelectedText.Length > 0)
            {
                _text.Cut();
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (_text.SelectedText.Length > 0)
            {
                _text.Copy();
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var openFileDialog1=new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                openFile(fileName);
            }
        }

        private void openFile(string fileName)
        {
            if (fileName == null)
            {
                return;
            }

            System.IO.StreamReader sr = new
                System.IO.StreamReader(fileName);

            _text.Text = sr.ReadToEnd();
            sr.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog {Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Save a Text File"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                string data = _text.Text;
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
            _text.Text = null;
        }

        
        
    }
}