using System;
using System.Collections.Generic;
using Notepad.Dtos;
using Notepad.Repositories;

namespace Notepad.Ideas.Tests
{
    public class IdeaRepository:IIdeaRepository
    {
        private readonly List<Idea> _ideas=new List<Idea>();
        public void Create(string ideaDescription)
        {
            _ideas.Add(new Idea(ideaDescription));
        }

        public IList<Idea> Retrieve()
        {
            return _ideas;
        }
    }
}