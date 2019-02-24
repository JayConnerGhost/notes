using System.Drawing;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class BrandController
    {
        private readonly NotepadController _notepadController;
        private readonly FileBrowserController _fileBrowserController;
        private  ImageList _iconList;
        public BrandController(NotepadController notepadController, FileBrowserController fileBrowserController)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
            SetUpIconList();
            SetIcons(true);
        }

        private void SetUpIconList()
        {
         
            _iconList = new ImageList {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(20,20)
            };

            var rfileImage=Notepad.UI.Properties.Resources.ResourceManager.GetObject("file");
            var rfolderImage = Notepad.UI.Properties.Resources.ResourceManager.GetObject("folder");
            var fileImage = (Image) rfileImage;
            var folderImage = (Image)rfolderImage;
            _iconList.Images.Add("File", fileImage);
            _iconList.Images.Add("File", folderImage);
        }

        public void SetIcons(bool display)
        {
            if (display)
            {
                _fileBrowserController.FileView.SmallImageList = _iconList;
            }
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