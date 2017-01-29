using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Ikc5.AutomataScreenSaver.Life.Models
{
	public static class GraphicsHelpers
	{
		/// <summary>
		/// Modified version of
		/// http://stackoverflow.com/questions/2070365/how-to-generate-an-image-from-text-on-fly-at-runtime
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="font"></param>
		/// <param name="textColor"></param>
		/// <param name="backColor"></param>
		/// <returns></returns>
		public static Bitmap DrawTextImage(string[] strings, int width, int height, Font font, Color textColor, Color backColor)
		{
			// First, create a dummy bitmap just to get a graphics object
			SizeF textSize;
			var text = string.Join(Environment.NewLine, strings);

			using (var dummyImage = new Bitmap(1, 1))
			using (var graphics = Graphics.FromImage(dummyImage))
			{
				// measure the string to see how big the image needs to be
				textSize = graphics.MeasureString(text, font);
			}

			// Create a new image of the size enough for text
			var textImage = new Bitmap((int)textSize.Width, (int)textSize.Height);
			using (var graphics = Graphics.FromImage(textImage))
			{
				// Paint the background
				graphics.Clear(backColor);

				// Create a brush for the text
				using (var textBrush = new SolidBrush(textColor))
				{
					graphics.DrawString(text, font, textBrush, 0, 0);
					graphics.Save();
				}
			}

			// Create a image of the right size
			var finalImage = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(finalImage))
			{
				graphics.DrawImage(textImage, new Rectangle(0, 0, width, height));
				graphics.Save();
			}

			return finalImage;
		}

		/// <summary>
		/// Modified version of
		/// http://www.codeproject.com/Articles/15186/Bitonal-TIFF-Image-Converter-for-NET
		/// </summary>
		/// <param name="originalImage"></param>
		/// <returns></returns>
		public static IEnumerable<Point> ConvertToPoints(Bitmap originalImage)
		{
			Bitmap source = null;
			var points = new List<Point>();

			try
			{
				// If originalImage bitmap is not already in 32 BPP, ARGB format, then convert
				if (originalImage.PixelFormat != PixelFormat.Format32bppArgb)
				{
					source = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb);
					source.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
					using (Graphics g = Graphics.FromImage(source))
					{
						g.DrawImageUnscaled(originalImage, 0, 0);
					}
				}
				else
				{
					source = originalImage;
				}

				// Lock source bitmap in memory
				var sourceData = originalImage.LockBits(
					new Rectangle(0, 0, source.Width, source.Height),
					ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

				// Copy image data to binary array
				var imageSize = sourceData.Stride * sourceData.Height;
				var sourceBuffer = new byte[imageSize];
				Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

				// Unlock source bitmap
				source.UnlockBits(sourceData);

				// Add points to the list if corresponding pixel is black
				var height = source.Height;
				var width = source.Width;
				const int threshold = 3*128;

				// Iterate lines
				for (var y = 0; y < height; y++)
				{
					var sourceIndex = y * sourceData.Stride;

					// Iterate pixels
					for (var x = 0; x < width; x++)
					{
						// Compute pixel brightness (i.e. total of Red, Green, and Blue values) - Thanks murx
						//                           B                             G                              R
						var pixelTotal = sourceBuffer[sourceIndex] + sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2];
						if (pixelTotal < threshold)
						{
							points.Add(new Point(x, y));
						}
						sourceIndex += 4;
					}
				}
			}
			finally
			{
				// Dispose of source if not originally supplied bitmap
				if (source!= null && source != originalImage)
				{
					source.Dispose();
				}
			}
			// Return
			return points;
		}
	
	}
}
