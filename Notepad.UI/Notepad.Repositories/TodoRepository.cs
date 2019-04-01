using System;
using Notepad.Adapters;

namespace Notepad.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IToDoDataAdapter _sqliteDbTodoAdapter;

        public TodoRepository(IToDoDataAdapter sqliteDbTodoAdapter)
        {
            _sqliteDbTodoAdapter = sqliteDbTodoAdapter;
        }

        public int Create(string name, string description)
        {
           return _sqliteDbTodoAdapter.CreateToDoItem(name, description);
        }
    }
}