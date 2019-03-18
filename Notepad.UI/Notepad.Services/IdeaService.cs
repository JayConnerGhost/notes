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

        public void New(Idea idea)
        {
            _repository.Create(idea.IdeaDescription);
        }

        public IList<Idea> All()
        {
            return _repository.Retrieve();
        }
    }
}