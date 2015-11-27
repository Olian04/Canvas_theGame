using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Canvas_theGame;

namespace Canvas_theGame.src
{
    class Level
    {
        public enum levels {DEMO};

        private static Game1.okColors originalPrimraryColor, originalSecondaryColor;
        private static Point startPos;
        private static List<Barrier> barriers;
        private static List<ColorBlob> colorBlobs;

        public static void loadLevel(levels levelToLoad) {
            new Level(levelToLoad);
        }

        private Dictionary<Game1.okColors, Color> availableColors;
        private Level(levels levelToLoad) {
            availableColors = Game1.getAvailableColors();
            barriers = new List<Barrier>();
            colorBlobs = new List<ColorBlob>();

            switch (levelToLoad) {
                case levels.DEMO:
                    loadDEMO();
                    break;
            }
        }
        public static Point getStartPos() {
            return startPos;
        }
        public static List<Barrier> getBarriers() {
            return barriers;
        }
        public static List<ColorBlob> getColorBlobs() {
            return colorBlobs;
        }
        public static void removeFromColorBlobs(ColorBlob colorBlobToRemove) {
            colorBlobs.Remove(colorBlobToRemove);
        }
        public static Game1.okColors getOriginalPrimraryColor() {
            return originalPrimraryColor;
        }
        public static Game1.okColors getOriginalSecondaryColor() {
            return originalSecondaryColor;
        }

        private void loadDEMO() {
            originalPrimraryColor = Game1.okColors.BLACK;
            originalSecondaryColor = Game1.okColors.WHITE;
            startPos = new Point(350 - 7, 250); //7 = half player width.

            barriers.Add(new Barrier(new Rectangle(new Point(300, 300), new Point(100, 20)), availableColors[Game1.okColors.BLACK])); //Floor
            barriers.Add(new Barrier(new Rectangle(new Point(300, 200), new Point(100, 20)), availableColors[Game1.okColors.BLACK])); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(280, 200), new Point(20, 120)), availableColors[Game1.okColors.BLACK])); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(380, 200), new Point(20, 120)), availableColors[Game1.okColors.BLACK])); //Right wall

            barriers.Add(new Barrier(new Rectangle(new Point(700, 300), new Point(100, 20)), availableColors[Game1.okColors.WHITE])); //White end floor
            barriers.Add(new Barrier(new Rectangle(new Point(700, 300), new Point(100, 20)), availableColors[Game1.okColors.ORANGE])); //Floor
            colorBlobs.Add(new ColorBlob(new Rectangle(new Point(730, 250), new Point(20, 20)), availableColors[Game1.okColors.WHITE], Game1.okColors.WHITE));
            barriers.Add(new Barrier(new Rectangle(new Point(700, 200), new Point(100, 20)), availableColors[Game1.okColors.ORANGE])); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(680, 200), new Point(20, 120)), availableColors[Game1.okColors.ORANGE])); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(780, 200), new Point(20, 120)), availableColors[Game1.okColors.ORANGE])); //Right wall

            barriers.Add(new Barrier(new Rectangle(new Point(300, 600), new Point(500, 20)), availableColors[Game1.okColors.WHITE])); //Bottom Line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 500), new Point(100, 20)), availableColors[Game1.okColors.WHITE])); //White help line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 400), new Point(100, 20)), availableColors[Game1.okColors.BLACK])); //Black help line
            colorBlobs.Add(new ColorBlob(new Rectangle(new Point(540, 370), new Point(20, 20)), availableColors[Game1.okColors.ORANGE], Game1.okColors.ORANGE));

        }
    }
}
