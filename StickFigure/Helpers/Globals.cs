using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace StickFigure.Helpers
{
    public static class Globals
    {
        public static string CurrentFolder { get; set; }

        public static Dictionary<int, JointFile> Files { get; set; }

        public static int CurrentShownNumber { get; set; }
        public static int CurrentActionNumber { get; set; }

        public static string LastUsedPath
        {
            get => PersistedStorage.Get(nameof(LastUsedPath));
            set => PersistedStorage.Set(nameof(LastUsedPath), value);
        }
    }
}
