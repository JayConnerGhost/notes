using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notepad.Ideas.Tests
{
    public class DatabaseIdeaTests
    {
        [Fact]
        public void Can_save_an_idea()
        {
            //Arrange 
            const int expected = 1;
            var repository= new IdeaRepository();
            const string ideaDescription = "this is a test idea";

            //Act
            repository.Create(ideaDescription);

            //Assert
            Assert.Equal(expected,repository.Retrieve().Count);
        }
    }
}
