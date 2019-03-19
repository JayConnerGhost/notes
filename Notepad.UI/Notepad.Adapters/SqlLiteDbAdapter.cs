using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Notepad.Dtos;

namespace Notepad.Adapters
{
    public class SqlLiteDbAdapter : IDbAdapter
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public SqlLiteDbAdapter(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public void CreateDatabase()
        {
            if (File.Exists(_databaseName))
            {
                return;

            }
            SQLiteConnection.CreateFile(_databaseName);
        }

        public void CreateIdeaTable()
        {
            const string tableName = "Ideas1";
            if (DoesTableExist(tableName))
            {
                return;
            }
             string sql = $"create table {tableName} (description varchar(100))";
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private bool DoesTableExist(string tableName)
        {
            var result = false;
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var sql= $"SELECT * FROM sqlite_master WHERE type = 'table' AND tbl_name = '{tableName}';";
            var command =new SQLiteCommand(sql,connection);
            var dataReader=command.ExecuteReader();
            result = dataReader.HasRows;
            connection.Close();
            return result;
        }

        public IList<Idea> SelectAllIdeas()
        {
            const string sql = "select * from Ideas";
            var ideas = new List<Idea>();
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command =new SQLiteCommand(sql,connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ideas.Add(new Idea((string)reader["description"]));
            }
            
            connection.Close();
            return ideas;
        }

        public void CreateIdea(string ideaDescription)
        {
            var sql = $"insert into Ideas (description) values('{ideaDescription}')";
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}