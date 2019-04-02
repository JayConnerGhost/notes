using System.Collections;
using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.UI;

namespace Notepad.Repositories
{
    public interface ITodoRepository
    {
        int Create(string testItem, string testItem1Description);
        IList<TodoItem> GetAll();
        void Delete(int id);
        void Update(int id, PositionNames position, string description, string name);
    }
}