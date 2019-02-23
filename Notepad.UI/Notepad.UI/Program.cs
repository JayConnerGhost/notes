using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var notepadFrame = new NotepadFrame();
            var notepadController = new NotepadController(notepadFrame);
            var fileBrowserController= new FileBrowserController(notepadFrame.splitControlArea.Panel2);
            var mainController=new MainController(notepadController, fileBrowserController);
            Application.Run(notepadFrame);
        }
    }
}
