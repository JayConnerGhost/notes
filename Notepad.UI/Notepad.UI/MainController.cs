using System.Windows.Forms;

namespace Notepad.UI
{
    public class MainController
    {
        private readonly NotepadController _notepadController;
        private readonly FileBrowserController _fileBrowserController;

        public MainController(NotepadController notepadController, FileBrowserController fileBrowserController)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
        }
    }
}