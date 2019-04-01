using Notepad.Dtos;

namespace Notepad.Services
{
    public interface ITodoService
    {
        void Create(TodoItem Todo);
    }
}