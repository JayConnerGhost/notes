using System;
using System.Collections.Generic;
using Notepad.Adapters;
using Notepad.Dtos;
using Notepad.Repositories;
using Notepad.Services;
using Notepad.UI;
using NSubstitute;
using Xunit;

namespace Notepad.TODO.Tests
{
    //UI Tests and full stack
    public class TodoTests : IDisposable
    {
        ITodoController _controller;
        ITodoService _service;
        const string ConnectionString = "Data Source=MyDatabase.sqlite;Version=3";
        const string DatabaseName = "MyDatabase.sqlite";

        public TodoTests()
        {
            _service = Substitute.For<ITodoService>();
            _controller = new TodoController(Substitute.For<ILoggingController>(), _service, new TodoFrame());
          
        }

        [Fact]
        public void Service_is_called_when_a_new_TODO_is_created()
        {
            //Arrange
            const string name = "test item 1";
            var testTodo=new TodoItem(name, string.Empty);
        
          
            //Act
        
            _controller.Add(name, string.Empty);
            //Assert
            _service.Received(1).Create(Arg.Is<TodoItem>(x=>x.Name==name));
        }

        [Fact]
        public void Add_description_to_todo_item()
        {
            //Arrange
            const string description = "test item1 description";
            const string name = "test item 1";
            var testTodo = new TodoItem(name,description);
            
            //Act
            _controller.Add(name,description);
            
            //Assert
            _service.Received(1).Create(Arg.Is<TodoItem>(x => x.Description == description));
        }

        [Fact]
        public void Repository_is_called_when_TODO_is_created()
        {
            //Arrange
            const string description = "test item1 description";
            const string name = "test item 1";
            var testTodo = new TodoItem(name, description);
            ITodoRepository repository=Substitute.For<ITodoRepository>();
            ITodoService service=new TodoService(repository);
            var controller=new TodoController(Substitute.For<ILoggingController>(), service, new TodoFrame());

            //Act
            controller.Add(name, description);

            //Assert
            repository.Received(1).Create(name, description);
        }

        [Fact]
        public void Position_is_set_ToDo_when_Item_is_created()
        {
            //Arrange
            const string name = "test item 2";
            const string description = "test item 2 description, some words and gunk here ";
            var sqliteDbTodoAdapter = new SqliteDbTodoAdapter(ConnectionString,DatabaseName);
            ITodoRepository repository=new TodoRepository(sqliteDbTodoAdapter);
            SetupDatabase(sqliteDbTodoAdapter);
            CreateTodoTable(sqliteDbTodoAdapter);

            //Act

            int ItemId=repository.Create(name,description);
            //Assert
            ITodoItem itemFromDatabase = GetItemFromDatabase(ItemId, sqliteDbTodoAdapter);
            Assert.Equal(name,itemFromDatabase.Name);
            Assert.Equal(description,itemFromDatabase.Description);
        }

        private void CreateTodoTable(SqliteDbTodoAdapter sqliteDbTodoAdapter)
        {
            sqliteDbTodoAdapter.CreateTodoTable();
        }

        private void SetupDatabase(SqliteDbTodoAdapter sqliteDbTodoAdapter)
        {
            sqliteDbTodoAdapter.CreateDatabase(true);
        }

        private ITodoItem GetItemFromDatabase(int itemId, SqliteDbTodoAdapter sqliteDbTodoAdapter)
        {
            return sqliteDbTodoAdapter.Get(itemId);
        }

        public void Dispose()
        {
            _controller = null;
            _service = null;
        }
    }
}
