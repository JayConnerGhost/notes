using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Adapters
{
    public interface IToDoDataAdapter
    {
        int CreateToDoItem(string name, string description);
        IList<TodoItem> GetAll();
    }
}