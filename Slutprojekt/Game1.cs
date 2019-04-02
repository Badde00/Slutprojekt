using Microsoft.Xna.Framework;
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
        Paused, 
        Defeat
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

    public enum SelectedTower //När du placerar torn, så kommer du att välja ett och då ändras denna
    {
        Empty,
        Tower1,
        Tower2
    }

    delegate void Func();

    /*För ev cirkel+collision, fråga tim först:
     https://stackoverflow.com/questions/24559585/how-to-create-a-circle-variable-in-monogame-and-detect-collision-with-other-circ
     */

    public class Game1 : Game 
    {
        /*KOM IHÅG:
         * Gör rektanglar i TxU0
         */

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Vector2 position;
        private static Game1 game; //Försöker kopiera hur du gjorde en "singelton" (tror jag det hette)
        
        private int t1U0Cost;
        private int t2U0Cost;
        private List<BaseUnit> unitsWhenPlaying;
        private FrontMenu menu;
        private double time;
        private GameState gameState;
        private SelectedTower selectedTower;
        MouseState mouseState;
        MouseState previousMouseState;
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;
        public delegate void EmptyTest2(int i);
        private Vector2 loadButton; //Så om man vill flytta knappen så flyttas texten med
        private Vector2 aboutButton;
        private static Playing currentGame;


        public double Time
        {
            get;
            private set;
        }

        public GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }

        public List<BaseUnit> UnitsWhenPlaying
        {
            get;
            set;
        }

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
            unitsWhenPlaying = new List<BaseUnit>();
            selectedTower = SelectedTower.Empty;
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
            t1U0Cost = 400;
            t2U0Cost = 650;
            loadButton = new Vector2(135, 220);
            aboutButton = new Vector2(425, 220);


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

            if (gameState == GameState.Menu)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                menu.Update();
            }else 
            if(gameState == GameState.Paused)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gameState = GameState.Playing;
            } else 
            if(gameState == GameState.Playing)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gameState = GameState.Paused;
                currentGame.Update();
                time += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else //Settings
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gameState = GameState.Menu;
            }

            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            base.Draw(gameTime);
            menu.Draw(spriteBatch, graphics);
            spriteBatch.End();
        }

        private void MakeStartMenu() //Inte klar. Måste fixa knappar först
        {
            gameState = GameState.Menu;
            List<MenuObject> menuObjectsList = new List<MenuObject>();

            menuObjectsList.Add(new MenuObjectText("Mitt Spel!", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - 75, 75)));
            menuObjectsList.Add(new MenuObjectButton(Assets.Bana1, new Rectangle(200, 150, 100, 100), StartLevel1)); //Start lvl 1
            menuObjectsList.Add(new MenuObjectButton(Assets.Bana2, new Rectangle(500, 150, 100, 100), StartLevel2)); //Start lvl 2
            menuObjectsList.Add(new MenuObjectButton(Assets.Button, new Rectangle((int)loadButton.X, (int)loadButton.Y, 225, 225), LoadGame)); //Load game
            menuObjectsList.Add(new MenuObjectText("Load Game", new Vector2(loadButton.X + 15, loadButton.Y + 90))); //Text över load
            menuObjectsList.Add(new MenuObjectButton(Assets.Button, new Rectangle((int)aboutButton.X, (int)aboutButton.Y, 225, 225), OpenAbout)); //About
            menuObjectsList.Add(new MenuObjectText("About", new Vector2(aboutButton.X + 15, aboutButton.Y + 90)));
            menuObjectsList.Add(new MenuObjectButton(Assets.Bana1, new Rectangle(200, 150, 100, 100), Exit)); //Exit
            menu = new FrontMenu(menuObjectsList);
        }

        private void StartLevel1()
        {
            gameState = GameState.Playing;

        }

        private void StartLevel2()
        {
            gameState = GameState.Playing;
        }

        private void LoadGame() //Gör senare
        {

        }

        private void OpenAbout() //Gör senare
        {

        }

        private void PlayingUpdate() //Flytta till Playing update
        {
            foreach (BaseUnit u in unitsWhenPlaying)
                u.Update();

            if (keyboardState.IsKeyUp(Keys.D1) && keyboardState.IsKeyDown(Keys.D1) && money >= t1U0Cost)
            {
                selectedTower = SelectedTower.Tower1;
            }

            if (keyboardState.IsKeyUp(Keys.D2) && keyboardState.IsKeyDown(Keys.D2) && money >= t2U0Cost)
            {
                selectedTower = SelectedTower.Tower2;
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower1) //Placera torn
            {
                PlaceTower(SelectedTower.Tower1);
            }

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && selectedTower == SelectedTower.Tower2) //Placera torn
            {
                PlaceTower(SelectedTower.Tower2);
            }
        }

        private void PlaceTower(SelectedTower s)
        {
            if(s == SelectedTower.Tower1)
            {
                money -= t1U0Cost;
                unitsWhenPlaying.Add(new T1U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                selectedTower = SelectedTower.Empty;
            }
            else if(s == SelectedTower.Tower2)
            {
                money -= t2U0Cost;
                unitsWhenPlaying.Add(new T2U0(new Vector2(mouseState.Position.X, mouseState.Position.Y), null));
                selectedTower = SelectedTower.Empty;
            }
        }

        private void Save()
        {
            /*
             Kommer behöva: Map, runda, liv, pengar, Torn<>(positioner(x,y), texNamn, dmgCaused, projectileTex)
             */
        }

        public void EmptyTest(int i)
        {

        }
    }
}
