using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.UI
{
    public class BrowserController
    {
        public void Show()
        {
          var browserDialog=new DialogForm(new FormInfo("Browser",500, 500));

          ConstructUI(browserDialog);
          browserDialog.ShowDialog();
        }

        private void ConstructUI(DialogForm browserDialog)
        {
            var browserUrl = new TextBox { Dock = DockStyle.Top };
            var toolBar = new ToolBar { Dock = DockStyle.Top };
            BuildToolBar(toolBar);
            var webBrowser = new WebBrowser { Dock = DockStyle.Fill };
            browserDialog.Controls.Add(browserUrl);
            browserDialog.Controls.Add(toolBar);
            browserDialog.Controls.Add(webBrowser);
        }

        private void BuildToolBar(ToolBar toolBar)
        {
            //Add back 
            //Add Forward
            //Add Refresh

            throw new NotImplementedException();
        }
    }
}
