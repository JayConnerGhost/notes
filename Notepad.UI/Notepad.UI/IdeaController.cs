using System.IO;
using System.Windows.Forms;
using Notepad.Services;
using Notepad.Dtos;
namespace Notepad.UI
{
    public class IdeaController
    {
        private IdeaService _ideaService;

      
        private void PopulateData(ListView ideaList)
        {
          var ideas=_ideaService.All();
          foreach (var idea in ideas)
          {
              ideaList.Items.Add(idea.IdeaDescription);
          }
        }

        public IdeaController(TabControl area, IdeaService ideaService) 
        {
            this._ideaService = ideaService;
            BuildUIArea(area);
            var ideaList = area.TabPages[1].Controls[0];
            PopulateData((ListView)ideaList);
        }

        private void BuildUIArea(TabControl area)
        {
            var itemList = new ListView {CheckBoxes = true, Dock = DockStyle.Fill};
            
            var tabPage = new TabPage {Dock = DockStyle.Fill,Text = "Ideas"};
            tabPage.Controls.Add(itemList);
            area.TabPages.Add(tabPage);
            itemList.View = View.List;
            
        }
    }
}