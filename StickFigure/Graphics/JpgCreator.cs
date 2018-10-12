using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StickFigure.Graphics
{
    public static class JpgCreator
    {
        public static void ConvertPngToJpg(string pngFile, string jpgFolder)
        {
            using (var img = Image.FromFile(pngFile))
            {
                Directory.CreateDirectory(jpgFolder);

                var jpgFile = Path.Combine(jpgFolder,
                    Path.ChangeExtension(Path.GetFileNameWithoutExtension(pngFile), "jpg"));

                img.Save(jpgFile, ImageFormat.Jpeg);
            }
        }
    }
}
