using System;
using System.Collections.Generic;
using System.Linq;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickFigure.Graphics;

namespace StickFigure.Input
{
    public static class InputManager
    {
        private static bool _actionHappened = true;
        public static void RefreshFiles(JointManager joinManager)
        {
            if (_actionHappened)
            {
                _actionHappened = false;
                Globals.Files = FileManager.LoadAllFiles(Globals.CurrentFolder);
                if (!Globals.Files.ContainsKey(Globals.CurrentShownNumber))
                {
                    Globals.CurrentShownNumber = 1;
                }

                if (Globals.Files.ContainsKey(Globals.CurrentShownNumber))
                {
                    var file = Globals.Files[Globals.CurrentShownNumber];
                    joinManager.FromFile(file);
                }
            }
        }

        public static void InBetweenGeneration()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.I))
            {
                _actionHappened = true;
                if (Globals.Files.Count < 2)
                    return;

                var lastFileNumber = 0;
                foreach (var fileNumber in Globals.Files.Keys.OrderBy(k => k))
                {
                    if (lastFileNumber > 0 && lastFileNumber < fileNumber - 1)
                    {
                        InBetweenGeneration(lastFileNumber, fileNumber);
                    }

                    lastFileNumber = fileNumber;
                }
            }
        }

        private static void InBetweenGeneration(int from, int to)
        {
            var file1 = Globals.Files[from];
            var file2 = Globals.Files[to];
            if (file2.IsLast)
            {
                file2 = Globals.Files[1];
            }

            for (int fileNumber = from + 1; fileNumber < to; fileNumber++)
            {
                var newJoints = new List<ConcreteJoint>();
                foreach (var concreteJoint1 in file1.ConcreteJoints)
                {
                    var concreteJoint2 = file2.ConcreteJoints.FirstOrDefault(cj => cj.Id == concreteJoint1.Id);
                    if (concreteJoint2 == null)
                    {
                        newJoints.Add(concreteJoint1);
                    }
                    else
                    {
                        newJoints.Add(ConcreteJoint.CreateInBetweenJoint(concreteJoint1, concreteJoint2, fileNumber - from, to - from));
                    }
                }

                var newFile = new JointFile
                {
                    ConcreteJoints = newJoints,
                    Lines = file1.Lines
                };
                FileManager.Save(FileManager.GetFileName(fileNumber, Globals.CurrentFolder), newFile);
            }



        }

        public static void LeftRightArrowKey()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Left))
            {
                _actionHappened = true;
                var keys = Globals.Files.Keys.OrderBy(k => k);
                var max = keys.LastOrDefault();
                var min = keys.FirstOrDefault();
                do
                {
                    Globals.CurrentShownNumber--;
                    if (Globals.CurrentShownNumber < min && max < 1)
                    {
                        Globals.CurrentShownNumber = 1;
                    }
                    else if (Globals.CurrentShownNumber < min)
                    {
                        Globals.CurrentShownNumber = max;
                    }
                } while (Globals.CurrentShownNumber != 1 && !Globals.Files.ContainsKey(Globals.CurrentShownNumber));
            }
            else if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Right))
            {
                _actionHappened = true;
                var keys = Globals.Files.Keys.OrderBy(k => k);
                var max = keys.LastOrDefault();
                do
                {
                    Globals.CurrentShownNumber++;
                    if (Globals.CurrentShownNumber > max)
                    {
                        Globals.CurrentShownNumber = 1;
                    }
                } while (Globals.CurrentShownNumber != 1 && !Globals.Files.ContainsKey(Globals.CurrentShownNumber));
            }
        }

        public static void UpDOwnArrowKey()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Up))
            {
                _actionHappened = true;
                Globals.CurrentActionNumber++;
            }
            else if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Down))
            {
                _actionHappened = true;
                Globals.CurrentActionNumber--;
                if (Globals.CurrentActionNumber < 1)
                {
                    Globals.CurrentActionNumber = 1;
                }
            }
        }

        public static void CreateGraphic(GraphicsDevice graphicsDevice, Action<RenderTarget2D, Vector2> draw)
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.P))
            {
                _actionHappened = true;
                var dimensions = PngCreator.GetDimensions(Globals.Files.Values.SelectMany(f => f.ConcreteJoints));
                var texture = PngCreator.GetTexture(dimensions, graphicsDevice);
                draw(texture, new Vector2(-dimensions.X, -dimensions.Y));
                string pngFile = FileManager.GetPngFileName(Globals.CurrentShownNumber, Globals.CurrentFolder);
                PngCreator.Save(texture, pngFile);
                JpgCreator.ConvertPngToJpg(pngFile, FileManager.GetJpgFolder(Globals.CurrentFolder));

            }
        }

        public static void CreateGif()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.G))
            {
                _actionHappened = true;
                GifCreator.CreateGif(Globals.CurrentFolder);
            }
        }

        public static void Save(JointManager jointManager)
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.S))
            {
                _actionHappened = true;
                FileManager.Save(FileManager.GetFileName(Globals.CurrentShownNumber, Globals.CurrentFolder),
                    jointManager.ToFile());
            }
        }

        public static void MarkAsLast()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.L))
            {
                _actionHappened = true;
                FileManager.MarkAsLast(FileManager.GetFileName(Globals.CurrentActionNumber, Globals.CurrentFolder));
            }
        }

        public static void Copy()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.C))
            {
                _actionHappened = true;
                if (Globals.CurrentShownNumber != Globals.CurrentActionNumber)
                {
                    FileManager.Copy(FileManager.GetFileName(Globals.CurrentShownNumber, Globals.CurrentFolder),
                        FileManager.GetFileName(Globals.CurrentActionNumber, Globals.CurrentFolder));
                }
            }
        }
    }
}
