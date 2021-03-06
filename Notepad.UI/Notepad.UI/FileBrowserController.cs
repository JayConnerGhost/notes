﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using Notepad.TODO.Tests;

namespace Notepad.UI
{
    public class FileBrowserController
    {
        private readonly TabControl _tabContainer;
        private readonly ILoggingController _loggingController;
        private readonly SplitterPanel _container;
        public TreeView FolderView;
        public ListView FileView;
        public EventHandler OpenFile;
        public FileBrowserController(TabControl tabContainer, ILoggingController loggingController)
        {
            _tabContainer = tabContainer;
            _loggingController = loggingController;

            BuildFileBrowser();
            FolderView.NodeMouseClick += FolderViewNodeMouseClick;
            PopulateLocal(GetStartingDirectory());
            _loggingController.Log(MessageType.information, "FileBrowserController Constructed");
        }

        private string GetStartingDirectory()
        {
            var startingDirectory = string.Empty;
            var appSettingsReader = new System.Configuration.AppSettingsReader();
            startingDirectory = (string)appSettingsReader.GetValue("startingFolder", typeof(string));
            _loggingController.Log(MessageType.information, " Initial directory loaded from config");
            return startingDirectory;
        }

        private void FolderViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //work here to expand tree
            var newSelected = e.Node;
            FileView.Items.Clear();
            var directory = (DirectoryInfo)newSelected.Tag;
            try
            {
                ListViewItem item = null;
                ListViewItem.ListViewSubItem[] subItems;
                foreach (var dir in directory.GetDirectories())
                {
                    item = new ListViewItem(dir.Name, 1);
                    subItems = new ListViewItem.ListViewSubItem[]
                    {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                    };

                    item.SubItems.AddRange(subItems);
                    FileView.Items.Add(item);
                }

                foreach (var file in directory.GetFiles())
                {
                    item = new ListViewItem(file.Name, 0) {Tag = file.FullName, Name = file.Name};
                    subItems = new ListViewItem.ListViewSubItem[]
                    {
                    new ListViewItem.ListViewSubItem(item,"File"),
                    new ListViewItem.ListViewSubItem(item,
                        file.LastAccessTime.ToShortDateString())
                       };

                    item.SubItems.AddRange(subItems);
                    FileView.Items.Add(item);

                }
            }
            catch (System.UnauthorizedAccessException uae)
            {
                Console.WriteLine(uae.Message);
                _loggingController.Log(MessageType.Error, " UnathorizedAccessException - accessing file system");
            }
            FileView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        private void PopulateFolderView(string path)
        {
            var info = new DirectoryInfo(path);
            if (!info.Exists) return;
            var rootNode = new TreeNode(info.Name) {Tag = info};
            GetDirectories(info.GetDirectories(), rootNode);
            FolderView.Nodes.Add(rootNode);
            _loggingController.Log(MessageType.information, " Load folders ");
        }

        private void GetDirectories(DirectoryInfo[] getDirectories, TreeNode rootNode)
        {
            foreach (var subDir in getDirectories)
            {
                var aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                try
                {
                    var subSubDirs = subDir.GetDirectories();
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
            var fileBrowserTabPage = new TabPage {Text = "Files"};
            fileBrowserTabPage.Controls.Add(BuildOuterBrowser());
            _tabContainer.TabPages.Add(fileBrowserTabPage);
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
            FolderView = new TreeView { Dock = DockStyle.Fill };
            FileView = new ListView
            {
                Dock = DockStyle.Fill,
                GridLines = true,
                View = View.List
            };

            FileView.ItemSelectionChanged += _fileView_ItemSelectionChanged;
            outerContainer.Panel1.Controls.Add(FolderView);
            outerContainer.Panel2.Controls.Add(FileView);
            return outerContainer;
        }

        private void _fileView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                var openFile = OpenFile;
                openFile?.Invoke(sender, e);
            }
        }

        public void PopulateLocal(string path)
        {
            FolderView.Nodes.Clear();
            PopulateFolderView(path);
        }
    }
}