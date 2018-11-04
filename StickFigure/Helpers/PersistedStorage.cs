using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace StickFigure.Helpers
{
    public static class PersistedStorage
    {
        public static string Get(string key)
        {
            using (var isoStore = IsolatedStorageFile.GetMachineStoreForAssembly())
            {
                if (isoStore.FileExists($"{key}.txt"))
                {
                    using (var fs = isoStore.OpenFile($"{key}.txt", FileMode.Open))
                    using (var sr = new StreamReader(fs))
                    {
                        return sr.ReadToEnd();
                    }
                }
                return null;
            }
        }

        public static void Set(string key, string value)
        {
            using (var isoStore = IsolatedStorageFile.GetMachineStoreForAssembly())
            using (var fs = isoStore.OpenFile($"{key}.txt", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(value);
            }
        }
    }
}
