using System.Windows.Forms;

namespace Notepad.UI
{
    public class FileInformationModel
    {
        public TabPage Page { get; }
        public string FileName { get; }
        public string Tag { get; }

        public FileInformationModel(TabPage page, string fileName, string tag)
        {
            Page = page;
            FileName = fileName;
            Tag = tag;
        }
    }
}