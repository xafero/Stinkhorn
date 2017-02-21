using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Stinkhorn.Core
{
	public static class ImageExtensions
	{
		public static byte[] ToBytes(this Image img, ImageFormat fmt)
		{
			using (var memory = new MemoryStream())
			{
				img.Save(memory, fmt);
				return memory.ToArray();
			}
		}
	}
}