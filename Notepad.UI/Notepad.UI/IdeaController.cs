using System.IO;
using System.Windows.Forms;
using Notepad.Services;
using Notepad.Dtos;
namespace Notepad.UI
{
    public class IdeaController
    {
        private readonly TabControl _area;
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
            _area = area;
            this._ideaService = ideaService;
            BuildUIArea(_area);
            var ideaList = GetIdeaList(_area);
            PopulateData((ListView)ideaList);
        }

        private static Control GetIdeaList(TabControl area)
        {
            var table = area.TabPages[1].Controls[0];
            var ideaList = table.Controls[0];
            return ideaList;
        }

        private void BuildUIArea(TabControl area)
        {
            var itemList = new ListView {CheckBoxes = true, Dock = DockStyle.Fill};
            
            var tabPage = new TabPage {Dock = DockStyle.Fill,Text = "Ideas"};
            //add tablelayoutpanel
            var tableLayout = new TableLayoutPanel {ColumnCount = 1, RowCount = 2,Dock = DockStyle.Fill};
            tableLayout.Controls.Add(itemList,0,1);
            var buttonLayout = new FlowLayoutPanel {FlowDirection = FlowDirection.LeftToRight,Height = 25,Dock = DockStyle.Top};
            var addButton = new Button(){Text="Add"};
            addButton.Click += AddButton_Click;
            buttonLayout.Controls.Add(addButton);
            var deleteButton = new Button(){Text = "Delete"};
            deleteButton.Click += DeleteButton_Click;
            buttonLayout.Controls.Add(deleteButton);
            tableLayout.Controls.Add(buttonLayout,0,1);
            
            tabPage.Controls.Add(tableLayout);
            area.TabPages.Add(tabPage);
            itemList.View = View.List;
            
        }

        private void DeleteButton_Click(object sender, System.EventArgs e)
        {
            var selectedIdeas = ((ListView)GetIdeaList(_area)).CheckedItems;
            //get selected controls 
            foreach (ListViewItem idea in selectedIdeas)
            {
                idea.Remove();
            }
        }

        private void AddButton_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Add Record");
        }
    }
}