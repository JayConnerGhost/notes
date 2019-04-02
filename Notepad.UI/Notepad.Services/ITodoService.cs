using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.UI;

namespace Notepad.Services
{
    public interface ITodoService
    {
        int Create(TodoItem Todo);
        IList<TodoItem> GetAll();
        void Delete(int Id);
        void Update(int Id, PositionNames Position, string Description, string Name);
    }
}