using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Stinkhorn.API;

using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Stinkhorn.Core
{
	public class ScreenshotHandler
		: IMessageHandler<ScreenshotRequest, ScreenshotResponse>
	{
		public ScreenshotResponse Process(ScreenshotRequest input)
		{
			var images = new List<IImage>();
			foreach (var screen in Screen.AllScreens)
			{
				var width = screen.Bounds.Width;
				var height = screen.Bounds.Height;
				using (var img = new Bitmap(width, height))
				{
					var size = new Size(img.Width, img.Height);
					var graph = Graphics.FromImage(img);
					var x = screen.Bounds.X;
					var y = screen.Bounds.Y;
					graph.CopyFromScreen(x, y, 0, 0, size);
					var bytes = img.ToBytes(ImageFormat.Png);
					images.Add(new PngImage(bytes, img.Width, img.Height));
				}
			}
			return new ScreenshotResponse { Screenshots = images };
		}
	}
}