using System.IO;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class IdeaController
    {
        public IdeaController(TabControl area)
        {
            BuildUIArea(area);
        }                                                                        

        private void BuildUIArea(TabControl area)
        {
            var itemList = new ListView {CheckBoxes = true, Dock = DockStyle.Fill};
            
            var tabPage = new TabPage {Dock = DockStyle.Fill,Text = "Ideas"};
            tabPage.Controls.Add(itemList);
            area.TabPages.Add(tabPage);
            itemList.View = View.List;
            itemList.Items.Add("test idea 1");
            itemList.Items.Add("test idea 2");
            itemList.Items.Add("test idea 3");
        }
    }
}