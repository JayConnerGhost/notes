using System.Collections.Generic;

namespace Notepad.Dtos
{
    public interface ITodoItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}