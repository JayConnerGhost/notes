using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notepad.Dtos;
using Notepad.Services;
using NSubstitute;
using Xunit;

namespace NotePad.Ideas.Tests
{
    
    public class PersistingIdeaTests
    {
        [Fact]
        public void Repository_Is_Called_when_Saving_An_Idea()
        {
            //Arrange
            const string ideaDescription = "test description 1";
            var repository= Substitute.For<IIdeaRepository>();
            IIdeaService service = new IdeaService(repository);

            //Act
            service.New(new Idea(ideaDescription));

            //Assert
            repository.Received().Create(ideaDescription);
        }
    }
}
