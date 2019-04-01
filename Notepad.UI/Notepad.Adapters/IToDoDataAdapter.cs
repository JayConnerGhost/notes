using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.UI;

namespace Notepad.Adapters
{
    public interface IToDoDataAdapter
    {
        int CreateToDoItem(string name, string description, PositionNames todo);
        IList<TodoItem> GetAll();
        void Delete(int id);
    }
}