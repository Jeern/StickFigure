using System;
using StickFigure.Helpers;

namespace StickFigure
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Globals.CurrentFolder = FileManager.ChooseFolder();
            Globals.LastUsedPath = Globals.CurrentFolder;
            if(Globals.CurrentFolder == "")
                return;
            Globals.CurrentShownNumber = 1;
            Globals.CurrentActionNumber = 1;

            using (var game = new DrawingSurface())
                game.Run();


            //Pil venstre/højre til op ned til næste fil der skal vises
            //Pil op/ned til næste fil der skal kopieres / Finish
            //S = Save
            //C = Kopier
            //F = Finish
            //I = In-between generering
            //P = Create PNG
        }
    }
#endif
}
