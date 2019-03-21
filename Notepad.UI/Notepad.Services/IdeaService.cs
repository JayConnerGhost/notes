using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.Repositories;

namespace Notepad.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _repository;

        public IdeaService(IIdeaRepository repository)
        {
            _repository = repository;

        }

        public int New(Idea idea)
        {
           return _repository.Create(idea.Description);
        }

        public IList<Idea> All()
        {
            return _repository.Retrieve();
        }

        public void Delete(int id)
        {
           _repository.Delete(id);
        }
    }
}