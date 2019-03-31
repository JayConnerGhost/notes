using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSpell.SpellChecker;
using Notepad.UI;

namespace Notepad.UI
{
    public class NotepadController
    {
        private readonly Dictionary<int,FileInformationModel>_fileRegister=new Dictionary<int, FileInformationModel>();
        private readonly SplitterPanel _notePadPanel;
        private readonly LoggingController _loggingController;
        public EventHandler DirectoryChanged;
        private readonly FormState _formState = new FormState();
        internal Spelling SpellChecker;
        public TabControl MdiInterface;
        internal RichTextBox Text { get; private set; }
        public BrandController BrandController { private get; set; }

        
        public NotepadController(SplitterPanel notePadPanel, LoggingController loggingController)
        {
            _notePadPanel = notePadPanel;
            _loggingController = loggingController;
            AddMDISupport();
            _loggingController.Log(MessageType.information,"NotePadController Constructed");
        }

        private void AddMDISupport()
        {
            MdiInterface = new TabControl {Dock = DockStyle.Fill};
            _notePadPanel.Controls.Add(MdiInterface);
            AddMDIPage();
            _loggingController.Log(MessageType.information, " Build interface ");
        }

        public TabPage AddMDIPage()
        {
            var tmpDocumentMarker = ConfigurationManager.AppSettings["tmpFileMarker"];
            var tabPage = new TabPage { Dock = DockStyle.Fill };
            tabPage.Controls.Add(AddTextControl());
            MdiInterface.TabPages.Add(tabPage);
            var fileName = $"{tmpDocumentMarker}{tabPage.TabIndex}";
            AddToFileRegister(tabPage,fileName, fileName);
            tabPage.Tag = fileName;
            tabPage.Text = fileName;
           _loggingController.Log(MessageType.information, " Add Page");
           return tabPage;
        }

        private RichTextBox GetSelectedTextControl()
        {
          return (RichTextBox) MdiInterface.SelectedTab.Controls[0];
        }

        private void AddSpellingSupport()
        {
            //http://www.loresoft.com/The-NetSpell-project
            SpellChecker = new Spelling {ShowDialog = true};
            SpellChecker.MisspelledWord += new Spelling.MisspelledWordEventHandler(SpellChecker_MisspelledWord);
            SpellChecker.EndOfText += new Spelling.EndOfTextEventHandler(SpellChecker_EndOfText);
            SpellChecker.DoubledWord += new Spelling.DoubledWordEventHandler(SpellChecker_DoubledWord);
            _loggingController.Log(MessageType.information, " Build spellchecker");
        }

        
        private void AddContextMenu()
        {
            var contextMenu=new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Cut", Cut_Click));
            contextMenu.MenuItems.Add(new MenuItem("Copy", Copy_Click));
            contextMenu.MenuItems.Add(new MenuItem("Paste", Paste_Click));
            Text.ContextMenu = contextMenu;
            _loggingController.Log(MessageType.information, " Build context menu");
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            PasteText();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            CopyText();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            CutText();
        }

        private RichTextBox AddTextControl()
        {
            Text = new RichTextBox()
            {
                Dock = DockStyle.Fill, Multiline = true, AcceptsTab = true, AllowDrop = true,
                BorderStyle = BorderStyle.None
            };
            AddDragAndDropSupport(Text);
            AddContextMenu();
            AddSpellingSupport();
            return Text;
        }

        private void AddDragAndDropSupport(RichTextBox text)
        {
            text.DragEnter += new DragEventHandler(RTF_DragEnter);
            text.DragDrop += new DragEventHandler(RTF_DragDrop);
        }

