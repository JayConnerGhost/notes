using System.Windows.Forms;

namespace Notepad.UI
{
    public class AboutDialog
    {
        public void ShowDialog()
        {
            using (var form = new DialogForm(new FormInfo("About IronText", 260, 400)))
            {
               

                Label lblWritenby = new Label { Text = "Written by Jay Katie Martin" };
                Label lblLicence = new Label { Text = "Licence MIT" };
                Button close = new Button() { Text = "Close", Width = 80, TabIndex = 1, TabStop = true };
                close.Click += (sender, e) => { form.Close(); };
                var layout = new FlowLayoutPanel();
             
                PictureBox picture = new PictureBox
                {
                    ImageLocation = @"Icons\logo.png",
                    Width = 237,
                    Height = 239,
                };
                form.Controls.Add(layout);
                layout.Dock = DockStyle.Fill;
                layout.FlowDirection = FlowDirection.TopDown;
                layout.Controls.Add(picture);
                layout.Controls.Add(lblWritenby);
                layout.Controls.Add(lblLicence);
                layout.Controls.Add(close);
                

         
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                form.ShowDialog();
            }
        }
    }
}