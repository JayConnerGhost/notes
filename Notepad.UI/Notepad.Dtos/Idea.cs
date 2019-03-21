namespace Notepad.Dtos
{
    public class Idea
    {
        public int Id { get; }
        public string Description { get; }

        public Idea(string description)
        {
            Description = description;
        }
        public Idea(string description, int id)
        {
            Id = id;
            Description = description;
        }
    }
}