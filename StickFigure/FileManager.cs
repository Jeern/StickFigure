using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StickFigure
{
    public static class FileManager
    {
        public static string ChooseFolder()
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Choose Stick Figure Folder",
                SelectedPath = Directory.GetCurrentDirectory(),
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.SelectedPath;
            return "";
        }

        public static List<string> GetStickFigureFiles(string folder)
        {
            return Directory.GetFiles(folder, "sf*.txt").Where(f => NumberFromFileName(f) != -1).ToList();
        }

        public static int NumberFromFileName(string fileName)
        {
            //Gets the Number from the StickFigureFileName - -1 if there is no number
            int number;
            if (int.TryParse(Path.GetFileNameWithoutExtension(fileName).Substring(2), out number))
                return number;
            return -1;
        }

        public static Dictionary<int, JointFile> LoadAllFiles(string folder)
        {
            var dict = new Dictionary<int, JointFile>();
            var fileNames = GetStickFigureFiles(folder);
            foreach (var fileName in fileNames)
            {
                dict.Add(NumberFromFileName(fileName), Load(fileName));
            }
            return dict;
        }

        public static JointFile Load(string fileName)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<JointFile>(File.ReadAllText(fileName), settings);
        }

        public static void Save(string fileName, JointFile jointFile)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(jointFile, settings));
        }

        public static string GetFileName(int number, string folder)
        {
            return Path.Combine(folder, $"sf{number}.txt");
        }
    }
}
