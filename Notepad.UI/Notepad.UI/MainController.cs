using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class MainController
    {
        private enum ViewMode
        {
            Normal,
            Focused
        }

        private readonly NotepadController _notepadController;
        private readonly FileBrowserController _fileBrowserController;
        private readonly BrandController _brandController;
        private readonly NotepadFrame _frame;
        private readonly FormState _formState = new FormState();
        private ViewMode _viewMode;

        public MainController(NotepadController notepadController, FileBrowserController fileBrowserController,
            BrandController brandController,
            NotepadFrame frame)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
            _fileBrowserController.OpenFile +=OpenFile;
            _brandController = brandController;
            _frame = frame;
            BuildUserInterface(frame);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var listViewItemSelectionChangedEventArgs = (ListViewItemSelectionChangedEventArgs)e;
            var listViewItem = listViewItemSelectionChangedEventArgs.Item;
            _notepadController.OpenFile((string)listViewItem.Tag,listViewItem.Name);
        }

        private void BuildUserInterface(NotepadFrame frame)
        {
            BuildMenu(frame.menuMain);
        }

        private void BuildMenu(MenuStrip frameMenuMain)
        {
            var mnuFile = new ToolStripMenuItem("File");
            var mnuEdit = new ToolStripMenuItem("Edit");
            var mnuView = new ToolStripMenuItem("View");
            var mnuTools = new ToolStripMenuItem("Tools");
            ComposeFileMenu(mnuFile);
            ComposeEditMenu(mnuEdit);
            ComposeViewMenu(mnuView);
            ComposeToolsMenu(mnuTools);

            frameMenuMain.Items.Add(mnuFile);
            frameMenuMain.Items.Add(mnuEdit);
            frameMenuMain.Items.Add(mnuView);
            frameMenuMain.Items.Add(mnuTools);
            frameMenuMain.Dock = DockStyle.Top;
        }

        private void ComposeToolsMenu(ToolStripMenuItem mnuTools)
        {
            mnuTools.DropDownItems.Add(new ToolStripMenuItem("Spell Check", null, new EventHandler(SpellCheck_Click),
                Keys.Alt | Keys.S));
        }

        private void SpellCheck_Click(object sender, EventArgs e)
        {
            _notepadController.SpellCheck();
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

        private void NormalMode_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.Normal)return;
            _formState.Restore(_frame);
            _viewMode = ViewMode.Normal;
        }

        private void DistractionFree_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.Focused) return;
            _formState.Maximize(_frame);
            _viewMode = ViewMode.Focused;
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

        private void Search_Click(object sender, EventArgs e)
        {
            _notepadController.SearchText();
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            _notepadController.SelectAllText();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            _notepadController.PasteText();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            _notepadController.CutText();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            _notepadController.CopyText();
        }

        private void ComposeFileMenu(ToolStripMenuItem mnuFile)
        {
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("New", null, new EventHandler(New_Click),
                Keys.Control | Keys.F));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Save", null, new EventHandler(Save_Click),
                Keys.Control | Keys.S));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Open", null, new EventHandler(Open_Click),
                Keys.Control | Keys.O));
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Close", null, new EventHandler(Close_Click),
                Keys.Control | Keys.X));
        }

        private void Close_Click(object sender, EventArgs e)
        {
           _frame.Close();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var fileName = openFileDialog1.FileName;
       
            if (fileName == null)return;
            var shortFileName = openFileDialog1.SafeFileName;
            _notepadController.OpenFile(fileName, shortFileName);
            var directory=new FileInfo(fileName).DirectoryName;
            _fileBrowserController.PopulateLocal(directory);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            var saveFileDialog1 = new SaveFileDialog { Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Save a Text File" };
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName == "") return;
            var fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
            var data = _notepadController.GetText();
            var info = new UTF8Encoding(true).GetBytes(data);
            fs.Write(info, 0, info.Length);
            var btData = new byte[] { 0x0 };
            fs.Write(btData, 0, btData.Length);

            fs.Close();
            fs = null;
            if (_notepadController.GetSelectedTabPageTag() != string.Empty) return;
            var name = (new FileInfo(saveFileDialog1.FileName)).Name;
            var fileName = name;
            _notepadController.UpdateMDITag(fileName);

        }

        private void New_Click(object sender, EventArgs e)
        {
            var mdiPage = _notepadController.AddMDIPage();
            _notepadController.SetSelectedMDIPage(mdiPage);
            //more work here to support opening a new tab 
            _notepadController.ClearText();
        }
    }
}
