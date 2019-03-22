using System.Windows.Forms;
using Notepad.Dtos;
using Notepad.Services;

namespace Notepad.UI
{
    public class EditIdeaDialog
    {
        private int _itemId;
        private IdeaService _service;

        public string ShowDialog(int itemId, IdeaService service)
        {
            _itemId = itemId;
            _service = service;
           var idea=RetrieveItem();
           return ShowForm(idea);
        }

        private string ShowForm(Idea idea)
        {
            TextBox input = new TextBox
            {
                Left = 16,
                Top = 45,
                Width = 320,
                Height = 250,
                TabIndex = 0,
                TabStop = true,
                Multiline = true,
                Text = idea.Description
            };
            using (var form = new DialogForm(new FormInfo("Edit Idea", 400, 400)))
            {
                Label label = new Label() {Left = 16, Top = 20, Width = 240, Text = "Please Enter Changes"};
                Button confirmation = new Button()
                    {Text = "Save", Left = 16, Width = 80, Top = 300, TabIndex = 1, TabStop = true};
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

        private Idea RetrieveItem()
        {
            return _service.Get(_itemId);
        }
    }
}