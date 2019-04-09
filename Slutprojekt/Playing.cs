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
    public enum ChosenE
    {
        e1,
        e2
    }

    static class Playing //Gjorde denna klass för att tömma på Game1. Allt spelande kommer att pågå i denna klass
    {
        struct EnemySpawner
        {
            public BaseEnemy enemyToSpawn;
            public double time;

            public EnemySpawner(BaseEnemy enemyType, double spawnTime)
            {
                enemyToSpawn = enemyType;
                time = spawnTime;
            }
        }

        private static int round = 0;
        private static int life = 100;
        private static int money = 1000;
        private static int t1U0Cost = 450;
        private static int t2U0Cost = 600;
        private static int enemyCount = 0;
        private static List<BaseUnit> unitsWhenPlaying = new List<BaseUnit>();
        private static KeyboardState keyboardState; //Gör separata keyboardstate från Game1 då det blir mindre att skriva och prestanda och effektiv programmering inte är det jag fokuserar på
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static MouseState mouseState;
        private static MouseState previousMouseState = Mouse.GetState();
        private static Texture2D tex;
        private static GraphicsDeviceManager graphics;
        private static double spawnTime = 0;
        private static GameTime gameTime = new GameTime();
        private static List<BaseUnit> temp = new List<BaseUnit>();
        private static List<double> spawnCd = new List<double>();
        private static List<double> tempCd = new List<double>();
        private static List<ChosenE> chosenEs = new List<ChosenE>();
        private static bool roundActive = false;
        private static List<MenuObject> menuTemp = new List<MenuObject>();
        private static PartialMenu nextRoundButton;
        private static List<PartialMenu> menuList = new List<PartialMenu>();
        private static Queue<EnemySpawner> enemySpawners = new Queue<EnemySpawner>();


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
            menuTemp.Add(new MenuObjectButton(Assets.Button, new Rectangle(graphics.GraphicsDevice.Viewport.Width - 100, graphics.GraphicsDevice.Viewport.Height - 100, 100, 50), StartRound));
            menuList.Add(nextRoundButton = new PartialMenu(menuTemp, Assets.Blank));
        }

        public static void ContiniuePlaying()
        {
            nextRoundButton.Update();
        }

        public static void Update()
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            spawnTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (unitsWhenPlaying.Count > 0)
            {
                foreach (BaseUnit u in unitsWhenPlaying) //Rensar min units lista från döda fiender.
                {
                    if (u is BaseEnemy)
                    {
                        if ((u as BaseEnemy).IsDead != true)
                        {
                            temp.Add(u);
                        }
                    }
                    else
                    {
                        temp.Add(u);
                    }
                }
                unitsWhenPlaying = temp;
                temp = null;
            }

            if(!roundActive)
            {
                foreach(PartialMenu p in menuList) { p.Update(); }
            }


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
            spawnTime += gameTime.ElapsedGameTime.TotalSeconds;
            if(round == 1)
            {
                AddEnemy(0, new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
                AddEnemy(0, new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
                AddEnemy(0, new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
                AddEnemy(0, new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
                SpawnEnemy();
            }
            else if(round == 2)
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

        public static void AddEnemy(double t, BaseEnemy e)
        {
            enemySpawners.Enqueue(new EnemySpawner(e, t));
        }

        public static void SpawnEnemy()
        {
            if(spawnTime >= enemySpawners.Peek().time)
            {
                spawnTime = 0;
                unitsWhenPlaying.Add(enemySpawners.Dequeue().enemyToSpawn);
                enemyCount++;
            }
            
            
            /*if (spawnTime >= spawnCd[0])
            {
                for (int i = 1; i < spawnCd.Count - 2; i++)
                {
                    tempCd.Add(spawnCd[i]);
                }
                spawnCd = tempCd;
                tempCd = null;

                if(chosenEs[chosenEs.Count - spawnCd.Count - 1] == ChosenE.e1)
                {
                    enemyCount++;
                    unitsWhenPlaying.Add(new Enemy1(round, Assets.Enemy1, new Rectangle((int)tPoints[0].X, (int)tPoints[0].Y, 50, 50), tPoints));
                } else
                {
                    enemyCount++;
                    unitsWhenPlaying.Add(new Enemy2());
                }
            }*/
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(BaseUnit u in unitsWhenPlaying)
            {
                u.Draw(spriteBatch);
            }
            spriteBatch.Draw(tex, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            foreach (PartialMenu p in menuList)
            { p.Draw(spriteBatch, graphics); }
        }


        public static int Life
        {
            get;
            set;
        }
    }
}
