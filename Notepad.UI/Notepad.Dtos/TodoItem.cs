namespace Notepad.Dtos
{
    public class TodoItem:ITodoItem
    {

        public TodoItem(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}