using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class BaseMenu
    {
        protected List<MenuObject> menuObjects = new List<MenuObject>();
        protected Texture2D tex;


        public Texture2D Tex
        {
            get { return tex; }
            private set { tex = value; }
        }

        public List<MenuObject> MenuObjects
        {
            get { return menuObjects; }
            set { menuObjects = value; }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager g)
        {
            spriteBatch.Draw(tex, new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height), Color.White);
            foreach(MenuObject i in menuObjects)
            {
                i.Draw(spriteBatch);
            }
        }

        public virtual void Update()
        {

        }
    }
}
