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
            get;
            private set;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Vector2(0, 0), Color.White);
        }

        public virtual void Update()
        {

        }
    }
}
