using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace tty.com.Util
{
    public static class ToolUtil
    {
        public static string MD5Encrypt32(string data)
        {
            string cl = data;
            //string pwd = "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in s)
            {
                stringBuilder.AppendFormat("{0:X2}", item);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// StackOverflow <see cref="BitmapImage"/> -> <see cref="byte"/>[]
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
        public static BitmapImage BytesToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }
        public static string BytesToHex(byte[] data)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in data)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", b);
            }
            var hex = ret.ToString();

            return hex;
        }
        public static byte[] HexToBytes(string hex)
        {
            var inputByteArray = new byte[hex.Length / 2];
            for (var x = 0; x < inputByteArray.Length; x++)
            {
                var i = Convert.ToInt32(hex.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            return inputByteArray;
        }
        public static BitmapImage ReSizeImage(BitmapSource bitmap, Size size)
        {
            if (bitmap.PixelWidth <= size.Width && bitmap.PixelHeight <= size.Height)
            {
                return BitmapSourceToBitmapImage(bitmap);
            }
            else
            {
                double destP = 0;
                if ((bitmap.PixelWidth / bitmap.PixelHeight ) > (size.Width / size.Height))
                {
                    destP = size.Width / bitmap.PixelWidth;
                    
                }
                else
                {
                    destP = size.Height / bitmap.PixelHeight;
                }

                var destWidth = (int)(bitmap.PixelWidth * destP);
                var destHeight = (int)(bitmap.PixelHeight * destP);
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                var bSource = BitmapSourceToBitmap(bitmap);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bSource, 0, 0, destWidth, destHeight);

                return BitmapToBitmapImage(b);
            }
        }
        public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
        public static byte[] BitmapToBytes(System.Drawing. Bitmap Bitmap) 
        { 

            MemoryStream ms = null; 
            try 
            { 
                ms = new MemoryStream(); 
                Bitmap.Save(ms, Bitmap.RawFormat); 
                byte[] byteImage = new Byte[ms.Length]; 
                byteImage = ms.ToArray(); 
                return byteImage; 
            } 
            catch (ArgumentNullException ex) 
            { 
                throw ex; 
            } 
            finally 
            { 
                ms.Close(); 
            } 
        } 
        public static System.Drawing.Bitmap BitmapSourceToBitmap(BitmapSource imageSource)
        {
            BitmapSource m = imageSource;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); // 坑点：选Format32bppRgb将不带透明度

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }
    }

}
