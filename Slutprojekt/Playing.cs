using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Slutprojekt
{
    /*Att fixa:
     */

    /*Att göra:
     * Kommentarer
     * Api - Senare
     * Generisk klass
     * Generisk metod
     * 
     * Kolla Interface betygnivå
     * Kolla om upgradering är polymorfism
     * 
     * Mindre viktigt/manual labour:
     *     Hur man skickar projektiler med instansen av det som sköt det
     *     Attack mode
     */
     
    //api.openweathermap.org/data/2.5/forecast?id=524901&APPID=af8632ef1bec2c349fa2f2902007786b

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
        private static PartialMenu pausedMenu;
        private static PartialMenu towerSelectedMenu;
        private static SelectedTrack selectedTrack; //För då jag ska starta om, så jag vet vilken bana
        private static bool mouseOverTower;
        private static int points;
        private static bool willUnPause;
        private static bool maxPressed;
        private static bool willMakeTSM;
        

        private static List<Vector2> enemiesTurningPoints1; //Vart fiender ska gå i bana 1
        private static List<Vector2> enemiesTurningPoints2; //Vart fiender ska gå i bana 2
        private static List<Vector2> tPoints; //Den jag ska använda



        public static void StartPlaying(SelectedTrack s, GraphicsDeviceManager g) //Nytt Game
        {
            graphics = g; //Tar grafiken från Game1
            ForStarting();

            if (s == SelectedTrack.Level1) //Den valda banan från Game1 startmenyn väljer bakgrundsbild och väljer vart fiender ska gå
            {
                tex = Assets.Bana1;
                tPoints = enemiesTurningPoints1;
                selectedTrack = s;
            } else
            {
                tex = Assets.Bana2;
                tPoints = enemiesTurningPoints2;
                selectedTrack = s;
            }
            round = 0;
            life = 100;
            money = 100000;
            points = 0;
        }

        public static void ContiniuePlaying(int zPoints, int zLife, int zMoney, int zRound, SelectedTrack s, List<BaseTower> towerList, GraphicsDeviceManager g) //Load Game
        {
            graphics = g; //Tar grafiken från Game1
            ForStarting();

            if (s == SelectedTrack.Level1) //Den valda banan från Game1 startmenyn väljer bakgrundsbild och väljer vart fiender ska gå
            {
                tex = Assets.Bana1;
                tPoints = enemiesTurningPoints1;
                selectedTrack = s;
            }
            else
            {
                tex = Assets.Bana2;
                tPoints = enemiesTurningPoints2;
                selectedTrack = s;
            }
            round = zRound;
            life = zLife;
            money = zMoney;
            points =  zPoints;
            foreach(BaseTower t in towerList)
            {
                unitsWhenPlaying.Add(t);
            }
        }

        public static void ForStarting()
        {
            pState = PlayingState.start;
            enemiesTurningPoints1 = new List<Vector2>();
            enemiesTurningPoints2 = new List<Vector2>();
            menuList = new List<PartialMenu>();
            towerSelectedMenu = new PartialMenu(new List<MenuObject>(), Assets.Blank, new Vector2(0), new Rectangle());
            MakeETP(); //Fyller enemiesTurningPoints 1&2

            selectedTower = SelectedTower.Empty;
            menuList.Add(new PartialMenu(new List<MenuObject>(), Assets.Blank)); //Den första listan ska användas till alla vanliga knappar
            MakeNTurnButton(); //Knappen för ny runda

            pausedMenu = new PartialMenu(new List<MenuObject>(), Assets.PartialMenu, new Vector2(200, 50), new Rectangle(200, 50, 400, 380));
            pausedMenu.MenuObjects.Add(new MenuObjectText("Paused", new Vector2(320, 70)));
            pausedMenu.MenuObjects.Add(new MenuObjectText("Points: " + points.ToString(), new Vector2(310, 150)));
            pausedMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(280, 220, 250, 130), WillUnPause));
            pausedMenu.MenuObjects.Add(new MenuObjectText("UnPause", new Vector2(310, 260)));

            t1U0Cost = 450;
            t2U0Cost = 600;
            enemyCount = 0;
            firstESpawned = false;
            unitsWhenPlaying = new List<BaseUnit>();
            previousKeyboardState = Keyboard.GetState();
            spawnTime = 0;
            temp = new List<BaseUnit>();
            enemySpawners = new Queue<EnemySpawner>();
            selectedTower = new SelectedTower();
            shootingTowers = new List<BaseTower>();
            lostMenu = new PartialMenu(new List<MenuObject>(), Assets.PartialMenu, new Vector2(200, 120), new Rectangle(200, 120, 400, 240));
            mouseOverTower = false;
            willUnPause = false;
            maxPressed = false;
            willMakeTSM = false;
        }

        public static void Update(GameTime gameTime)
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


                if (keyboardState.IsKeyUp(Keys.Escape) && previousKeyboardState.IsKeyDown(Keys.Escape))
                {
                    pState = PlayingState.paused;
                    menuList.Add(pausedMenu);

                }
            } 
            else if(pState == PlayingState.paused)
            {
                if(keyboardState.IsKeyUp(Keys.Escape) && previousKeyboardState.IsKeyDown(Keys.Escape))
                {
                    pState = PlayingState.playing;
                }
            }

            if(pState == PlayingState.playing || pState == PlayingState.start || pState == PlayingState.ended)
            {
                PlaceTowers(); //Känner om du har valt ett torn och om du klickar för att placera det

                UnselectTower();

                SelectTower();
            }

            foreach(PartialMenu p in menuList) //Uppd. alla menyer
                p.Update();
            if(willMakeTSM)
            {
                UnSelect();
                MakeTSM();
                willMakeTSM = false;
            }

            if (willUnPause)
                UnPause();

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
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 2)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 3)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 4)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 5)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy2(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 6)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy2(round, tPoints));
                }
                else if (round == 7)
                {
                    AddEnemy(0.5, new Enemy2(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                }
                else if (round == 8)
                {
                    AddEnemy(0.5, new Enemy2(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy2(round, tPoints));
                }
                else if (round == 9)
                {
                    AddEnemy(0.5, new Enemy2(round, tPoints));
                    AddEnemy(0.3, new Enemy2(round, tPoints));
                    AddEnemy(0.3, new Enemy2(round, tPoints));
                    AddEnemy(0.3, new Enemy2(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(0.7, new Enemy1(round, tPoints));
                }
                else if (round == 10)
                {
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(1, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy2(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                }
                else if (round > 10)
                {
                    AddEnemy(0.5, new Enemy2(round, tPoints));
                    AddEnemy(1.5, new Enemy2(round, tPoints));
                    AddEnemy(0.5, new Enemy1(round, tPoints));
                    AddEnemy(0.3, new Enemy1(round, tPoints));
                    AddEnemy(0.3, new Enemy1(round, tPoints));
                    AddEnemy(2, new Enemy2(round, tPoints));
                    AddEnemy(2, new Enemy2(round, tPoints));
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
                            points += (u as BaseEnemy).DangerLevel;
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

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower1 && !mouseOverTower) //Placera torn
            {
                money -= t1U0Cost;
                unitsWhenPlaying.Add(new T1U0(new Vector2(mouseState.X, mouseState.Y)));
                selectedTower = SelectedTower.Empty;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower2 && !mouseOverTower) //Placera torn
            {
                money -= t2U0Cost;
                unitsWhenPlaying.Add(new T2U0(new Vector2(mouseState.Position.X, mouseState.Position.Y)));
                selectedTower = SelectedTower.Empty;
            }
        }

        public static void SelectTower()
        {
            foreach (BaseUnit u in unitsWhenPlaying)
            {
                if (u is BaseTower)
                {
                    if (u.Collide(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && 
                        !towerSelectedMenu.Collide(mouseState.Position))
                    {
                        chosenT = u as BaseTower;

                        MakeTSM();

                        break;
                    }
                }
            }
        }

        public static void MakeTSM()
        {
            if (chosenT.Pos.X <= graphics.GraphicsDevice.Viewport.Width - 270)
            {
                towerSelectedMenu = new PartialMenu(new List<MenuObject>(), Assets.PartialMenu, new Vector2(0), new Rectangle(graphics.GraphicsDevice.Viewport.Width
                                - 270, 0, 270, graphics.GraphicsDevice.Viewport.Height - 150));

                if (chosenT is T1U0)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 1", new Vector2(575, 15)));
                if (chosenT is T1U1)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 2", new Vector2(575, 15)));
                if (chosenT is T1U2)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 3", new Vector2(575, 15)));
                if (chosenT is T1U3)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, Max", new Vector2(575, 15)));
                if (chosenT is T2U0)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 1", new Vector2(575, 15)));
                if (chosenT is T2U1)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 2", new Vector2(575, 15)));
                if (chosenT is T2U2)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 3", new Vector2(575, 15)));
                if (chosenT is T2U3)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, Max", new Vector2(575, 15)));

                towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Dmg Done: " + (chosenT as BaseTower).DmgCaused, new Vector2(550, 65)));
                towerSelectedMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(565, 110, 200, 110), UpgradeTower));
                towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Upgrade", new Vector2(590, 145)));
            }
            else
            {
                towerSelectedMenu = new PartialMenu(new List<MenuObject>(), Assets.PartialMenu, new Vector2(0), new Rectangle(0, 150, 270, graphics.GraphicsDevice.Viewport.Height - 150));

                if (chosenT is T1U0)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 1", new Vector2(45, 165)));
                if (chosenT is T1U1)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 2", new Vector2(45, 165)));
                if (chosenT is T1U2)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, 3", new Vector2(45, 165)));
                if (chosenT is T1U3)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 1, Max", new Vector2(45, 165)));
                if (chosenT is T2U0)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 1", new Vector2(45, 165)));
                if (chosenT is T2U1)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 2", new Vector2(45, 165)));
                if (chosenT is T2U2)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, 3", new Vector2(45, 165)));
                if (chosenT is T2U3)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Tower 2, Max", new Vector2(45, 165)));

                towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Dmg Done: " + (chosenT as BaseTower).DmgCaused, new Vector2(20, 215)));
                towerSelectedMenu.MenuObjects.Add(new MenuObjectButton(Assets.Button, new Rectangle(15, 260, 200, 110), UpgradeTower));
                towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Upgrade", new Vector2(60, 295)));
            }
            menuList.Add(towerSelectedMenu);
        } //TowerSelectedMenu

        public static void UnselectTower()
        {
            if(previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released && chosenT != null
                && !(towerSelectedMenu.Collide(mouseState.Position) || chosenT.Collide(mouseState.Position + new Point(5)))) //Har klickat och är utanför torn och meny
            {
                chosenT = null;
                UnSelect();
            }
        }

        public static void UnSelect()
        {
            maxPressed = false;
            menuList.Remove(towerSelectedMenu);
            towerSelectedMenu = new PartialMenu(new List<MenuObject>(), Assets.Blank, new Vector2(), new Rectangle());
            towerSelectedMenu.MenuObjects.Clear();
        }

        public static void UpgradeTower()
        {
            if (chosenT is T1U0 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T1U1(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if(chosenT is T1U1 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T1U2(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if (chosenT is T1U2 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T1U3(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if (chosenT is T1U3)
            {
                maxPressed = true;
            }
            else if (chosenT is T2U0 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T2U1(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if (chosenT is T2U1 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T2U2(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if (chosenT is T2U2 && money >= chosenT.UpgradeCost)
            {
                money -= chosenT.UpgradeCost;
                BaseTower b = new T2U3(chosenT.Pos, chosenT.DmgCaused);
                unitsWhenPlaying.Remove(chosenT);
                unitsWhenPlaying.Add(b);
                chosenT = b;
                willMakeTSM = true;
            }
            else if (chosenT is T2U3)
            {
                maxPressed = true;
            }
        }

        public static void ToAMStrong() //To attackmode: Strong
        {
            willMakeTSM = true;
            BaseTower b = chosenT;
            b.Target = AttackMode.strong;
            unitsWhenPlaying.Remove(chosenT);
            unitsWhenPlaying.Add(b);
        }

        public static void ToAMLast()
        {
            willMakeTSM = true;
            BaseTower b = chosenT;
            b.Target = AttackMode.last;
            unitsWhenPlaying.Remove(chosenT);
            unitsWhenPlaying.Add(b);
        }

        public static void ToAMFirst()
        {
            willMakeTSM = true;
            BaseTower b = chosenT;
            b.Target = AttackMode.first;
            unitsWhenPlaying.Remove(chosenT);
            unitsWhenPlaying.Add(b);
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
            if (points > Game1.Game.Highscore)
                Game1.Game.Highscore = points;
            Save();
        }

        public static void Save()
        {
            StreamWriter sw = new StreamWriter("SlutprojektSave.txt");
            sw.WriteLine(Game1.Game.Highscore);
            sw.WriteLine(points);
            sw.WriteLine(life);
            sw.WriteLine(money);
            sw.WriteLine(round);
            sw.WriteLine((int)selectedTrack);
            foreach(BaseUnit b in unitsWhenPlaying)
            {
                if (b is BaseTower)
                {
                    sw.WriteLine(b.Pos.X);
                    sw.WriteLine(b.Pos.Y);
                    sw.WriteLine((b as BaseTower).DmgCaused);
                    if (b is T1U0)
                    {
                        sw.WriteLine("T1U0");
                    }
                    else if (b is T1U1)
                    {
                        sw.WriteLine("T1U1");
                    }
                    else if (b is T1U2)
                    {
                        sw.WriteLine("T1U2");
                    }
                    else if (b is T1U3)
                    {
                        sw.WriteLine("T1U3");
                    }
                    else if (b is T2U0)
                    {
                        sw.WriteLine("T2U0");
                    }
                    else if (b is T2U1)
                    {
                        sw.WriteLine("T2U1");
                    }
                    else if (b is T2U2)
                    {
                        sw.WriteLine("T2U2");
                    }
                    else if (b is T2U3)
                    {
                        sw.WriteLine("T2U3");
                    }
                }
            }
            sw.Close();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);

            foreach (BaseUnit u in unitsWhenPlaying)
            { u.Draw(spriteBatch); }

            if (selectedTower != SelectedTower.Empty)
            { MouseDraw(spriteBatch); }

            if (chosenT != null)
            {
                spriteBatch.Draw(Assets.Circle, new Rectangle((int)chosenT.Pos.X - chosenT.Radius + 25, (int)chosenT.Pos.Y - chosenT.Radius + 25, chosenT.Radius * 2 + 25, chosenT.Radius * 2 + 25), Color.Black);
                if(maxPressed)
                    towerSelectedMenu.MenuObjects.Add(new MenuObjectText("Max Upgrade", new Vector2(290, 80)));
            }

            foreach (PartialMenu p in menuList)
            { p.Draw(spriteBatch, graphics); }

            
            spriteBatch.DrawString(Assets.Text, "Life: " + life.ToString(), new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(Assets.Text, "$ " + money.ToString(), new Vector2(10, 70), Color.Black);
        }

        private static void MouseDraw(SpriteBatch spriteBatch)
        {
            CheckMouseOverTower();

            if (selectedTower == SelectedTower.Tower1 && !mouseOverTower)
            {
                spriteBatch.Draw(Assets.T1U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), Color.White);
            }
            else if(selectedTower == SelectedTower.Tower2 && !mouseOverTower)
            {
                spriteBatch.Draw(Assets.T2U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), Color.White);
            }
            else if(selectedTower == SelectedTower.Tower1)
            {
                spriteBatch.Draw(Assets.T1U0, new Rectangle(mouseState.X, mouseState.Y, 50, 50), new Color(Color.Red, 0.5f));
            }
            else if(selectedTower == SelectedTower.Tower2)
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
                    if (u.Collide(new Rectangle(mouseState.X - 10, mouseState.Y - 10, 70, 70)))
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

            enemiesTurningPoints2.Add(new Vector2(582, 431));
            enemiesTurningPoints2.Add(new Vector2(598, 347));
            enemiesTurningPoints2.Add(new Vector2(713, 345));
            enemiesTurningPoints2.Add(new Vector2(769, 326));
            enemiesTurningPoints2.Add(new Vector2(780, 284));
            enemiesTurningPoints2.Add(new Vector2(732, 238));
            enemiesTurningPoints2.Add(new Vector2(672, 233));
            enemiesTurningPoints2.Add(new Vector2(547, 234));
            enemiesTurningPoints2.Add(new Vector2(541, 157));
            enemiesTurningPoints2.Add(new Vector2(721, 153));
            enemiesTurningPoints2.Add(new Vector2(715, 44));
            enemiesTurningPoints2.Add(new Vector2(85, 43));
            enemiesTurningPoints2.Add(new Vector2(79, 147));
            enemiesTurningPoints2.Add(new Vector2(279, 154));
            enemiesTurningPoints2.Add(new Vector2(369, 164));
            enemiesTurningPoints2.Add(new Vector2(399, 233));
            enemiesTurningPoints2.Add(new Vector2(385, 293));
            enemiesTurningPoints2.Add(new Vector2(336, 318));
            enemiesTurningPoints2.Add(new Vector2(266, 314));
            enemiesTurningPoints2.Add(new Vector2(199, 327));
            enemiesTurningPoints2.Add(new Vector2(184, 393));

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

        private static void WillUnPause()
        {
            willUnPause = true;
        }

        private static void UnPause()
        {
            pState = PlayingState.playing;
            willUnPause = false;
            menuList.Remove(pausedMenu);
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