        private void RTF_DragDrop(object sender, DragEventArgs e)
        {
            Image img = default(Image);
            img = Image.FromFile(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
            Clipboard.SetImage(img);

            var richTextBox = (RichTextBox)sender;
            richTextBox.SelectionStart = 0;
            richTextBox.Paste();
        }

        private void RTF_DragEnter(object sender, DragEventArgs e)
        {

            if ((e.Data.GetDataPresent(DataFormats.FileDrop)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void SpellChecker_DoubledWord(object sender, SpellingEventArgs args)
        {
            // update text  
            GetSelectedTextControl().Text = SpellChecker.Text;
        }

        private void SpellChecker_EndOfText(object sender, EventArgs args)
        {
            // update text  
            GetSelectedTextControl().Text = SpellChecker.Text;
        }

        private void SpellChecker_MisspelledWord(object sender, SpellingEventArgs args)
         {
            // update text  
            GetSelectedTextControl().Text = SpellChecker.Text;
        }

        public void OpenFile(string fileName, string tag)
        {
            if (fileName == null) return;
            if (IsFileAlreadyOpen(fileName)) return;

            //var page = MdiInterface.TabPages[0].Text == string.Empty ? MdiInterface.TabPages[0] : AddMDIPage();
            var page = AddMDIPage();
            page.Text = tag;
            MdiInterface.SelectedTab = page;
            var target = (RichTextBox) page.Controls[0];
           
            try
            {
                target.LoadFile(fileName, RichTextBoxStreamType.RichText);
            }
            catch (Exception e)
            {
                target.LoadFile(fileName, RichTextBoxStreamType.PlainText);
            }

            BrandTarget(target);
            AddToFileRegister(page, fileName, tag);
        }

        private bool IsFileAlreadyOpen(string fileName)
        {

            foreach (var fileInformationModel in _fileRegister)
            {
                if (fileInformationModel.Value.FileName == fileName)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddToFileRegister(TabPage page, string fileName, string tag)
        {

            try
            {
                var fileInformationModel = new FileInformationModel(page, fileName, tag);
                _fileRegister.Add(page.TabIndex, fileInformationModel);
            }
            catch (System.ArgumentException ae)
            {
                _loggingController.Log(MessageType.Error,ae.Message);
            }
         
        }

        public void RemoveFileFromFileRegister(int tabIndex)
        {
            _fileRegister.Remove(tabIndex);
        }

        private void BrandTarget(RichTextBox target)
        {
            BrandController.BrandTextArea(target);
        }

        public void ClearText()
        {
            GetSelectedTextControl().Text = null;
        }

        public string GetText()
        {
            return GetSelectedTextControl().Text;
        }

        public void CopyText()
        {
            if (GetSelectedTextControl().SelectedText.Length > 0)
            {
                GetSelectedTextControl().Copy();
            }
        }

        public void CutText()
        {
            if (GetSelectedTextControl().SelectedText.Length > 0)
            {
                GetSelectedTextControl().Cut();
            }
        }

        public void PasteText()
        {
            GetSelectedTextControl().Paste();
        }

        public void SelectAllText()
        {
            GetSelectedTextControl().SelectAll();
        }

        public void SearchText()
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI
            _loggingController.Log(MessageType.information, " load search tool");
            var index = 0;
            var temp = GetSelectedTextControl().Text;
            GetSelectedTextControl().Text = "";
            GetSelectedTextControl().Text = temp;

            var searchController = new SearchController();
            var searchTerm = searchController.ShowDialog();

            while (index < GetSelectedTextControl().Text.LastIndexOf(searchTerm))
            {
                GetSelectedTextControl().Find(searchTerm, index, GetSelectedTextControl().TextLength, RichTextBoxFinds.None);
                GetSelectedTextControl().SelectionBackColor = Color.Aqua;
                index = GetSelectedTextControl().Text.IndexOf(searchTerm, index) + 1;
            }
        }

        public void SpellCheck()
        {
            _loggingController.Log(MessageType.information, " load spell check");
            SpellChecker.Text = GetSelectedTextControl().Text;
            SpellChecker.SpellCheck();
        }

        public void SetSelectedMDIPage(TabPage mdiPage)
        {
            MdiInterface.SelectedTab = mdiPage;
        }

        public string GetSelectedTabPageTag()
        {
            return MdiInterface.SelectedTab.Text;
        }

        public void UpdateMDITag(string fileName)
        {
            MdiInterface.SelectedTab.Text = fileName;
        }

        public void SetForeColor(Color color)
        {
            foreach (var mdiInterfaceTabPage in MdiInterface.TabPages)
            {
                var tabControl=(TabPage)mdiInterfaceTabPage;
                tabControl.Controls[0].ForeColor = color;
            }
        }

        public void SetBackColor(Color color)
        {
            foreach (var mdiInterfaceTabPage in MdiInterface.TabPages)
            {
                var tabControl = (TabPage)mdiInterfaceTabPage;
                tabControl.Controls[0].BackColor = color;
            }
        }

        public void SetFont(Font font)
        {
            foreach (var mdiInterfaceTabPage in MdiInterface.TabPages)
            {
                var tabControl = (TabPage)mdiInterfaceTabPage;
                tabControl.Controls[0].Font = font;
            }
        }

        public string GetFileName()
        {
            var selectedTabPageIndex = GetSelectedTabPageIndex();
            var fileInformationModel = _fileRegister[selectedTabPageIndex];
            return fileInformationModel.FileName;
        }

        private int GetSelectedTabPageIndex()
        {
            var selectedTabTabIndex = MdiInterface.SelectedTab.TabIndex;
            return selectedTabTabIndex;
        }

        public RichTextBox GetRTFControl()
        {
            return GetSelectedTextControl();
        }

        public void OpenFileInSelectedTab(string fileName, string name)
        {
            var targetTab = (TabPage) MdiInterface.SelectedTab;
            var target = (RichTextBox)targetTab.Controls[0];
            targetTab.Text = name;
            try
            {
                target.LoadFile(fileName, RichTextBoxStreamType.RichText);
            }
            catch (Exception e)
            {
                target.LoadFile(fileName, RichTextBoxStreamType.PlainText);
            }

            BrandTarget(target);
            AddToFileRegister(targetTab,fileName, name);


        }
    }
}