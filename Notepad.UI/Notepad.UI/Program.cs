using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.CompilerServices;
using Notepad.Adapters;
using Notepad.Repositories;
using Notepad.Services;

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
            var loggingController = SetupLoggingController(notepadFrame);
            var notepadController = new NotepadController(notepadFrame.splitControlArea.Panel2, loggingController);
            var fileBrowserController= new FileBrowserController((TabControl)notepadFrame.splitControlArea.Panel1.Controls[0],loggingController);
            var sqlLiteDbAdapter = new SqlLiteDbAdapter(GetConnectionString(),GetDatabaseName());
            SetupDatabase(sqlLiteDbAdapter);
            var ideaController = SetupIdeaController(sqlLiteDbAdapter, notepadFrame, loggingController);
            var brandController = SetupBrandController(notepadController, fileBrowserController, ideaController, loggingController, notepadFrame);
            SetupMainController(notepadController, fileBrowserController, brandController, notepadFrame, ideaController, loggingController);

            Application.Run(notepadFrame);
        }

        private static void SetupMainController(NotepadController notepadController,
            FileBrowserController fileBrowserController, BrandController brandController, NotepadFrame notepadFrame,
            IdeaController ideaController, LoggingController loggingController)
        {
            var mainController = new MainController(notepadController, fileBrowserController, brandController, notepadFrame,
                ideaController, loggingController);
            notepadFrame.Controller = mainController;
        }

        private static BrandController SetupBrandController(NotepadController notepadController,
            FileBrowserController fileBrowserController, IdeaController ideaController, LoggingController loggingController, Form frame)
        {
            var brandController =
                new BrandController(notepadController, fileBrowserController, ideaController, loggingController, frame);
            notepadController.BrandController = brandController;
            return brandController;
        }

        private static IdeaController SetupIdeaController(SqlLiteDbAdapter sqlLiteDbAdapter, NotepadFrame notepadFrame,
            LoggingController loggingController)
        {
            var ideaRepository = new IdeaRepository(sqlLiteDbAdapter);
            var ideaService = new IdeaService(ideaRepository);
            var ideaController = new IdeaController((TabControl) notepadFrame.splitControlArea.Panel1.Controls[0],
                ideaService, loggingController);
            return ideaController;
        }

        private static LoggingController SetupLoggingController(NotepadFrame notepadFrame)
        {
            var bottomTabs = notepadFrame.scOuter.Panel2.Controls[0];
            var logTabPage = bottomTabs.Controls[0];
            var loggingController = new LoggingController((TabPage) logTabPage);
            return loggingController;
        }

        private static void SetupDatabase(SqlLiteDbAdapter sqlLiteDbAdapter)
        {
            sqlLiteDbAdapter.CreateDatabase(false);
            sqlLiteDbAdapter.CreateIdeaTable();
        }

       

        private static string GetDatabaseName()
        {
            return (string) ConfigurationManager.AppSettings["DatabaseName"];
        }

        private static string GetConnectionString()
        {
            return (string) ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}
