using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Notepad.Adapters;
using Notepad.Dtos;
using Notepad.Repositories;
using Xunit;

namespace Notepad.Ideas.Tests
{
    public class DatabaseIdeaTests
    {
        const string ConnectionString = "Data Source=MyDatabase.sqlite;Version=3";
        const string DatabaseName = "MyDatabase.sqlite";

        [Fact]
        public void Can_save_an_idea_to_a_database()
        {
            //Arrange 
            const string ideaDescription = "test idea";
            var database = SetupDatabase();
            var repository=new IdeaRepository(database);

            //Act
            repository.Create(ideaDescription);

            //Assert
            var result = RetrieveIdeaCollectionFromDatabase(database);
            Assert.NotEmpty(result);

        }

        [Fact]
        public void Can_delete_an_idea_from_the_database()
        {
            //Arrange 
            const string ideaDescription = "test idea";
            var database = SetupDatabase();
            var repository = new IdeaRepository(database);
            //Act
            repository.Create(ideaDescription);
            IList<Idea> resultCheckState = RetrieveIdeaCollectionFromDatabase(database); ;
            Assert.NotEmpty(resultCheckState);

            repository.Delete(resultCheckState[0].Id);


            //Assert
            IEnumerable result= RetrieveIdeaCollectionFromDatabase(database); ;
            Assert.Empty(result);
        }

        private IDbAdapter SetupDatabase()
        {
            //connect to SQLlite db 
        
            IDbAdapter dbAdapter = new SqlLiteDbAdapter(ConnectionString, DatabaseName);
            ((SqlLiteDbAdapter)dbAdapter).CreateDatabase();
            dbAdapter.CreateIdeaTable();

            return dbAdapter;
        }

        private IList<Idea> RetrieveIdeaCollectionFromDatabase(IDbAdapter database)
        {
            IDbAdapter dbAdapter = new SqlLiteDbAdapter(ConnectionString, DatabaseName);
            return dbAdapter.SelectAllIdeas();
           }
    }
}
