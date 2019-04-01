using System;
using Notepad.Dtos;
using Notepad.Repositories;

namespace Notepad.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public void Create(TodoItem todo)
        {
            _repository.Create(todo.Name, todo.Description);
        }
    }
}