using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            IDbAdapter database;
            const string ideaDescription = "test idea";
            try
            {
           
                database = SetupDatabase(false);
            }
            catch (FieldAccessException faex)
            {
                Console.WriteLine(faex.Message);
                 database = SetupDatabase(false);
            }
           
            var repository=new IdeaRepository(database);

            //Act
            repository.Create(ideaDescription);

            //Assert
            var result = RetrieveIdeaCollectionFromDatabase(database);
            Assert.NotEmpty(result);

        }

        [Fact]
        public void Should_receive_record_id_after_record_insert()
        {
            //Arrange 
            const string ideaDescription = "test idea 10";
            var sqlLiteDbAdapter = new SqlLiteDbAdapter(ConnectionString,DatabaseName);
            var repository=new IdeaRepository(sqlLiteDbAdapter);

            //Act
            int result=repository.Create(ideaDescription);


            //Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Can_delete_an_idea_from_the_database()
        {
            //Arrange 
            const string ideaDescription = "test idea";
            var database = SetupDatabase(true);
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

        private IDbAdapter SetupDatabase(bool force)
        {
            //connect to SQLlite db 
        
            IDbAdapter dbAdapter = new SqlLiteDbAdapter(ConnectionString, DatabaseName);
            ((SqlLiteDbAdapter)dbAdapter).CreateDatabase(true);
        
            dbAdapter.CreateIdeaTable();
            Thread.Sleep(400);
            return dbAdapter;
        }

        private IList<Idea> RetrieveIdeaCollectionFromDatabase(IDbAdapter database)
        {
            IDbAdapter dbAdapter = new SqlLiteDbAdapter(ConnectionString, DatabaseName);
            return dbAdapter.SelectAllIdeas();
           }
    }
}
