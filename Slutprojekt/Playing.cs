﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    /*Att fixa:
     * Fiender ger ej pengar när de dör
     */

     /*Att göra:
      * Ej placera på bana
      * Klicka på och välja 
      * Bana 2
      * Torn 2
      * Torn uppgraderingar
      * Fiende 2
      */

    public enum PlayingState //Vilken state det spelande spelet är, för vad som ska uppdateras
    {
        start,
        playing,
        paused,
        ended
    }

    public enum SelectedTower //När du ska placera torn, innan du gjort det, så kommer du att välja ett och då ändras denna
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

        private static int round;
        private static int life;
        private static int money;
        private static int t1U0Cost; //t1U0 == tower 1, upgrade 0
        private static int t2U0Cost;
        private static int enemyCount;
        private static bool firstESpawned; //Eftersom jag avslutar rundan när det inte finns fiender så använder jag även denna för att kolla att jag inte avslutar innan första fienden
        private static List<BaseUnit> unitsWhenPlaying; //Listan med torn, fienden och projektiler, för uppdatering
        private static KeyboardState keyboardState; //Gör separata keyboardstate från Game1 då det blir mindre att skriva och prestanda och effektiv programmering inte är det jag fokuserar på
        private static KeyboardState previousKeyboardState;
        private static MouseState mouseState;
        private static MouseState previousMouseState;
        private static Texture2D tex; //Banans bakgrund
        private static GraphicsDeviceManager graphics;
        private static double spawnTime; //Tiden mellan varje fiende
        private static List<BaseUnit> temp; //När jag tömmer unitsWhenPlaying, så jag inte ändrar i en foreach
        private static List<PartialMenu> menuList; //Alla menyer i playing, så de inte grupperas med de i Game1
        private static Queue<EnemySpawner> enemySpawners; //Lägger fiender som ska spawnas i denna kö, så jag kan spawna dem en i taget
        private static PlayingState pState;
        private static SelectedTower selectedTower;
        private static List<BaseTower> shootingTowers; //Jag kan inte skuta när jag uppdaterar alla unitsWhenPlaying, så jag gjorde denna listan och gör det efter
        private static BaseTower chosenT; //När jag har valt ett torn genom att klicka på det. För uppgradering och info om torn
        private static PartialMenu lostMenu; //Ska bara ha en och om jag gör den här så kan jag använda list.Remove(lostMenu);
        private static SelectedTrack selectedTrack; //För då jag ska starta om, så jag vet vilken bana
        private static bool mouseOverTower;
        private static bool mouseOverTrack;
        private static List<Line> lineList;
        

        private static List<Vector2> enemiesTurningPoints1; //Vart fiender ska gå i bana 1
        private static List<Vector2> enemiesTurningPoints2; //Vart fiender ska gå i bana 2
        private static List<Vector2> tPoints; //Den jag ska använda



        public static void StartPlaying(SelectedTrack s, GraphicsDeviceManager g) //Nytt Game
        {
            graphics = g; //Tar grafiken från Game1
            pState = PlayingState.start;
            enemiesTurningPoints1 = new List<Vector2>();
            enemiesTurningPoints2 = new List<Vector2>();
            menuList = new List<PartialMenu>();
            MakeETP(); //Fyller enemiesTurningPoints 1&2
            lineList = new List<Line>();

            if (s == SelectedTrack.Level1) //Den valda banan från Game1 startmenyn väljer bakgrundsbild och väljer vart fiender ska gå
            {
                tex = Assets.Bana1;
                tPoints = enemiesTurningPoints1;
            } else
            {
                tex = Assets.Bana2;
                tPoints = enemiesTurningPoints2;
            }
            MakeTrackLines();

            selectedTower = SelectedTower.Empty;
            menuList.Add(new PartialMenu(new List<MenuObject>(), Assets.Blank)); //Den första listan ska användas till alla vanliga knappar
            MakeNTurnButton(); //Knappen för ny runda

            round = 0;
            life = 100;
            money = 1000;
            t1U0Cost = 450;
            t2U0Cost = 600;
            enemyCount = 0;
            firstESpawned = false;
            unitsWhenPlaying = new List<BaseUnit>();
            previousKeyboardState = Keyboard.GetState();
            spawnTime = 0;
            temp = new List<BaseUnit>();
            enemySpawners = new Queue<EnemySpawner>();
            pState = new PlayingState();
            selectedTower = new SelectedTower();
            shootingTowers = new List<BaseTower>();
            lostMenu = new PartialMenu(new List<MenuObject>(), Assets.PartialMenu, new Vector2(200, 120), new Rectangle(200, 120, 400, 240));
            mouseOverTower = false;
            mouseOverTrack = false;
        }

        public static void ContiniuePlaying() //Load Game
        {

        }


        public static void Update(GameTime gameTime) //Jag tar gameTime från Game1 då det inte uppdaterar när jag gör en i Playing. Måste kolla.
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if(pState == PlayingState.playing) //Det som endast ska hända när jag aktivt spelar, inte pausat etc
            {
                RemoveDead(); //Tar bort döda fiender och projektiler samt avslutar rundan

                spawnTime += gameTime.ElapsedGameTime.TotalSeconds; //Räknar tiden mellan att fiender spawnar
                

                if (enemySpawners.Count > 0 && spawnTime >= enemySpawners.Peek().time) //Spawnar nya fiender efter en tid
                {
                    SpawnEnemy();
                }

                if (life <= 0)
                    YouLose();

                if (enemyCount == 0 && firstESpawned)
                EndTurn();

                foreach (BaseUnit u in unitsWhenPlaying) //Uppd. alla units
                {
                    u.Update();

                    if (u is BaseTower && (u as BaseTower).WillShoot) //Kan inte skjuta i updatera då det ändrar listan (torn å bullets i samma). Min lösning
                    {
                        shootingTowers.Add(u as BaseTower);
                    }
                }
            }

            PlaceTowers(); //Känner om du har valt ett torn och om du klickar för att placera det

            foreach(PartialMenu p in menuList) //Uppd. alla menyer
                p.Update();

            

            foreach(BaseTower t in shootingTowers) //Skjuter
            {
                t.Shoot();
            }
            shootingTowers.Clear();


            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
        }
        
        public static void StartRound()
        {
            List<MenuObject> t = new List<MenuObject>(); //som temp, men för menyer. används för att ta bort bara nextRound knappen
            round++;
            pState = PlayingState.playing;
            for (int i = 0; i <= menuList[0].MenuObjects.Count - 1; i++)
            {
                if (menuList[0].MenuObjects[i].Id != 123) //Nextroundknappen och dens text är de enda med detta id
                {
                    t.Add(menuList[0].MenuObjects[i]);
                }
            }
            menuList[0].MenuObjects = t;

            //Innehåller det som ska spawnas under rundor, if(round==1) etc. Jag gjorde if(true) så jag kan förminska allt. Innehåller alla fiender för de 10 första rundorna
            if (true)
            {
                if (round == 1)
                {
                    AddEnemy(0.5, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(1.5, new Enemy1(round, Assets.Enemy1, tPoints));
                    AddEnemy(0.5, new Enemy1(round, Assets.Enemy1, tPoints));
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
            if (!firstESpawned) //Rundan avslutas när det inte finns fiender kvar. Detta är så att rundor inte ska avslutas innan de börjats
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
                        }
                        else
                        {
                            enemyCount--;
                            money += (u as BaseEnemy).Gold;
                        }
                    }
                    else if(u is Projectile)
                    {
                        if (!(u as Projectile).IsDead)
                            temp.Add(u);
                    }
                    else
                    {
                        temp.Add(u);
                    }
                }
                
            }
            unitsWhenPlaying.Clear();
            foreach(BaseUnit u in temp) //Gör på detta sätt eftersom temp.Add(u) lade till u i unitsWhenPlaying också när jag bara hade unitsWhenPlaying = temp
            {
                unitsWhenPlaying.Add(u);
            }
            temp.Clear();
        }

        public static void PlaceTowers()
        {
            CheckMouseOverTower();
            CheckMouseOverTrack();

            if (keyboardState.IsKeyUp(Keys.D1) && previousKeyboardState.IsKeyDown(Keys.D1) && money >= t1U0Cost) //Väljer torn. I draw så kollar den vilket torn som är valt och ritar det vi musen
            {
                selectedTower = SelectedTower.Tower1;
            }

            if (keyboardState.IsKeyUp(Keys.D2) && previousKeyboardState.IsKeyDown(Keys.D2) && money >= t2U0Cost)
            {
                selectedTower = SelectedTower.Tower2;
            }

            if(selectedTower != SelectedTower.Empty && keyboardState.IsKeyUp(Keys.Back) && previousKeyboardState.IsKeyDown(Keys.Back)) //För att sluta placera torn 
            {
                selectedTower = SelectedTower.Empty;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower1 && !mouseOverTower && !mouseOverTrack) //Placera torn
            {
                money -= t1U0Cost;
                unitsWhenPlaying.Add(new T1U0(new Vector2(mouseState.X, mouseState.Y)));
                selectedTower = SelectedTower.Empty;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower2 && !mouseOverTower && !mouseOverTrack) //Placera torn
            {
                money -= t2U0Cost;
                unitsWhenPlaying.Add(new T2U0(new Vector2(mouseState.Position.X, mouseState.Position.Y)));
                selectedTower = SelectedTower.Empty;
            }
        }

        public static void EndTurn()
        {
            pState = PlayingState.ended;
            firstESpawned = false;
            MakeNTurnButton();
            money += 100 + round * round * 10;

            foreach(BaseUnit u in unitsWhenPlaying)
            {
                if (!(u is Projectile))
                    temp.Add(u);
            }
            unitsWhenPlaying.Clear();
            foreach (BaseUnit u in temp)
                unitsWhenPlaying.Add(u);
            temp.Clear();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);

            foreach (PartialMenu p in menuList)
            { p.Draw(spriteBatch, graphics); }

            foreach (BaseUnit u in unitsWhenPlaying)
            { u.Draw(spriteBatch); }

            if (selectedTower != SelectedTower.Empty)
            {
                MouseDraw(spriteBatch);
            }

            spriteBatch.DrawString(Assets.Text, "Life: " + life.ToString(), new Vector2(10, 70), Color.Black);
            spriteBatch.DrawString(Assets.Text, "$ " + money.ToString(), new Vector2(10, 130), Color.Black);
        }

        private static void MouseDraw(SpriteBatch spriteBatch)
        {
            CheckMouseOverTower();
            CheckMouseOverTrack();

            if (selectedTower == SelectedTower.Tower1 && !mouseOverTower)
            {
                spriteBatch.Draw(Assets.T1U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), Color.White);
            }
            else if(selectedTower == SelectedTower.Tower2 && !mouseOverTower)
            {
                spriteBatch.Draw(Assets.T2U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), Color.White);
            }
            else if(selectedTower == SelectedTower.Tower1 && mouseOverTower)
            {
                spriteBatch.Draw(Assets.T1U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), new Color(Color.Red, 0.5f));
            }
            else
            {
                spriteBatch.Draw(Assets.T2U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), new Color(Color.Red, 0.5f));
            }
        }

        private static void CheckMouseOverTower()
        {
            foreach (BaseUnit u in unitsWhenPlaying)
            {
                if (u is BaseTower)
                {
                    if (u.Hitbox.Intersects(new Rectangle(mouseState.X - 10, mouseState.Y - 10, 70, 70)))
                    {
                        mouseOverTower = true;
                        break;
                    }
                    else
                    {
                        mouseOverTower = false;
                    }
                }
            }
        }

        private static void CheckMouseOverTrack()
        {
            foreach (Line l in lineList)
            {
                if (l.Intersect(new Rectangle(mouseState.X - 10, mouseState.Y - 10, 70, 70)))
                {
                    mouseOverTrack = true;
                    break;
                }
            }
        }

        private static void MouseOverTrack()
        {
            Line line;

            foreach(Vector2 point in tPoints)
            {
                if (tPoints.IndexOf(point) + 1 != tPoints.Count)
                {
                    line = new Line(point, tPoints[tPoints.IndexOf(point) + 1]);
                }
                else
                    line = new Line();
                
            }
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
            enemiesTurningPoints1.Add(new Vector2(750, 267));
            enemiesTurningPoints1.Add(new Vector2(780, 194));
            enemiesTurningPoints1.Add(new Vector2(750, 115));
            enemiesTurningPoints1.Add(new Vector2(694, 95));
            enemiesTurningPoints1.Add(new Vector2(503, 78));
            enemiesTurningPoints1.Add(new Vector2(303, 138));
            enemiesTurningPoints1.Add(new Vector2(265, 203));
            enemiesTurningPoints1.Add(new Vector2(239, 379));
            enemiesTurningPoints1.Add(new Vector2(154, 363));
            enemiesTurningPoints1.Add(new Vector2(192, 53));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));
            enemiesTurningPoints2.Add(new Vector2(1, 1));

        }

        public static void MakeTrackLines()
        {
            for (int i = 0; i < tPoints.Count() - 1; i++)
            {
                lineList.Add(new Line(tPoints[i], tPoints[i + 1]));
            } 
        }

        public static void YouLose()
        {
            pState = PlayingState.ended;

            unitsWhenPlaying.Clear();

            menuList.Add(lostMenu);
            lostMenu.MenuObjects.Add(new MenuObjectText("YOU LOSE", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 100, 150)));
            lostMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(270, 250, 120, 100), GoToStartMenu)); //Meny
            lostMenu.MenuObjects.Add(new MenuObjectText("Menu", new Vector2(280, 280)));
            if(selectedTrack == SelectedTrack.Level1)
                lostMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(410, 250, 150, 100), Game1.Game.StartLevel1)); //Starta om vid bana 1
            else
                lostMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(410, 250, 150, 100), Game1.Game.StartLevel2)); //Starta om vid bana 2
            lostMenu.MenuObjects.Add(new MenuObjectText("Restart", new Vector2(420, 280)));
            lostMenu.MenuObjects.Add(new MenuObjectButton(Assets.Exit, new Rectangle(560, 100, 60, 60), Game1.Game.Exit)); // Exit
        }

        private static void GoToStartMenu()
        {
            Game1.Game.GameState = GameState.Menu;
        }


        public static int Life
        {
            get { return life; }
            set { life = value; }
        }

        public static List<BaseUnit> UnitsWhenPlaying
        {
            get { return unitsWhenPlaying; }
            set { unitsWhenPlaying = value; }
        }

        public static SelectedTower GetSelectedTower
        {
            get { return selectedTower; }
        }

        public static int Money
        {
            get { return money; }
            set { money = value; }
        }
    }
}
