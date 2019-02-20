using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NetSpell.SpellChecker;
using Notepad.UI;

namespace Notepad.UI
{
    public class NotepadController
    {
        private enum ViewMode
        {
            normal,
            focused
        }

        private readonly NotepadFrame _notepadFrame;
        private readonly SplitterPanel _notepadControlHolderPanel1;
        private readonly MenuStrip _notepadMainMenu;
        public  RichTextBox Text;
        private readonly FormState _formState = new FormState();
        internal Spelling SpellChecker;
        private readonly SplitterPanel _notepadControlHolderPanel2;
        private FileBrowserController _fileBrowserController;
        private BrandController _brandController;
        private ViewMode _viewMode;

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
            CompositionStepFour_setUpSpelling();
            CompositionStepFive_setUpContextMenu();
     
            _brandController = new BrandController(this, _fileBrowserController);
            _brandController.SetBaseStyle();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var eventArgs = (ListViewItemSelectionChangedEventArgs) e;
            openFile((string) eventArgs.Item.Tag);
        }

        private void CompositionStepFive_setUpContextMenu()
        {
            var contextMenu=new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Cut", Cut_Click));
            contextMenu.MenuItems.Add(new MenuItem("Copy", Copy_Click));
            contextMenu.MenuItems.Add(new MenuItem("Paste", Paste_Click));
            Text.ContextMenu = contextMenu;
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

        private void CompositionStepOne_addText()
        {

            Text = new RichTextBox()
            {
                Dock = DockStyle.Fill, Multiline = true, AcceptsTab = true, AllowDrop = true,
                BorderStyle = BorderStyle.None
            };

            _notepadControlHolderPanel2.Controls.Add(Text);
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
            SpellChecker.Text = Text.Text;
            SpellChecker.SpellCheck();
        }

        private void SpellChecker_DoubledWord(object sender, SpellingEventArgs args)
        {
            // update text  
            Text.Text = SpellChecker.Text;
        }

        private void SpellChecker_EndOfText(object sender, EventArgs args)
        {
            // update text  
            Text.Text = SpellChecker.Text;
        }

        private void SpellChecker_MisspelledWord(object sender, SpellingEventArgs args)
        {
            // update text  
            Text.Text = SpellChecker.Text;
        }
        
        private void NormalMode_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.normal)
            {
                return;
            }
            _formState.Restore(_notepadFrame);
            _viewMode = ViewMode.normal;
        }

        private void DistractionFree_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.focused)
            {
                return;
            }
            _formState.Maximize(_notepadFrame);
            _viewMode = ViewMode.focused;
        }

        private void HackerContrast_Click(object sender, EventArgs e)
        {
            _brandController.SetHackerStyle();
        }

        private void RegularContrast_Click(object sender, EventArgs e)
        {
           _brandController.SetBaseStyle();
        }

        private void HighContrast_Click(object sender, EventArgs e)
        {
          _brandController.SetHighContrastStyle();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            _notepadFrame.Close();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI

            int index = 0;
            String temp = Text.Text;
            Text.Text = "";
            Text.Text = temp;

            var searchController=new SearchController();
           var searchTerm= searchController.ShowDialog();

           while (index < Text.Text.LastIndexOf(searchTerm))
           {
               Text.Find(searchTerm, index, Text.TextLength, RichTextBoxFinds.None);
               Text.SelectionBackColor = Color.Aqua;
               index = Text.Text.IndexOf(searchTerm, index) + 1;
           }
        }


        private void SelectAll_Click(object sender, EventArgs e)
        {
            Text.SelectAll();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
           Text.Paste();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (Text.SelectedText.Length > 0)
            {
                Text.Cut();
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (Text.SelectedText.Length > 0)
            {
                Text.Copy();
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var openFileDialog1=new OpenFileDialog();
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var fileName = openFileDialog1.FileName;
            openFile(fileName);

        }

        private void openFile(string fileName)
        {
            if (fileName == null)
            {
                return;
            }

            System.IO.StreamReader sr = new
                System.IO.StreamReader(fileName);

            Text.Text = sr.ReadToEnd();
            sr.Close();
            OpenDirectory(fileName);
        }

        private void OpenDirectory(string fileName)
        {
            var fileInfo=new FileInfo(fileName);
            var directoryName=fileInfo.DirectoryName;
            _fileBrowserController.PopulateLocal(directoryName);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog {Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Save a Text File"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                string data = Text.Text;
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
            Text.Text = null;
        }
   }
}