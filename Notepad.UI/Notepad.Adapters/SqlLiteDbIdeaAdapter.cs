using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using Notepad.Dtos;

namespace Notepad.Adapters
{
    public class SqlLiteDbIdeaAdapter : IDbAdapter
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public SqlLiteDbIdeaAdapter(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public void CreateDatabase(bool force)
       {
            if (File.Exists(_databaseName) && !force)
            {
                return;
            }
            SQLiteConnection.CreateFile(_databaseName);
        }

        public void CreateIdeaTable()
        {
            const string tableName = "Ideas";
            if (DoesTableExist(tableName))
            {
                return;
            }
             string sql = $"create table {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT,description varchar(100))";
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private bool DoesTableExist(string tableName)
        {
            var result = false;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var sql = $"SELECT * FROM sqlite_master WHERE type = 'table' AND tbl_name = '{tableName}';";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        result = reader.HasRows;
                    }

                    connection.Close();
                }
            }

            return result;
        }

        public IList<Idea> SelectAllIdeas()
        {
            const string sql = "select rowid,* from Ideas";
            var ideas = new List<Idea>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ideas.Add(new Idea((string) reader["description"], (int) reader.GetInt32(0)));
                        }
                    }
                }

                connection.Close();
            }

            return ideas;
        }

        public string convertQuotes(string str)
        {
            return str.Replace("'", "''");
        }


        public int CreateIdea(string ideaDescription)
        {
            var finalItemDescription= convertQuotes(ideaDescription);
               var sql = $"insert into Ideas (description) values('{finalItemDescription}')";
     
            using (var connection = new SQLiteConnection(_connectionString))
            {

                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

               connection.Close();
            }
            
            return GetId(ideaDescription);
        }

        private int GetId(string ideaDescription)
        {
            var id = 0;
            var SqlId = $"select id from Ideas where description='{convertQuotes(ideaDescription)}' ORDER BY rowid DESC ";
            using (var connection2 = new SQLiteConnection(_connectionString))
            {
                connection2.Open();
                using (var commandSelect = new SQLiteCommand(SqlId, connection2))
                {
                    using (var data = commandSelect.ExecuteReader())
                    {
                        data.Read();
                        id = data.GetInt32(0);
                    }
                }
                connection2.Close();
            }

            return id;
        }

        public void Delete(int id)
        {
            var sql = $"delete from Ideas where id={id}";

            using (var connection = new SQLiteConnection(_connectionString))
            {

                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public Idea Get(int itemId)
        {
            var sql = $"select * from Ideas where id={itemId}";
            Idea idea;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        idea =new Idea(reader.GetString(1) ,reader.GetInt32(0));
                    }
                }
                connection.Close();
            }
            return idea;
        }

        public void Update(string editedDescription, string itemId)
        {
            var sql = $"update Ideas set Description='{editedDescription}' where id={itemId}";

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

        }
    }
}