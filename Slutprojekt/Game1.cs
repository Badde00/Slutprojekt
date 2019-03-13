using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

    public enum Menu //Startmeny knappar
    {
        Start,
        Settings,
        About,
        Exit
    }


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
