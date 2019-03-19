﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Slutprojekt
{
    public enum Weather
    {
        Rain, //Gör fiender långsamma
        Sunshine, //Standard
        Thunder, //Passivt skadar fiender
        Snow, //Accelererar fiender, men långsam deacceleration
        Disabled //Vädret avstängt i inställningar
    }

    public enum GameState
    {
        Menu,
        Playing,
        Settings,
        Paused
    }

    public enum MenuEnum //Startmeny knappar
    {
        StartL1,
        StartL2,
        Settings,
        About,
        Exit
    }

    public enum SelectedTower //När du placerar torn, så kommer du att välja ett och då ändras denna
    {
        Empty,
        Tower1,
        Tower2
    }


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Vector2 position;
        private static Game1 game; //Försöker kopiera hur du gjorde en "singelton" (tror jag det hette)

        private Texture2D bana1;
        private Texture2D bana2;
        private Texture2D t1U0;
        private Texture2D t1U1;
        private Texture2D t1U2;
        private Texture2D t1U3;
        private Texture2D t1Projectile;
        private Texture2D t2U0;
        private Texture2D t2U1;
        private Texture2D t2U2;
        private Texture2D t2U3;
        private Texture2D t2Projectile;
        private Texture2D menuTex;
        private int money;
        private int t1U0Cost;
        private int t2U0Cost;
        private List<BaseUnit> unitsWhenPlaing;
        private Menu menu;
        private GameState gameState;
        private SelectedTower selectedTower;
        MouseState mouseState;
        MouseState previousMouseState;
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;
        public delegate void EmptyTest2(int i);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            unitsWhenPlaing = new List<BaseUnit>();
            MakeMenu();
            gameState = GameState.Menu;
            selectedTower = SelectedTower.Empty;
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
            money = 1000;
            t1U0Cost = 400;
            t2U0Cost = 650;
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bana1 = Content.Load<Texture2D>("Bana1");
            bana2 = Content.Load<Texture2D>("Bana2");
            t1U0 = Content.Load<Texture2D>("T1U0");
            t1U1 = Content.Load<Texture2D>("T1U1");
            t1U2 = Content.Load<Texture2D>("T1U2");
            t1U3 = Content.Load<Texture2D>("T1U3");
            t1Projectile = Content.Load<Texture2D>("T1Projectile");
            t2U0 = Content.Load<Texture2D>("T2U0");
            t2U1 = Content.Load<Texture2D>("T2U1");
            t2U2 = Content.Load<Texture2D>("T2U2");
            t2U3 = Content.Load<Texture2D>("T2U3");
            t2Projectile = Content.Load<Texture2D>("T2Projectile");
            menuTex = Content.Load<Texture2D>("Menu");
        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            if (gameState == GameState.Menu)
                menu.Update();

            if (gameState == GameState.Playing)
            {
                PlayingUpdate();
            }


            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        private void MakeMenu() //Inte klar. Måste fixa knappar först
        {
            gameState = GameState.Menu;
            List<MenuObject> menuObjectsList = new List<MenuObject>();
            menuObjectsList.Add(new MenuObjectButton(menuTex, position,  new Rectangle(100, 100, 10, 10), EmptyTest(1)));
            menu = new Menu();
        }

        private void StartLevel()
        {
            gameState = GameState.Playing;

        }

        private void PlayingUpdate()
        {
            foreach (BaseUnit u in unitsWhenPlaing)
                u.Update();

            if (keyboardState.IsKeyUp(Keys.NumPad1) && keyboardState.IsKeyDown(Keys.NumPad1) && money >= t1U0Cost)
            {
                selectedTower = SelectedTower.Tower1;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) //Placera torn
            {

            }
        }

        private void PlaceTower()
        {

        }


        public void EmptyTest(int i)
        {

        }
    }
}
