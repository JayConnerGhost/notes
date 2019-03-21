using System.Windows.Forms;

namespace Notepad.UI
{
    public class AddIdeaDialog
    {
        public string ShowDialog()
        {
            TextBox input = new TextBox() { Left = 16, Top = 45, Width = 320, Height = 250, TabIndex = 0, TabStop = true, Multiline = true};

            using (var form = new DialogForm(new FormInfo("Add Idea", 400, 400)))
            {
                Label label = new Label() { Left = 16, Top = 20, Width = 240, Text = "Please Enter Idea" };
                Button confirmation = new Button() { Text = "Save", Left = 16, Width = 80, Top = 300, TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) => { form.Close(); };
                form.Controls.Add(label);
                form.Controls.Add(confirmation);
                form.Controls.Add(input);
                //TODO build form
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                form.ShowDialog();
            }

            return input.Text;
        }
    }
}