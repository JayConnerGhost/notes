namespace Notepad.Adapters
{
    public interface IToDoDataAdapter
    {
        int CreateToDoItem(string name, string description);
    }
}