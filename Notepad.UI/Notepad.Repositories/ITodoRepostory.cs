using System.Collections;
using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Repositories
{
    public interface ITodoRepository
    {
        int Create(string testItem, string testItem1Description);
        IList<TodoItem> GetAll();
    }
}