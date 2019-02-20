using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace Notepad.UI
{
    public class FileBrowserController
    {
        private readonly SplitterPanel _container;
        private TreeView _folderView;
        private ListView _fileView;
        public EventHandler OpenFile;
        public FileBrowserController(SplitterPanel container)
        {
            _container = container;
            BuildFileBrowser();
            _folderView.NodeMouseClick += FolderViewNodeMouseClick;
            PopulateLocal(GetStartingDirectory());
        }

        private string GetStartingDirectory()
        {
            var startingDirectory = string.Empty;
            var appSettingsReader = new System.Configuration.AppSettingsReader();
            startingDirectory = (string)appSettingsReader.GetValue("startingFolder", typeof(string));
            return startingDirectory;
        }

        private void FolderViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //work here to expand tree
            TreeNode newSelected = e.Node;
            _fileView.Items.Clear();
            DirectoryInfo directory = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            try
            {


                foreach (var dir in directory.GetDirectories())
                {
                    item = new ListViewItem(dir.Name, 0);
                    subItems = new ListViewItem.ListViewSubItem[]
                    {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                    };

                    item.SubItems.AddRange(subItems);
                    _fileView.Items.Add(item);
                }

                foreach (FileInfo file in directory.GetFiles())
                {
                    item = new ListViewItem(file.Name, 1);
                    item.Tag = file.FullName;
                    subItems = new ListViewItem.ListViewSubItem[]
                    {
                    new ListViewItem.ListViewSubItem(item,"File"),
                    new ListViewItem.ListViewSubItem(item,
                        file.LastAccessTime.ToShortDateString())
                       };

                    item.SubItems.AddRange(subItems);
                    _fileView.Items.Add(item);

                }
            }
            catch (System.UnauthorizedAccessException uae)
            {
                Console.WriteLine(uae.Message);
            }
            _fileView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        private void PopulateFolderView(string path)
        {
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo(path);
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                _folderView.Nodes.Add(rootNode);

            }
        }

        private void GetDirectories(DirectoryInfo[] getDirectories, TreeNode rootNode)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in getDirectories)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                try
                {
                    subSubDirs = subDir.GetDirectories();
                    if (subSubDirs.Length != 0)
                    {
                        GetDirectories(subSubDirs, aNode);
                    }
                }
                catch (System.UnauthorizedAccessException uae)
                {
                    Console.WriteLine(uae.Message);
                }
                rootNode.Nodes.Add(aNode);
            }
        }

        private void BuildFileBrowser()
        {
            this._container.Controls.Add(BuildOuterBrowser());
        }

        public SplitContainer BuildOuterBrowser()
        {
            var outerContainer = new SplitContainer
            {
                Orientation = Orientation.Horizontal,
                Dock = DockStyle.Fill,
                SplitterDistance = 100,
                SplitterWidth = 6

            };
            outerContainer.Panel1.Name = "folderView";
            outerContainer.Panel2.Name = "fileView";
            _folderView = new TreeView { Dock = DockStyle.Fill };
            _fileView = new ListView
            {
                Dock = DockStyle.Fill,
                GridLines = true,
                View = View.List
            };

            _fileView.ItemSelectionChanged += _fileView_ItemSelectionChanged;
            outerContainer.Panel1.Controls.Add(_folderView);
            outerContainer.Panel2.Controls.Add(_fileView);
            return outerContainer;
        }

        private void _fileView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var openFile =  OpenFile;
            openFile?.Invoke(sender, e);
        }

        public void PopulateLocal(string path)
        {
            _folderView.Nodes.Clear();
            PopulateFolderView(path);
        }
    }
}