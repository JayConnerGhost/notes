using System.Collections.Generic;
using System.Data.SQLite;
using Notepad.Dtos;

namespace Notepad.Adapters
{
    public interface IDbAdapter
    {
        void CreateIdeaTable();
        IList<Idea> SelectAllIdeas();
        void CreateIdea(string ideaDescription);
    }
}