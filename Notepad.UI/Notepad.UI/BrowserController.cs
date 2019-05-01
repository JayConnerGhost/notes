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


          var browserUrl = new TextBox {Dock = DockStyle.Top};
          var toolBar=new ToolBar { Dock = DockStyle.Top };
          var webBrowser = new WebBrowser {Dock = DockStyle.Fill};
          browserDialog.Controls.Add(browserUrl);
          browserDialog.Controls.Add(toolBar);
          browserDialog.Controls.Add(webBrowser);

          browserDialog.ShowDialog();
        }
    }
}
