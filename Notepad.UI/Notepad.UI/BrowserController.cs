using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.UI
{
    public class BrowserController
    {
        public void Show()
        {
          var browserDialog=new DialogForm(new FormInfo("Browser",500, 500));

          

          browserDialog.ShowDialog();
        }
    }
}
