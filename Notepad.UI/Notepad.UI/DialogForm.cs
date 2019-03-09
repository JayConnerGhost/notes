using System.Windows.Forms;

namespace Notepad.UI
{
    public class DialogForm : Form
    {
        public DialogForm(FormInfo info):base()
        {
            this.Height = info.FormHeight;
            this.Width = info.FormWidth;
            this.Text = info.FormCaption;
        }
    }
}