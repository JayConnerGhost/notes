using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Services
{
    public interface IIdeaService
    {
        void New(Idea idea);
        IList<Idea> All();
    }
}