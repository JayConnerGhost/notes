namespace Notepad.Dtos
{
    public class TodoItem:ITodoItem
    {

        public TodoItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}