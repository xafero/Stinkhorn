using Stinkhorn.API;
using System.Windows.Forms;
using System.Drawing;
using Mono.Addins;

namespace Stinkhorn.Demo.API
{
    [Extension]
    public class ScreenshotHandler : IResponseHandler<ScreenshotResponse, object>
    {
        public ResponseStatus Process(object sender, ScreenshotResponse msg)
        {
            foreach (var screen in msg.Screenshots)
            {
                var dialog = new Form();
                var box = new PictureBox();
                box.Image = screen.ToDrawingImage();
                box.Dock = DockStyle.Fill;
                box.SizeMode = PictureBoxSizeMode.Zoom;
                dialog.Controls.Add(box);
                dialog.Size = new Size(screen.Width, screen.Height);
                dialog.Text = $"{screen.Width}x{screen.Height} ({screen.Format})";
                dialog.Show();
            }
            return ResponseStatus.NotHandled;
        }
    }
}