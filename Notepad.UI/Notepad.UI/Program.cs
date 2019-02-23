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
            var fileBrowserController= new FileBrowserController(notepadFrame.splitControlArea.Panel1);
            var notepadController = new NotepadController(notepadFrame.splitControlArea.Panel2);
            var brandController =new BrandController(notepadController,fileBrowserController);
            var mainController=new MainController(notepadController, fileBrowserController,brandController, notepadFrame);
            
            notepadFrame.Controller = mainController;

            Application.Run(notepadFrame);
        }
    }
}
