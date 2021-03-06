﻿using System;
using System.Collections.Generic;
using Notepad.Adapters;
using Notepad.Dtos;
using Notepad.UI;

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
           return _sqliteDbTodoAdapter.CreateToDoItem(name, description, PositionNames.Todo);
        }

        public IList<TodoItem> GetAll()
        {
            return _sqliteDbTodoAdapter.GetAll();
        }

        public void Delete(int id)
        {
            _sqliteDbTodoAdapter.Delete(id);
        }

        public void Update(int id, PositionNames position, string description, string name)
        {
            _sqliteDbTodoAdapter.Update(id, position, description, name);
        }
    }
}