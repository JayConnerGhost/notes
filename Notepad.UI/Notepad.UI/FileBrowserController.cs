using System;
using System.IO;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class FileBrowserController
    {
        private readonly SplitterPanel _container;
        private TreeView _treeView;
        public FileBrowserController(SplitterPanel container)
        {
            _container = container;
            BuildFileBrowser();
            PopulateLocal(@"C:\");
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            // https://stackoverflow.com/questions/6239544/populate-treeview-with-file-system-directory-structure
            var directoryNode = new TreeNode(directoryInfo.Name);
            try
            {
               foreach (var directory in directoryInfo.GetDirectories())
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                e = null;
            }
            return directoryNode;
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }
        private void BuildFileBrowser()
        {
            _treeView = new TreeView {Dock = DockStyle.Fill};
            this._container.Controls.Add(_treeView);
        }

        public void PopulateLocal(string path)
        {
            _treeView.Nodes.Clear();
            ListDirectory(_treeView, path);
        }
    }
}