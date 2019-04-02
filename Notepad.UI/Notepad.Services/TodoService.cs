using System;
using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.Repositories;
using Notepad.UI;

namespace Notepad.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public int Create(TodoItem todo)
        {
           return _repository.Create(todo.Name, todo.Description);
        }

        public IList<TodoItem> GetAll()
        {
           return _repository.GetAll();
        }

        public void Delete(int Id)
        {
            _repository.Delete(Id);
        }

        public void Update(int Id, PositionNames Position, string Description, string Name)
        {
            _repository.Update(Id, Position, Description, Name);
        }
    }
}