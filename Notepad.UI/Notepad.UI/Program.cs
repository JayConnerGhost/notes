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
            var todoFrame = new TodoFrame();
            ILoggingController loggingController = SetupLoggingController(notepadFrame);
            var notepadController = new NotepadController(notepadFrame.splitControlArea.Panel2, loggingController);
            var fileBrowserController= new FileBrowserController((TabControl)notepadFrame.splitControlArea.Panel1.Controls[0],loggingController);
            var sqlLiteDbAdapter = new SqlLiteDbIdeaAdapter(GetConnectionString(),GetDatabaseName());
            var sqliteDbTodoAdapter = new SqliteDbTodoAdapter(GetConnectionString(),GetDatabaseName());
            SetupDatabase(sqlLiteDbAdapter, sqliteDbTodoAdapter);
            var ideaController = SetupIdeaController(sqlLiteDbAdapter, notepadFrame, loggingController);
            var brandController = SetupBrandController(notepadController, fileBrowserController, ideaController, loggingController, notepadFrame);
            var todoRepository = new TodoRepository(sqliteDbTodoAdapter);
            var todoController = new TodoController(loggingController, new TodoService(todoRepository),todoFrame);
            SetupMainController(notepadController, fileBrowserController, brandController, notepadFrame, ideaController, loggingController, todoController);

            Application.Run(notepadFrame);
        }

        private static void SetupMainController(NotepadController notepadController,
            FileBrowserController fileBrowserController, BrandController brandController, NotepadFrame notepadFrame,
            IdeaController ideaController, ILoggingController loggingController, ITodoController todoController)
        {
            var mainController = new MainController(notepadController, fileBrowserController, brandController, notepadFrame,
                ideaController,todoController, loggingController);
            notepadFrame.Controller = mainController;
        }

        private static BrandController SetupBrandController(NotepadController notepadController,
            FileBrowserController fileBrowserController, IdeaController ideaController, ILoggingController loggingController, Form frame)
        {
            var brandController =
                new BrandController(notepadController, fileBrowserController, ideaController, loggingController, frame);
            notepadController.BrandController = brandController;
            return brandController;
        }

        private static IdeaController SetupIdeaController(SqlLiteDbIdeaAdapter sqlLiteDbIdeaAdapter, NotepadFrame notepadFrame,
            ILoggingController loggingController)
        {
            var ideaRepository = new IdeaRepository(sqlLiteDbIdeaAdapter);
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

        private static void SetupDatabase(SqlLiteDbIdeaAdapter sqlLiteDbIdeaAdapter,
            SqliteDbTodoAdapter sqliteDbTodoAdapter)
        {
            sqlLiteDbIdeaAdapter.CreateDatabase(false);
            sqlLiteDbIdeaAdapter.CreateIdeaTable();
            sqliteDbTodoAdapter.CreateTodoTable();
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
