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

        internal RichTextBox Text { get; private set; }

        public NotepadController(SplitterPanel notePadPanel)
        {
            _notePadPanel = notePadPanel;
            AddTextControl();
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

        private void AddTextControl()
        {
            Text = new RichTextBox()
            {
                Dock = DockStyle.Fill, Multiline = true, AcceptsTab = true, AllowDrop = true,
                BorderStyle = BorderStyle.None
            };

            _notePadPanel.Controls.Add(Text);
            AddContextMenu();
            AddSpellingSupport();
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
            if (fileName == null)return;

            var sr = new StreamReader(fileName);

            Text.Text = sr.ReadToEnd();
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
            if (Text.SelectedText.Length > 0)
            {
                Text.Copy();
            }
        }

        public void CutText()
        {
            if (Text.SelectedText.Length > 0)
            {
                Text.Cut();
            }
        }

        public void PasteText()
        {
            Text.Paste();
        }

        public void SelectAllText()
        {
            Text.SelectAll();
        }

        public void SearchText()
        {
            //https://www.youtube.com/watch?v=kfbkLxH8xDI

            var index = 0;
            var temp = Text.Text;
            Text.Text = "";
            Text.Text = temp;

            var searchController = new SearchController();
            var searchTerm = searchController.ShowDialog();

            while (index < Text.Text.LastIndexOf(searchTerm))
            {
                Text.Find(searchTerm, index, Text.TextLength, RichTextBoxFinds.None);
                Text.SelectionBackColor = Color.Aqua;
                index = Text.Text.IndexOf(searchTerm, index) + 1;
            }
        }

        public void SpellCheck()
        {
            SpellChecker.Text = Text.Text;
            SpellChecker.SpellCheck();
        }
    }
}