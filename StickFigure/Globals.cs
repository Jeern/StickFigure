using System.Collections.Generic;

namespace StickFigure
{
    public static class Globals
    {
        public static string CurrentFolder { get; set; }

        public static Dictionary<int, JointFile> Files { get; set; }

        public static int CurrentShownNumber { get; set; }
        public static int CurrentActionNumber { get; set; }
    }
}
