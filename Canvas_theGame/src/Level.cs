using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Canvas_theGame;
using System.IO;

namespace Canvas_theGame.src
{
    class Level
    {
        #region Values
        public enum levels { DEMO1, DEMO2 };

        private static Game1.okColors originalPrimraryColor, originalBackgroundColor, originalSecondaryColor;
        private Dictionary<Game1.okColors, Color> availableColors;
        private Dictionary<string, Char> markers;
        private static Vector2 startPos;
        private static List<Barrier> barriers;
        private static List<ColorBlob> colorBlobs;
        private static AABB levelEnding;

        private StreamReader streamReader;

        private static levels nextLevel;

        private int blockWidth = 20;
        #endregion

        #region get/set
        public static Vector2 getStartPos()
        {
            return startPos;
        }
        public static List<Barrier> getBarriers()
        {
            return barriers;
        }
        public static List<ColorBlob> getColorBlobs()
        {
            return colorBlobs;
        }
        public static void removeFromColorBlobs(ColorBlob colorBlobToRemove)
        {
            colorBlobs.Remove(colorBlobToRemove);
        }
        public static Game1.okColors getOriginalPrimraryColor()
        {
            return originalPrimraryColor;
        }
        public static Game1.okColors getOriginalSecondaryColor()
        {
            return originalSecondaryColor;
        }
        public static Game1.okColors getOriginalBackgroundColor()
        {
            return originalBackgroundColor;
        }
        #endregion

        public static void Update(AABB player)
        {
            if (player.Intersects(levelEnding))
            {
                loadLevel(nextLevel);
            }
        }

        public static void loadLevel(levels levelToLoad) {
            new Level(levelToLoad);
            Game1.resetLevelStatic();
        }
        private Level(levels levelToLoad) {
            availableColors = Game1.getAvailableColors();
            barriers = new List<Barrier>();
            colorBlobs = new List<ColorBlob>();

            switch (levelToLoad) {
                case levels.DEMO1:
                    loadDEMO1();
                    break;
                case levels.DEMO2:
                    loadDEMO2();
                    break;
            }
        }

        private void initMarkers() {
            markers = new Dictionary<string, char>();
            markers.Add("blank", '0');
            markers.Add("mainColor", '-');
            markers.Add("backgroundColor", '+');
            markers.Add("spawn", '!');
            markers.Add("goal", '?');
            markers.Add("endDefine", '=');   
            markers.Add("newLayer", '|');
        }

        #region Dynamic Load
        private void loadDEMO1()
        {
            originalPrimraryColor = Game1.okColors.BLACK;
            originalSecondaryColor = Game1.okColors.ORANGE;
            originalBackgroundColor = Game1.okColors.WHITE;
            loadFromFile("DEMO1");
            nextLevel = levels.DEMO2;
        }
        private void loadDEMO2()
        {
            originalPrimraryColor = Game1.okColors.BLACK;
            originalSecondaryColor = Game1.okColors.ORANGE;
            originalBackgroundColor = Game1.okColors.WHITE;
            loadFromFile("DEMO2");
            nextLevel = levels.DEMO1;
        }

