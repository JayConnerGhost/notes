using Notepad.Dtos;
using Notepad.Services;
using Notepad.UI;
using NSubstitute;
using Xunit;

namespace Notepad.TODO.Tests
{
    //UI Tests and full stack
    public class TodoCreateTests
    {
        [Fact]
        public void Service_is_called_when_a_new_TODO_is_created()
        {
            //Arrange
            const string name = "test item 1";
            var testTodo=new TodoItem(name);
            var service = NSubstitute.Substitute.For<ITodoService>();
            ITodoController controller=new TodoController(Substitute.For<ILoggingController>(),service, new TodoFrame());
            //Act
            controller.Show();
            controller.Add(name);
            //Assert
            service.Received(1).Create(Arg.Is<TodoItem>(x=>x.Name==name));
        }
    }
}
