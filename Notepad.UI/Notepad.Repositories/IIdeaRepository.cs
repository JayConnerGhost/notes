using System.Collections;
using System.Collections.Generic;
using Notepad.Dtos;

namespace Notepad.Repositories
{
    public interface IIdeaRepository
    {
        int Create(string ideaDescription);
        IList<Idea> Retrieve();
        void Delete(int id);
        Idea Get(int itemId);
        void Update(string editedDescription, string itemId);
    }
}