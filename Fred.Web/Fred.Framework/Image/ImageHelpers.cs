using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Fred.Framework.Images
{
    /// <summary>
    /// Représente les methodes d'extentions pour les images
    /// </summary>
    public static class ImageHelpers
    {
        /// <summary>
        /// Permet de retaillé une image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="height">La hauteur de l'image max</param>
        /// <returns>Une Image</returns>
        public static object Resize(object img, double height)
        {
            Image image = (Image)img;
            Bitmap resizedImage = null;

            if (image != null)
            {
                Graphics g = Graphics.FromImage(image);
                double h = height * g.DpiY / 2.54d;
                double l = h * image.Width / image.Height;
                resizedImage = new Bitmap(image, (int)l, (int)h);
                resizedImage.SetResolution(g.DpiX, g.DpiY);
                image.Dispose();
                g.Dispose();
            }

            return resizedImage;
        }

        public static byte[] ProcessImage(byte[] imgByteArray, int maxWidth, int maxHeight)
        {
            if (imgByteArray?.Length > 0)
            {
                Image img = (Image)ConvertByteArrayToImage(imgByteArray);

                if (img?.Width > maxWidth)
                {
                    img = (Image)Resize(img, maxWidth, img.Height);
                }

                if (img?.Height > maxHeight)
                {
                    img = (Image)Resize(img, img.Width, maxHeight);
                }

                return ToByteArray(img);
            }

            return null;
        }

        private static object Resize(object img, int width, int height)
        {
            Image image = (Image)img;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        ///   Convertit une imag en tableau de bytes
        /// </summary>
        /// <param name="img">Image</param>
        /// <returns>Tableau de bytes</returns>
        public static byte[] ToByteArray(object img)
        {
            Image image = (Image)img;
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static object ConvertByteArrayToImage(byte[] array)
        {
            var converter = new ImageConverter();
            return converter.ConvertFrom(array);
        }

        public static bool CheckImageSize(this byte[] array, int width, int height)
        {
            ImageConverter converter = new ImageConverter();
            var image = (Image)converter.ConvertFrom(array);

            return (image == null || image.Width <= width) && (image == null || image.Height <= height);
        }

        public static byte[] GetThumbnailImage(byte[] imageInByte, int width, int height)
        {
            var image = (Image)ConvertByteArrayToImage(imageInByte);

            var thumbnail = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

            return ToByteArray(thumbnail);
        }
    }
}
