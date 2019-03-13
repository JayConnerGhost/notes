namespace Notepad.Dtos
{
    public class Idea
    {
        public string IdeaDescription { get; }

        public Idea(string ideaDescription)
        {
            IdeaDescription = ideaDescription;
        }
    }
}