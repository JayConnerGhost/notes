namespace Notepad.Dtos
{
    public class TodoItem:ITodoItem
    {

        public TodoItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public int Id { get; set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
    }
}