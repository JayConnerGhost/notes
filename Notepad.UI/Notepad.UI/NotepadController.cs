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
        private readonly SplitterPanel _notePadPanel;
        public EventHandler DirectoryChanged;
        private readonly FormState _formState = new FormState();
        internal Spelling SpellChecker;
        public TabControl MdiInterface;
        internal RichTextBox Text { get; private set; }

        public NotepadController(SplitterPanel notePadPanel)
        {
            _notePadPanel = notePadPanel;
            AddMDISupport();
            
        }

        private void AddMDISupport()
        {
            MdiInterface=new TabControl();
            MdiInterface.Dock = DockStyle.Fill;
            var tabPage = new TabPage {Dock = DockStyle.Fill};
            tabPage.Controls.Add(AddTextControl());
            MdiInterface.TabPages.Add(tabPage);
            _notePadPanel.Controls.Add(MdiInterface);
        }

        private TabPage AddMDIPage()
        {
            var tabPage = new TabPage { Dock = DockStyle.Fill };
            tabPage.Controls.Add(AddTextControl());
            MdiInterface.TabPages.Add(tabPage);
            _notePadPanel.Controls.Add(MdiInterface);
            return tabPage;
        }

        private RichTextBox GetSelectedText()
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
        }

        
        private void AddContextMenu()
        {
            var contextMenu=new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Cut", Cut_Click));
            contextMenu.MenuItems.Add(new MenuItem("Copy", Copy_Click));
            contextMenu.MenuItems.Add(new MenuItem("Paste", Paste_Click));
            Text.ContextMenu = contextMenu;
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

            AddContextMenu();
            AddSpellingSupport();
            return Text;
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

        public void OpenFile(string fileName)
        {
            if (fileName == null) return;

            var sr = new StreamReader(fileName);
            var page = AddMDIPage();
            MdiInterface.SelectedTab = page;
            var target = (RichTextBox) page.Controls[0];
            target.Text = sr.ReadToEnd();
            sr.Close();
        }

        public void ClearText()
        {
            Text.Text = null;
        }

        public string GetText()
        {
            return Text.Text;
        }

        public void CopyText()
        {
            if (GetSelectedText().SelectedText.Length > 0)
            {
                GetSelectedText().Copy();
            }
        }

        public void CutText()
        {
            if (GetSelectedText().SelectedText.Length > 0)
            {
                GetSelectedText().Cut();
            }
        }

        public void PasteText()
        {
            GetSelectedText().Paste();
        }

        public void SelectAllText()
        {
            GetSelectedText().SelectAll();
        }

        public void SearchText()
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI

            var index = 0;
            var temp = GetSelectedText().Text;
            GetSelectedText().Text = "";
            GetSelectedText().Text = temp;

            var searchController = new SearchController();
            var searchTerm = searchController.ShowDialog();

            while (index < GetSelectedText().Text.LastIndexOf(searchTerm))
            {
                GetSelectedText().Find(searchTerm, index, GetSelectedText().TextLength, RichTextBoxFinds.None);
                GetSelectedText().SelectionBackColor = Color.Aqua;
                index = GetSelectedText().Text.IndexOf(searchTerm, index) + 1;
            }
        }

        public void SpellCheck()
        {
            SpellChecker.Text = Text.Text;
            SpellChecker.SpellCheck();
        }
    }
}