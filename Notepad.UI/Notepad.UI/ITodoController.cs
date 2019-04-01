using System.Collections;
using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.UI
{
    public interface ITodoController
    {
        void Show();
        void Add(string testItem, string name);
        void GetAll();
    }
}