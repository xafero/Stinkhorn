using System;

namespace Stinkhorn.API
{
	public enum ImageFormat
	{
		PNG
	}
	
	public interface IImage
	{
		ImageFormat Format { get; }
		
		byte[] Bytes { get; }
		
		int Width { get; }
		
		int Height { get; }
	}
	
	public class PngImage : IImage
	{
		public byte[] Bytes { get; private set; }
		
		public int Width { get; private set; }
		
		public int Height { get; private set; }

		public PngImage(byte[] bytes, int width, int height)
		{
			Bytes = bytes;
			Width = width;
			Height = height;
		}

		public ImageFormat Format {
			get { return ImageFormat.PNG; }
		}
	}
}