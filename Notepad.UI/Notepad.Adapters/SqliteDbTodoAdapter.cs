using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Notepad.Dtos;
using Notepad.UI;

namespace Notepad.Adapters
{
    public class SqliteDbTodoAdapter:IToDoDataAdapter
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public SqliteDbTodoAdapter(string connectionString, string databaseName)
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

        public int CreateToDoItem(string name, string description, PositionNames position)
        {
            var sql = $"insert into Todos (name, description, position) values('{name}','{description}','{position}')";

            using (var connection = new SQLiteConnection(_connectionString))
            {

                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return GetId(name);
        }

        public IList<TodoItem> GetAll()
        {
            const string sql = "select * from Todos";
            var todos = new List<TodoItem>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var todoItem = new TodoItem((string) reader["name"], (string) reader["description"])
                            {
                                Id = (int) reader.GetInt32(0),
                                Position = (PositionNames)Enum.Parse(typeof(PositionNames), reader.GetString(3))

                            };
                            todos.Add(todoItem);
                        }
                    }
                }

                connection.Close();
            }

            return todos;
        }

        public void Delete(int id)
        {
            var sql = $"delete from Todos where id={id}";

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

        private int GetId(string name)
        {
            var id = 0;
            var SqlId = $"select id from Todos where name='{name}' ORDER BY rowid DESC ";
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

        public void CreateTodoTable()
        {
            const string tableName = "Todos";
            if (DoesTableExist(tableName))
            {
                return;
            }
            string sql = $"create table {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT, name varchar(50), description varchar(250), position varchar(250))";
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

        public ITodoItem Get(int itemId)
        {
            var sql = $"select * from Todos where id={itemId}";
            TodoItem idea;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        idea = new TodoItem(reader.GetString(1), reader.GetString(2))
                        {
                            Id = reader.GetInt32(0),
                            Position=(PositionNames)Enum.Parse(typeof(PositionNames),reader.GetString(3))
                        };
                    }
                }
                connection.Close();
            }
            return idea;
        }
    }
}