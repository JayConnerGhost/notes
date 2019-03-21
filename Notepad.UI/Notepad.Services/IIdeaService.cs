using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Services
{
    public interface IIdeaService
    {
        int New(Idea idea);
        IList<Idea> All();
        void Delete(int id);
    }
}