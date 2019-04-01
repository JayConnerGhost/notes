using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Services
{
    public interface ITodoService
    {
        void Create(TodoItem Todo);
        IList<TodoItem> GetAll();
    }
}