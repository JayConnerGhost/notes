using System.Collections.Generic;
using System.Data.SQLite;
using Notepad.Dtos;

namespace Notepad.Adapters
{
    public interface IDbAdapter
    {
        void CreateIdeaTable();
        IList<Idea> SelectAllIdeas();
        int CreateIdea(string ideaDescription);
        void Delete(int id);
        Idea Get(int itemId);
        void Update(string editedDescription, string itemId);
    }
}