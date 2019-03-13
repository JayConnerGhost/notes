using Notepad.Dtos;

namespace Notepad.Services
{
    public interface IIdeaService
    {
        void New(Idea idea);
        void All();
    }
}