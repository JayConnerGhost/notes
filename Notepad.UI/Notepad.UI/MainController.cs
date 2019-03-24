﻿using System;
using System.Drawing;
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
        private readonly IdeaController _ideaController;
        private readonly FormState _formState = new FormState();
        private ViewMode _viewMode;
        private IdeaController _IdeaController;
        private TabControl _Area1Tabs;

        // The size of the X in each tab's upper right corner.
        private int Xwid = 8;
        private const int tab_margin = 3;

        public MainController(NotepadController notepadController,
            FileBrowserController fileBrowserController,
            BrandController brandController,
            NotepadFrame frame, IdeaController ideaController)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
            _fileBrowserController.OpenFile +=OpenFile;
            _brandController = brandController;
            _frame = frame;
            _ideaController = ideaController;
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
            BuildUIArea1(frame.splitControlArea.Panel1);
            BuildUIArea2(frame.splitControlArea.Panel2);
        }

        private void BuildUIArea2(SplitterPanel area)
        {
            //TODO: configure Tabs
            SetUpArea2TabControls((TabControl) area.Controls[0]);
        }

        private void SetUpArea2TabControls(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.DrawItem+=TabControlOnDrawItem;
            tabControl.MouseDown+=TabControlOnMouseDown;

            Size tabSize = tabControl.ItemSize;
            tabSize.Width = 100;
            tabSize.Height += 6;
            tabControl.ItemSize = tabSize;
        }

        private void TabControlOnMouseDown(object sender, MouseEventArgs e)
        {
            //http://csharphelper.com/blog/2014/11/make-an-owner-drawn-tabcontrol-in-c/
            // See if this is over a tab.

            var tabMenu = (TabControl)_frame.splitControlArea.Panel2.Controls[0];
            if (tabMenu.TabPages.Count ==1) return;

            for (int i = 0; i < tabMenu.TabPages.Count; i++)
            {
                // Get the TabRect plus room for margins.
                Rectangle tab_rect = tabMenu.GetTabRect(i);
                RectangleF rect = new RectangleF(
                    tab_rect.Left + tab_margin,
                    tab_rect.Y + tab_margin,
                    tab_rect.Width - 2 * tab_margin,
                    tab_rect.Height - 2 * tab_margin);
                if (e.X >= rect.Right - Xwid &&
                    e.X <= rect.Right &&
                    e.Y >= rect.Top &&
                    e.Y <= rect.Top + Xwid)
                {
                    Console.WriteLine("Tab " + i);
                    tabMenu.TabPages.RemoveAt(i);
                   _notepadController.RemoveFileFromFileRegister(i);
                    return;
                }
            }
        }

        private void TabControlOnDrawItem(object sender, DrawItemEventArgs e)
        {
            //http://csharphelper.com/blog/2014/11/make-an-owner-drawn-tabcontrol-in-c/
            Brush txt_brush, box_brush;
            Pen box_pen;
            var tabMenu = (TabControl)_frame.splitControlArea.Panel2.Controls[0];

            // We draw in the TabRect rather than on e.Bounds
            // so we can use TabRect later in MouseDown.
            Rectangle tab_rect = tabMenu.GetTabRect(e.Index);

            // Draw the background.
            // Pick appropriate pens and brushes.
            if (e.State == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, tab_rect);
                e.DrawFocusRectangle();

                txt_brush = Brushes.Black;
                box_brush = Brushes.Black;
                box_pen = Pens.White;
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Gray, tab_rect);

                txt_brush = Brushes.Black;
                box_brush = Brushes.Black;
                box_pen = Pens.Lavender;
            }

            // Allow room for margins.
            RectangleF layout_rect = new RectangleF(
                tab_rect.Left + tab_margin,
                tab_rect.Y + tab_margin,
                tab_rect.Width - 2 * tab_margin,
                tab_rect.Height - 2 * tab_margin);
            using (StringFormat string_format = new StringFormat())
            {
                // Draw the tab # in the upper left corner.
                using (Font small_font = new Font(_frame.Font.FontFamily,
                    6, FontStyle.Bold))
                {
                    string_format.Alignment = StringAlignment.Near;
                    string_format.LineAlignment = StringAlignment.Near;
                    e.Graphics.DrawString(
                        e.Index.ToString(),
                        small_font,
                        txt_brush,
                        layout_rect,
                        string_format);
                }

                // Draw the tab's text centered.
                using (Font big_font = new Font(_frame.Font, FontStyle.Bold))
                {
                    string_format.Alignment = StringAlignment.Center;
                    string_format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(
                        tabMenu.TabPages[e.Index].Text,
                        big_font,
                        txt_brush,
                        layout_rect,
                        string_format);
                }

                // Draw an X in the upper right corner.
                Rectangle rect = tabMenu.GetTabRect(e.Index);
                e.Graphics.FillRectangle(box_brush,
                    layout_rect.Right - Xwid,
                    layout_rect.Top,
                    Xwid,
                    Xwid);
                e.Graphics.DrawRectangle(box_pen,
                    layout_rect.Right - Xwid,
                    layout_rect.Top,
                    Xwid,
                    Xwid);
                e.Graphics.DrawLine(box_pen,
                    layout_rect.Right - Xwid,
                    layout_rect.Top,
                    layout_rect.Right,
                    layout_rect.Top + Xwid);
                e.Graphics.DrawLine(box_pen,
                    layout_rect.Right - Xwid,
                    layout_rect.Top + Xwid,
                    layout_rect.Right,
                    layout_rect.Top);
            }
        }


        private void BuildUIArea1(SplitterPanel area)
        {
            //build vertical tab container
            area.Controls.Add(_Area1Tabs);
            _Area1Tabs = (TabControl) area.Controls[0];
            _Area1Tabs.SizeMode = TabSizeMode.Fixed;
            _Area1Tabs.Height = 40;
            _Area1Tabs.Width = 10;
            _Area1Tabs.Appearance = TabAppearance.Normal;

            //Add Expose Tabs to external interfaces
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
            mnuFile.DropDownItems.Add(new ToolStripMenuItem("Save As", null, new EventHandler(SaveAs_Click),
                Keys.Alt | Keys.A));
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

        private void SaveAs_Click(object sender, EventArgs e)
        {
            UserInteractiveFileSave(true);
        }

        private void SaveFile()
        {
            //if file exists - then save silently 
            var fileName = _notepadController.GetFileName();
            if (fileName != string.Empty)
            {
                SilentSaveFile(fileName);
                return;
            }

            UserInteractiveFileSave(false);
        }

        private void UserInteractiveFileSave(bool OpenFile)
        {
           
            var saveFileDialog1 = new SaveFileDialog
                {Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Save a Text File"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName == "") return;

            var fs = (System.IO.FileStream) saveFileDialog1.OpenFile();
            var data = _notepadController.GetText();
            var info = new UTF8Encoding(true).GetBytes(data);
            fs.Write(info, 0, info.Length);
            var btData = new byte[] {0x0};
            fs.Write(btData, 0, btData.Length);

            fs.Close();
            fs = null;

            OpenNewlySavedFile(OpenFile, saveFileDialog1);

            if (_notepadController.GetSelectedTabPageTag() != string.Empty) return;
            UpdateMDITag(saveFileDialog1);
         
          
        }

        private void OpenNewlySavedFile(bool OpenFile, SaveFileDialog saveFileDialog1)
        {
            if (OpenFile)
            {
                var name = (new FileInfo(saveFileDialog1.FileName)).Name;
                _notepadController.OpenFile(saveFileDialog1.FileName, name);
            }
        }

        private void SilentSaveFile(string fileName)
        {
            //TODO - saving a file using the filename 
            var data = _notepadController.GetText();
            System.IO.StreamWriter file =new StreamWriter(fileName);
            file.Write(data);
            file.Close();
        }

        private void UpdateMDITag(SaveFileDialog saveFileDialog)
        {
            var name = (new FileInfo(saveFileDialog.FileName)).Name;
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
