using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using StickFigure.Helpers;

namespace StickFigure.Graphics
{
    /// <summary>
    /// Code from https://social.msdn.microsoft.com/Forums/vstudio/en-US/0c4252c8-8274-449c-ad9b-e4f07a8f8cdd/how-could-i-create-an-animated-gif-file-from-several-other-jpg-files-in-c-express?forum=csharpgeneral
    /// But fixed to resemble normal C#
    /// </summary>
    public static class GifCreator
    {
        private static readonly byte[] GifAnimation = { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 };
        private static byte[] Delay = { 20, 0 };

        public static void CreateGif(string currentFolder)
        {
            string gifFolder = FileManager.GetGifFolder(currentFolder);
            Directory.CreateDirectory(gifFolder);

            string jpgFolder = FileManager.GetJpgFolder(currentFolder);
            string gifFile = Path.Combine(gifFolder, "sf.gif");

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(new FileStream(gifFile, FileMode.Create)))
            {
                string[] jpgFiles = Directory.GetFiles(jpgFolder, "*.jpg").OrderBy(NumberPart).ToArray();

                Image.FromFile(jpgFiles[0]).Save(ms, ImageFormat.Gif);
                byte[] bytes = ms.ToArray();
                bytes[10] = (byte)(bytes[10] & 0X78); //No global color table.
                bw.Write(bytes, 0, 13);
                bw.Write(GifAnimation);
                WriteGifImg(bytes, bw);
                for (int idx = 1; idx < jpgFiles.Length; idx++)
                {
                    ms.SetLength(0);
                    Image.FromFile(jpgFiles[idx]).Save(ms, ImageFormat.Gif);
                    bytes = ms.ToArray();
                    WriteGifImg(bytes, bw);
                }
                bw.Write(bytes[bytes.Length - 1]);
            }
        }

        private static int NumberPart(string fileName)
        {
            return Convert.ToInt32(Path.GetFileNameWithoutExtension(fileName).Replace("sf", ""));
        }

        private static  void WriteGifImg(byte[] bytes, BinaryWriter bw)
        {
            bytes[785] = Delay[0]; //5 secs delay
            bytes[786] = Delay[1];
            bytes[798] = (byte)(bytes[798] | 0X87);
            bw.Write(bytes, 781, 18);
            bw.Write(bytes, 13, 768);
            bw.Write(bytes, 799, bytes.Length - 800);
        }

    }
}
