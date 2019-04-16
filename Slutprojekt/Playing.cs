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

    public enum PlayingState
    {
        start,
        playing,
        ended,
        other
    }

    public enum SelectedTower //När du placerar torn, så kommer du att välja ett och då ändras denna
    {
        Empty,
        Tower1,
        Tower2
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
        private static bool firstESpawned = false; //Eftersom jag avslutar rundan när det inte finns fiender så använder jag även denna för att kolla att jag inte avslutar innan första fienden
        private static List<BaseUnit> unitsWhenPlaying = new List<BaseUnit>();
        private static KeyboardState keyboardState; //Gör separata keyboardstate från Game1 då det blir mindre att skriva och prestanda och effektiv programmering inte är det jag fokuserar på
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static MouseState mouseState;
        private static MouseState previousMouseState;
        private static Texture2D tex;
        private static GraphicsDeviceManager graphics;
        private static double spawnTime = 0;
        //private static GameTime gameTime = new GameTime();
        private static List<BaseUnit> temp = new List<BaseUnit>();
        private static List<PartialMenu> menuList = new List<PartialMenu>();
        private static Queue<EnemySpawner> enemySpawners = new Queue<EnemySpawner>();
        private static PlayingState pState = new PlayingState();

        private static List<MenuObject> tempList = new List<MenuObject>();


        private static List<Vector2> enemiesTurningPoints1 = new List<Vector2>(); //Vart fiender ska gå i bana 1
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
            pState = PlayingState.start;
            if(s == SelectedTrack.Level1)
            {
                tex = Assets.Bana1;
                tPoints = enemiesTurningPoints1;
            } else
            {
                tex = Assets.Bana2;
                tPoints = enemiesTurningPoints1;
            }

            menuList.Add(new PartialMenu(new List<MenuObject>(), Assets.Blank));
            MakeNTurnButton();
            MakeETP();
        }

        public static void ContiniuePlaying()
        {

        }


        public static void Update(GameTime gameTime) //Jag tar gameTime från Game1 då det inte uppdaterar när jag gör en i Playing. Måste kolla.
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if(pState == PlayingState.playing)
            {
                RemoveDead();

                spawnTime += gameTime.ElapsedGameTime.TotalSeconds;
                

                if (spawnTime >= enemySpawners.Peek().time)
                {
                    SpawnEnemy();
                }
            }

            PlaceTowers();

            foreach(PartialMenu p in menuList)
                p.Update();

            foreach (BaseUnit u in unitsWhenPlaying)
                u.Update();



            //Temporärt, till jag eller Tim fixar StartButton-bug
            if(keyboardState.IsKeyUp(Keys.U) && previousKeyboardState.IsKeyDown(Keys.U))
            {
                StartRound();
            }



            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
        }
        
        public static void StartRound()
        {
            round++;
            pState = PlayingState.playing;
            for (int i = 0; i <= menuList[0].MenuObjects.Count - 1; i++)
            {
                if (menuList[0].MenuObjects[i] is MenuObjectButton)
                {
                    if ((menuList[0].MenuObjects[i] as MenuObjectButton).Id == 123)
                    {
                        menuList[0].MenuObjects.RemoveAt(i);
                    }
                }
            }

            //Innehåller det som ska spawnas under rundor, if(round==1) etc. Jag gjorde if(true) så jag kan förminska allt.
            if (true)
            {
                if (round == 1)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 2)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 3)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 4)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 5)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 6)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 7)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 8)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 9)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round == 10)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round > 10)
                {
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1, new Enemy1(round, Assets.Enemy1, tPoints));
                }
                else if (round < 1)
                {
                    Game1.Game.Exit();
                }
            }
        }

        public static void AddEnemy(double t, BaseEnemy e)
        {
            enemySpawners.Enqueue(new EnemySpawner(e, t));
        }

        public static void SpawnEnemy()
        {
            spawnTime = 0;
            if (!firstESpawned)
                firstESpawned = true;
            unitsWhenPlaying.Add(enemySpawners.Dequeue().enemyToSpawn);
            enemyCount++;
        }

        public static void RemoveDead()
        {
            if (unitsWhenPlaying.Count > 0)
            {
                foreach (BaseUnit u in unitsWhenPlaying) //Rensar min units lista från döda fiender.
                {
                    if (u is BaseEnemy)
                    {
                        if (!(u as BaseEnemy).IsDead)
                        {
                            temp.Add(u); //temp är listan av det jag ska behålla. 
                            enemyCount--;
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

            if (enemyCount == 0 && firstESpawned)
                EndTurn();
        }

        public static void PlaceTowers()
        {
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
                money -= t1U0Cost;
                unitsWhenPlaying.Add(new T1U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                Game1.Game.selectedTower = SelectedTower.Empty;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && Game1.Game.selectedTower == SelectedTower.Tower2) //Placera torn
            {
                money -= t2U0Cost;
                unitsWhenPlaying.Add(new T2U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                Game1.Game.selectedTower = SelectedTower.Empty;
            }
        }

        public static void EndTurn()
        {
            pState = PlayingState.ended;
            firstESpawned = false;
            MakeNTurnButton();
            money += 100 + round * round * 10;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);

            foreach (PartialMenu p in menuList)
            { p.Draw(spriteBatch, graphics); }

            foreach (BaseUnit u in unitsWhenPlaying)
            { u.Draw(spriteBatch); }
        }

        public static void MakeNTurnButton()
        {
            menuList[0].MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(
                graphics.GraphicsDevice.Viewport.Width - 265, graphics.GraphicsDevice.Viewport.Height - 152, 230, 150), StartRound, 123));
            menuList[0].MenuObjects.Add(new MenuObjectText("Next Round", new Vector2(graphics.GraphicsDevice.Viewport.Width - 250, graphics.GraphicsDevice.Viewport.Height - 100), 123));
        }

        public static void MakeETP()
        {
            enemiesTurningPoints1.Add(new Vector2(392, 437));
            enemiesTurningPoints1.Add(new Vector2(407, 242));
            enemiesTurningPoints1.Add(new Vector2(558, 262));
            enemiesTurningPoints1.Add(new Vector2(679, 295));
            enemiesTurningPoints1.Add(new Vector2(740, 267));
            enemiesTurningPoints1.Add(new Vector2(760, 194));
            enemiesTurningPoints1.Add(new Vector2(740, 115));
            enemiesTurningPoints1.Add(new Vector2(694, 95));
            enemiesTurningPoints1.Add(new Vector2(503, 78));
            enemiesTurningPoints1.Add(new Vector2(303, 138));
            enemiesTurningPoints1.Add(new Vector2(265, 203));
            enemiesTurningPoints1.Add(new Vector2(239, 379));
            enemiesTurningPoints1.Add(new Vector2(154, 267));
            enemiesTurningPoints1.Add(new Vector2(192, 53));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));

        }


        public static int Life
        {
            get;
            set;
        }
    }
}
