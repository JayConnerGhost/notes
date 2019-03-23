using System.Drawing;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class BrandController
    {
        private readonly NotepadController _notepadController;
        private readonly FileBrowserController _fileBrowserController;
        private readonly IdeaController _ideaController;
        private  ImageList _iconList;
        private Brands ActiveBrand;

        public BrandController(NotepadController notepadController, FileBrowserController fileBrowserController,
            IdeaController ideaController)
        {
            _notepadController = notepadController;
            _fileBrowserController = fileBrowserController;
            _ideaController = ideaController;
            SetUpIconList();
            SetIcons(true);
            ActiveBrand = Brands.Normal;
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
            ActiveBrand = Brands.Normal;
            _notepadController.SetForeColor(Color.Black);
            _notepadController.SetBackColor(Color.White);
            _notepadController.SetFont(new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel));
           
            _fileBrowserController.FileView.BackColor = Color.White;
            _fileBrowserController.FileView.ForeColor=Color.Black;
            _fileBrowserController.FolderView.BackColor = Color.White;
            _fileBrowserController.FolderView.ForeColor = Color.Black;

            _ideaController.SetBackColor(Color.White);
            _ideaController.SetForeColor(Color.Black);
            _ideaController.SetFont(new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel));

        }

        public void SetHighContrastStyle()
        {
            ActiveBrand = Brands.HighContrast;
            _notepadController.SetForeColor(Color.Yellow);
            _notepadController.SetBackColor(Color.Black);
            _notepadController.SetFont(new Font(FontFamily.GenericSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel));
            _fileBrowserController.FileView.BackColor = Color.Black;
            _fileBrowserController.FileView.ForeColor = Color.Yellow;
            _fileBrowserController.FolderView.BackColor = Color.Black;
            _fileBrowserController.FolderView.ForeColor = Color.Yellow;

            _ideaController.SetBackColor(Color.Black);
            _ideaController.SetForeColor(Color.Yellow);
            _ideaController.SetFont(new Font(FontFamily.GenericSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel));

        }

        public void SetHackerStyle()
        {
            ActiveBrand = Brands.Hacker;
            _notepadController.SetForeColor(Color.ForestGreen);
            _notepadController.SetBackColor(Color.Black);
            _notepadController.Text.BackColor = Color.Black;
            _notepadController.Text.ForeColor = Color.ForestGreen;
            _notepadController.SetFont(new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel));

            _fileBrowserController.FileView.BackColor = Color.Black;
            _fileBrowserController.FileView.ForeColor = Color.ForestGreen;
            _fileBrowserController.FolderView.BackColor = Color.Black;
            _fileBrowserController.FolderView.ForeColor = Color.ForestGreen;


            _ideaController.SetBackColor(Color.Black);
            _ideaController.SetForeColor(Color.ForestGreen);
            _ideaController.SetFont(new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel));

        }

        public void BrandTextArea(RichTextBox target)
        {
            switch (ActiveBrand)
            {
                case Brands.HighContrast:
                    target.BackColor=Color.Black;
                    target.ForeColor = Color.Yellow;
                    target.Font=new Font(FontFamily.GenericSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel);
                    break;
                case Brands.Hacker:
                    target.BackColor = Color.Black;
                    target.ForeColor = Color.ForestGreen;
                    target.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
                    break;
                case Brands.Normal:
                    target.BackColor = Color.White;
                    target.ForeColor = Color.Black;
                    target.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Regular, GraphicsUnit.Pixel);
                    break;
            }
        }
    }

    public enum Brands
    {
        HighContrast,
        Hacker,
        Normal
    }
}