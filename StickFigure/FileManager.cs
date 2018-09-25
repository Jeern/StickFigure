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
        private const string LastFileText = "Last";

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
            int lastKey = -1;
            foreach (var fileName in fileNames)
            {
                var file = Load(fileName);
                var fileNumber = NumberFromFileName(fileName);
                if (file.IsLast)
                {
                    lastKey = fileNumber;
                }
                dict.Add(fileNumber, file);
            }

            return RemoveFilesAfterLast(dict, lastKey);
        }

        private static Dictionary<int, JointFile> RemoveFilesAfterLast(Dictionary<int, JointFile> dict, int lastKey)
        {
            if (lastKey < 0)
                return dict;

            var keys = dict.Keys;
            for (int idx = keys.Count - 1; idx >= 0; idx--)
            {
                var key = keys.ElementAt(idx);
                if (key > lastKey)
                {
                    dict.Remove(key);
                }

            }

            return dict;
        }

        public static JointFile Load(string fileName)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var fileContent = File.ReadAllText(fileName);
            if (fileContent == LastFileText)
            {
                fileContent = File.ReadAllText(GetFileName(1, Globals.CurrentFolder));
                var fileLast = JsonConvert.DeserializeObject<JointFile>(fileContent, settings);
                fileLast.IsLast = true;
                return fileLast;
            }
            var file = JsonConvert.DeserializeObject<JointFile>(fileContent, settings);
            file.IsLast = false;
            return file;

        }

        public static void Save(string fileName, JointFile jointFile)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(jointFile, settings));
        }

        public static void Copy(string from, string to)
        {
            File.Copy(from, to);
        }

        public static string GetFileName(int number, string folder)
        {
            return Path.Combine(folder, $"sf{number}.txt");
        }

        public static void MarkAsLast(string fileName)
        {
            File.WriteAllText(fileName, LastFileText);
        }
    }
}