        private void loadFromFile(string fileName) {
            streamReader = new StreamReader("Content/level_" + fileName + ".txt");

            initMarkers();

            #region DefinePhase
            while (!streamReader.EndOfStream) { //Define phase.
                string holder = streamReader.ReadLine();
                if (holder[0] == '\\' || holder.Length < 1) { //Skip Empty Lines
                    continue;
                }
                else if (holder[0] == markers["endDefine"]) {
                    break;
                }
                else {
                    string substring = holder.Substring(0, holder.IndexOf(':'));
                    if (substring == "blank") {
                        markers["blank"] = holder[holder.Length - 1];
                    }
                    else if (substring == "mainColor")
                    {
                        markers["mainColor"] = holder[holder.Length - 1];
                    }
                    else if (substring == "backgroundColor")
                    {
                        markers["backgroundColor"] = holder[holder.Length - 1];
                    }
                    else if (substring == "spawn")
                    {
                        markers["spawn"] = holder[holder.Length - 1];
                    }
                    else if (substring == "goal")
                    {
                        markers["goal"] = holder[holder.Length - 1];
                    }
                    else if (substring == "newLayer")
                    {
                        markers["newLayer"] = holder[holder.Length - 1];
                    }
                }
            }
            #endregion

            #region LoadPhase
            int j = -1, k = 0;
            while (!streamReader.EndOfStream) {
                string holder = streamReader.ReadLine();
                j++;
                k = 0;
                for (int i = 0; i < holder.Length; i++, k++) {
                    if (holder[i] == '\\' || holder.Length < 1){ //Skip Empty Lines
                        break;
                    }
                    else if (holder[i] == markers["blank"])
                    {
                        continue;
                    }
                    else if (holder[i] == markers["newLayer"]) {
                        k = -1;
                    }
                    else if (holder[i] == markers["spawn"])
                    {
                        startPos = new Vector2((blockWidth * k), blockWidth * j);
                    }
                    else if (holder[i] == markers["goal"])
                    {
                        levelEnding = new AABB(new Rectangle(blockWidth * k, blockWidth * j, 20, 20));
                    }
                    else if (holder[i] == markers["mainColor"])
                    { //FirstColor
                        barriers.Add(new Barrier(new Rectangle(blockWidth * (k), blockWidth * (j), 20, 20), originalPrimraryColor));
                    }
                    else if (holder[i] == markers["backgroundColor"])
                    { //BackgroundColor
                        barriers.Add(new Barrier(new Rectangle(blockWidth * (k), blockWidth * (j), 20, 20), originalBackgroundColor));
                    }
                    else //Undefined characters in the wrong places.
                    {
                        k--;
                    }
                }
            }
            #endregion

            streamReader.Close();
        }
        #endregion

        #region Static Load (OLD)
        private void loadDEMO() {
            originalPrimraryColor = Game1.okColors.BLACK;
            originalBackgroundColor = Game1.okColors.WHITE;
            startPos = new Vector2(350 - 7, 250); //7 = half player width.

            barriers.Add(new Barrier(new Rectangle(new Point(300, 300), new Point(100, blockWidth)), Game1.okColors.BLACK)); //Floor
            barriers.Add(new Barrier(new Rectangle(new Point(300, 200), new Point(100, blockWidth)), Game1.okColors.BLACK)); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(280, 200), new Point(blockWidth, 120)), Game1.okColors.BLACK)); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(380, 200), new Point(blockWidth, 120)), Game1.okColors.BLACK)); //Right wall

            barriers.Add(new Barrier(new Rectangle(new Point(700, 300), new Point(100, blockWidth)), Game1.okColors.WHITE)); //White end floor
            /* colorBlobs.Add(new ColorBlob(new Rectangle(new Point(730, 250), new Point(20, 20)), Game1.okColors.WHITE)); //Ending white blob
            barriers.Add(new Barrier(new Rectangle(new Point(700, 200), new Point(100, blockWidth)), Game1.okColors.ORANGE)); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(680, 200), new Point(blockWidth, 320)), Game1.okColors.ORANGE)); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(800, 200), new Point(blockWidth, 200)), Game1.okColors.ORANGE)); //Right wall
            */
            barriers.Add(new Barrier(new Rectangle(new Point(300, 600), new Point(500, blockWidth)), Game1.okColors.WHITE)); //Bottom Line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 500), new Point(100, blockWidth)), Game1.okColors.WHITE)); //White help line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 400), new Point(100, blockWidth)), Game1.okColors.BLACK)); //Black help line
            // olorBlobs.Add(new ColorBlob(new Rectangle(new Point(540, 370), new Point(20, 20)), Game1.okColors.ORANGE)); //Middle orange blob
        }
        #endregion
    }
}
