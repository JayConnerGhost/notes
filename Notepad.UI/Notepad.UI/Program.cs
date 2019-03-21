using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
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
            var notepadController = new NotepadController(notepadFrame.splitControlArea.Panel2);
            var fileBrowserController= new FileBrowserController((TabControl)notepadFrame.splitControlArea.Panel1.Controls[0]);
            var sqlLiteDbAdapter = new SqlLiteDbAdapter(GetConnectionString(),GetDatabaseName());
            SetupDatabase(sqlLiteDbAdapter);
            var ideaRepository = new IdeaRepository(sqlLiteDbAdapter);
            var ideaService = new IdeaService(ideaRepository);
            var ideaController = new IdeaController((TabControl) notepadFrame.splitControlArea.Panel1.Controls[0],
                ideaService);
            var brandController =new BrandController(notepadController,fileBrowserController);
            var mainController=new MainController(notepadController, fileBrowserController,brandController, notepadFrame, ideaController);
            notepadFrame.Controller = mainController;

            Application.Run(notepadFrame);
        }

        private static void SetupDatabase(SqlLiteDbAdapter sqlLiteDbAdapter)
        {
            sqlLiteDbAdapter.CreateDatabase(false);
            sqlLiteDbAdapter.CreateIdeaTable();
           LoadDevelopmentData(sqlLiteDbAdapter);
        }

        private static void LoadDevelopmentData(SqlLiteDbAdapter sqlLiteDbAdapter)
        {
            // to be removed when no longer needed 
            sqlLiteDbAdapter.CreateIdea("test idea 1");
            sqlLiteDbAdapter.CreateIdea("test idea 2");
            sqlLiteDbAdapter.CreateIdea("test idea 3");
            sqlLiteDbAdapter.CreateIdea("test idea 4");
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
