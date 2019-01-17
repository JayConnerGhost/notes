using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Notepad.UI
{
    public struct FormInfo
    {
        public string FormCaption;
        public int FormWidth;
        public int FormHeight;

        public FormInfo(string formCaption, int formWidth, int formHeight)
        {
            FormCaption = formCaption.Trim();
            FormWidth = formWidth;
            FormHeight = formHeight;
        }
    }

    public class DialogForm : Form
    {
        public DialogForm(FormInfo info):base()
        {
            this.Height = info.FormHeight;
            this.Width = info.FormWidth;
            this.Text = info.FormCaption;
        }
    }


    //https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
    public class SearchController
    {
        
        public string ShowDialog()
        {
            TextBox input =new TextBox(){Left=16, Top=45, Width=240, TabIndex=0, TabStop = true};
            using (var form = new DialogForm(new FormInfo("Search Form", 280, 160)))
            {
                Label label=new Label(){Left = 16, Top=20, Width = 240, Text="Please Enter Search Term"};
                Button confirmation = new Button() { Text = "Search", Left = 16, Width = 80, Top = 88, TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) => { form.Close(); };
                form.Controls.Add(label);
                form.Controls.Add(input);
                form.Controls.Add(confirmation);
                form.AcceptButton=confirmation;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                form.ShowDialog();
            }

            return input.Text;
        }

    }
}