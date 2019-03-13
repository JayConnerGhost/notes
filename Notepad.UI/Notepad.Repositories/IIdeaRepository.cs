namespace Notepad.Repositories
{
    public interface IIdeaRepository
    {
        void Create(string ideaDescription);
        void Retrieve();
    }
}