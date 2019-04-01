using System.Collections.Generic;
using Notepad.UI;

namespace Notepad.Dtos
{
    public interface ITodoItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        PositionNames Position { get; set; }
    }
}