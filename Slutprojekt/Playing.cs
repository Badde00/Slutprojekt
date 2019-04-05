using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    static class Playing //Gjorde denna klass för att tömma på Game1. Allt spelande kommer att pågå i denna klass
    {
        private static int round = 0;
        private static int life = 100;
        private static int money = 1000;
        private static int t1U0Cost = 450;
        private static int t2U0Cost = 600;
        private static List<BaseUnit> unitsWhenPlaying = new List<BaseUnit>();
        private static KeyboardState keyboardState; //Gör separata keyboardstate från Game1 då det blir mindre att skriva och prestanda och effektiv programmering inte är det jag fokuserar på
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static MouseState mouseState;
        private static MouseState previousMouseState = Mouse.GetState();
        private static Texture2D tex;
        private static GraphicsDeviceManager graphics;


        private static List<Vector2> enemiesTurningPoints1 = new List<Vector2>(); //Vart fiender ska gå i bana 1 - Måste fixa
        private static List<Vector2> enemiesTurningPoints2 = new List<Vector2>(); //Vart fiender ska gå i bana 2
        private static List<Vector2> tPoints;

        public static List<BaseUnit> UnitsWhenPlaying
        {
            get;
            set;
        }

        public static void StartPlaying(SelectedTrack s, GraphicsDeviceManager g)
        {
            graphics = g;
            if(s == SelectedTrack.Level1)
            {
                tex = Assets.Bana1;
                tPoints = enemiesTurningPoints1;
            } else
            {
                tex = Assets.Bana2;
                tPoints = enemiesTurningPoints1;
            }
        }

        public static void ContiniuePlaying()
        {

        }

        public static void Update()
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //Måste på ngt sätt se om runa är aktiv och starta nya rundor. 


            foreach (BaseUnit u in unitsWhenPlaying)
                u.Update();

            if (keyboardState.IsKeyUp(Keys.D1) && keyboardState.IsKeyDown(Keys.D1) && money >= t1U0Cost)
            {
                Game1.Game.selectedTower = SelectedTower.Tower1;
            }

            if (keyboardState.IsKeyUp(Keys.D2) && keyboardState.IsKeyDown(Keys.D2) && money >= t2U0Cost)
            {
                Game1.Game.selectedTower = SelectedTower.Tower2;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && Game1.Game.selectedTower == SelectedTower.Tower1) //Placera torn
            {
                PlaceTower(SelectedTower.Tower1);
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && Game1.Game.selectedTower == SelectedTower.Tower2) //Placera torn
            {
                PlaceTower(SelectedTower.Tower2);
            }


            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
        }

        public static void PlaceTower(SelectedTower s)
        {
            if (s == SelectedTower.Tower1)
            {
                money -= t1U0Cost;
                unitsWhenPlaying.Add(new T1U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                Game1.Game.selectedTower = SelectedTower.Empty;
            }
            else
            {
                money -= t2U0Cost;
                unitsWhenPlaying.Add(new T2U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                Game1.Game.selectedTower = SelectedTower.Empty;
            }
        }
        
        public static void StartRound()
        {
            round++;
            if(round == 1)
            {
                SpawnEnemy1();

            } else if(round == 2)
            {

            }
            else if (round == 3)
            {

            }
            else if (round == 4)
            {

            }
            else if (round == 5)
            {

            }
            else if (round == 6)
            {

            }
            else if (round == 7)
            {

            }
            else if (round == 8)
            {

            }
            else if (round == 9)
            {

            }
            else if (round == 10)
            {

            }
            else if (round > 10)
            {

            }
            else if (round < 1)
            {

            }
        }

        public static void SpawnEnemy1()
        {
            unitsWhenPlaying.Add(new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(BaseUnit u in unitsWhenPlaying)
            {
                u.Draw(spriteBatch);
            }
            spriteBatch.Draw(tex, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        }


        public static int Life
        {
            get;
            set;
        }
    }
}
