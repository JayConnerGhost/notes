using System.Drawing;

namespace Notepad.UI
{
    public class BrandController
    {
        private readonly NotepadController _notepadController;
        private readonly FileBrowserController _fileBrowserController;

        public BrandController(NotepadController notepadController, FileBrowserController fileBrowserController)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
        }

        public void SetBaseStyle()
        {
            _notepadController.Text.BackColor = Color.White;
            _notepadController.Text.ForeColor = Color.Black;
            _notepadController.Text.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
            _fileBrowserController.FileView.BackColor = Color.White;
            _fileBrowserController.FileView.ForeColor=Color.Black;
            _fileBrowserController.FolderView.BackColor = Color.White;
            _fileBrowserController.FolderView.ForeColor = Color.Black;
        }

        public void SetHighContrastStyle()
        {
            _notepadController.Text.BackColor = Color.Black;
            _notepadController.Text.ForeColor = Color.Yellow;
            _notepadController.Text.Font = new Font(FontFamily.GenericSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel);
            _fileBrowserController.FileView.BackColor = Color.Black;
            _fileBrowserController.FileView.ForeColor = Color.Yellow;
            _fileBrowserController.FolderView.BackColor = Color.Black;
            _fileBrowserController.FolderView.ForeColor = Color.Yellow;
        }

        public void SetHackerStyle()
        {
            _notepadController.Text.BackColor = Color.Black;
            _notepadController.Text.ForeColor = Color.ForestGreen;
            _notepadController.Text.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
            _fileBrowserController.FileView.BackColor = Color.Black;
            _fileBrowserController.FileView.ForeColor = Color.ForestGreen;
            _fileBrowserController.FolderView.BackColor = Color.Black;
            _fileBrowserController.FolderView.ForeColor = Color.ForestGreen;
        }
    }
}