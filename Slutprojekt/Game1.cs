using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Slutprojekt
{

    public enum GameState
    {
        Menu,
        Playing
    }

    public enum MenuEnum //Startmeny knappar
    {
        StartL1,
        StartL2,
        Load,
        Settings,
        About,
        Exit
    }

    public enum SelectedTrack //Vilken bana som väljs
    {
        Level1 = 1,
        Level2
    }

    delegate void Func();

    public class Game1 : Game 
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Vector2 position;
        private static Game1 game; //Försöker kopiera hur du gjorde en "singelton" (tror jag det hette)
        
        private FrontMenu menu;
        private double time;
        private GameState gameState;
        MouseState mouseState;
        MouseState previousMouseState;
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;
        public delegate void EmptyTest2(int i); // Vet inte vad denna gör. Testa sen
        private Vector2 loadButton; //Så om man vill flytta knappen så flyttas texten med
        private Vector2 aboutButton;
        private List<MenuObject> menuObjectsList;
        private static int highscore;
        private static StreamReader sr;
        private static int pPoints;
        private static int pLife;
        private static int pMoney;
        private static int pRound;
        private static int pSTrack;
        private static float pX;
        private static float pY;
        private static Vector2 pTPos;
        private static int pDmg;
        private static List<BaseTower> pTowers;


        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = this;
        }

        public static Game1 Game //Försöket till singelton
        {
            get
            {
                if (game == null)
                    game = new Game1();
                return game;
            }
            private set { game = value; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            this.IsMouseVisible = true;
            loadButton = new Vector2(135, 220);
            aboutButton = new Vector2(425, 220);
            menuObjectsList = new List<MenuObject>();
            pTowers = new List<BaseTower>();
            try
            {
                sr = new StreamReader("SlutprojektSave.txt");
                string rad = "";
                if ((rad = sr.ReadLine()) != null)
                    highscore = int.Parse(rad);
                sr.Close();
            }
            catch
            {
                highscore = 0;
            }

            MakeStartMenu();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.LoadContent(Content);
        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            GameStateUpdate(gameTime); //Updaterar bara det som behövs


            previousKeyboardState = keyboardState;
            previousMouseState =mouseState;
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            base.Draw(gameTime);
            if (gameState == GameState.Menu)
            {
                menu.Draw(spriteBatch, graphics);
                spriteBatch.DrawString(Assets.Text, "Highscore: " + highscore, new Vector2(50, 40), Color.Black);
            } else if(gameState == GameState.Playing)
            {
                Playing.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Assets.Text, mouseState.Position.ToString(), new Vector2(200), Color.Black);
            spriteBatch.End();
        }
        

        private void MakeStartMenu()
        {
            gameState = GameState.Menu;

            menuObjectsList.Add(new MenuObjectText("Mitt Spel!", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - 75, 75)));
            menuObjectsList.Add(new MenuObjectButton(Assets.Bana1, new Rectangle(200, 150, 100, 100), StartLevel1)); //Start lvl 1
            menuObjectsList.Add(new MenuObjectButton(Assets.Bana2, new Rectangle(500, 150, 100, 100), StartLevel2)); //Start lvl 2
            menuObjectsList.Add(new MenuObjectButton(Assets.Button, new Rectangle((int)loadButton.X, (int)loadButton.Y, 225, 225), LoadGame)); //Load game
            menuObjectsList.Add(new MenuObjectText("Load Game", new Vector2(loadButton.X + 15, loadButton.Y + 90))); //Text över load
            menuObjectsList.Add(new MenuObjectButton(Assets.Button, new Rectangle((int)aboutButton.X, (int)aboutButton.Y, 225, 225), OpenAbout)); //About
            menuObjectsList.Add(new MenuObjectText("About", new Vector2(aboutButton.X + 50, aboutButton.Y + 90)));
            menuObjectsList.Add(new MenuObjectButton(Assets.Exit, new Rectangle(graphics.GraphicsDevice.Viewport.Width - 100, 0, 100, 100), Exit)); //Exit
            
            menu = new FrontMenu(menuObjectsList);
        }

        private void GameStateUpdate(GameTime gameTime)
        {
            if (gameState == GameState.Menu)
            {
                if (previousKeyboardState.IsKeyDown(Keys.Escape) && keyboardState.IsKeyUp(Keys.Escape))
                    Exit();
                menu.Update();
            }
            else
            if (gameState == GameState.Playing)
            {
                Playing.Update(gameTime);
                time += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void StartLevel1()
        {
            gameState = GameState.Playing;
            Playing.StartPlaying(SelectedTrack.Level1, graphics);
        }

        public void StartLevel2()
        {
            gameState = GameState.Playing;
            Playing.StartPlaying(SelectedTrack.Level2, graphics);
        }

        private void LoadGame()
        {
            try
            {
                sr = new StreamReader("SlutprojektSave.txt");
                string rad = "";
                sr.ReadLine(); //Första raden är highscore
                if ((rad = sr.ReadLine()) != null) //points
                    pPoints = int.Parse(rad);
                if ((rad = sr.ReadLine()) != null) //life
                    pLife = int.Parse(rad);
                if ((rad = sr.ReadLine()) != null) //money
                    pMoney = int.Parse(rad);
                if ((rad = sr.ReadLine()) != null) //round
                    pRound = int.Parse(rad);
                if ((rad = sr.ReadLine()) != null) //selectedTrack (Lvl 1 = 1, Lvl 2 = 2)
                    pSTrack = int.Parse(rad);
                while ((rad = sr.ReadLine()) != null)
                {
                    pX = float.Parse(rad);
                    if ((rad = sr.ReadLine()) != null)
                        pY = float.Parse(rad);
                    if ((rad = sr.ReadLine()) != null)
                        pDmg = int.Parse(rad);
                    if ((rad = sr.ReadLine()) != null)
                    {
                        if(rad == "T1U0")
                        {
                            pTowers.Add(new T1U0(new Vector2(pX, pY), pDmg));
                        }
                        else if (rad == "T1U1")
                        {
                            pTowers.Add(new T1U1(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T1U2")
                        {
                            pTowers.Add(new T1U2(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T1U3")
                        {
                            pTowers.Add(new T1U3(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T2U0")
                        {
                            pTowers.Add(new T2U0(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T2U1")
                        {
                            pTowers.Add(new T2U1(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T2U2")
                        {
                            pTowers.Add(new T2U2(new Vector2(pX, pY), pDmg));
                        }
                        else if(rad == "T2U3")
                        {
                            pTowers.Add(new T2U3(new Vector2(pX, pY), pDmg));
                        }
                    }
                }
                    sr.Close();

                if (pSTrack == 1)
                {
                    gameState = GameState.Playing;
                    Playing.ContiniuePlaying(pPoints, pLife, pMoney, pRound, SelectedTrack.Level1, pTowers, graphics);
                }
                else if(pSTrack == 2)
                {
                    gameState = GameState.Playing;
                    Playing.ContiniuePlaying(pPoints, pLife, pMoney, pRound, SelectedTrack.Level2, pTowers, graphics);
                }
            }
            catch
            {

            }
        }

        private void OpenAbout() //Gör senare
        {

        }


        public double Time
        {
            get { return time; }
        }

        public GameState GameState
        {
            get { return GameState; }
            set { gameState = value; }
        }

        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        public int Highscore
        {
            get { return highscore; }
        }
    }

    interface ICollision
    {
        bool Collide(Rectangle hitBox);

        bool Collide(Vector2 pos);

        bool Collide(Point pos);
    }
}
