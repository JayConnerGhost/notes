using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Services
{
    public interface ITodoService
    {
        int Create(TodoItem Todo);
        IList<TodoItem> GetAll();
        void Delete(int Id);
    }
}