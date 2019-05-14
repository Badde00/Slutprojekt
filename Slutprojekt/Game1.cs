using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        Level1,
        Level2
    }

    delegate void Func();

    /*För ev cirkel+collision, fråga tim först:
     https://stackoverflow.com/questions/24559585/how-to-create-a-circle-variable-in-monogame-and-detect-collision-with-other-circ
     */

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


            /*var client = new RestClient("api.openweathermap.org/data/2.5/forecast?id=524901&APPID=af8632ef1bec2c349fa2f2902007786b");
            var request = new RestRequest("/", Method.GET);
            IRestResponse response = client.Execute(request);
            String content = response.Content;
            WeatherMain weatherMain = JsonConvert.DeserializeObject<WeatherMain>(content);*/



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
            } else if(gameState == GameState.Playing)
            {
                Playing.Draw(spriteBatch);
            }
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
            menuObjectsList.Add(new MenuObjectButton(Assets.Settings, new Rectangle(graphics.GraphicsDevice.Viewport.Width - 100, 
                graphics.GraphicsDevice.Viewport.Height - 100, 100, 100), OpenSettings)); //Settings
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

        private void LoadGame() //Gör senare
        {

        }

        private void OpenAbout() //Gör senare
        {

        }

        private void OpenSettings() //Gör senare
        {

        }

        

        private void Save()
        {
            /*
             Kommer behöva: Map, runda, liv, pengar, Torn<>(positioner(x,y), texNamn, dmgCaused, projectileTex)
             */
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
    }

    public class WeatherMain
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class Rain
    {
        public double __invalid_name__3h { get; set; }
    }

    public class List
    {
        public int dt { get; set; }
        public WeatherMain weatherMain { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
        public Rain rain { get; set; }
    }

    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
    }

    public class RootObject
    {
        public string cod { get; set; }
        public double message { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
        public City city { get; set; }
    }
}
