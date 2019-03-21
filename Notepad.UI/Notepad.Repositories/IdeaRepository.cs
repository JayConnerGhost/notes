using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Notepad.Adapters;
using Notepad.Dtos;

namespace Notepad.Repositories
{
    public class IdeaRepository:IIdeaRepository
    {
        private readonly IDbAdapter _dbAdapter;

        public IdeaRepository(IDbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        public int Create(string ideaDescription)
        {
            return _dbAdapter.CreateIdea(ideaDescription);
        }

        public IList<Idea> Retrieve()
        {
            return _dbAdapter.SelectAllIdeas();
        }

        public void Delete(int id)
        {
            _dbAdapter.Delete(id);
        }
    }
}