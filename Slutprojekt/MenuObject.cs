using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class MenuObject
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Rectangle hitbox;


        public Texture2D Tex
        {
            get;
            private set;
        }

        public Vector2 Pos
        {
            get;
            private set;
        }

        public Rectangle Hitbox
        {
            get;
            private set;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, hitbox, Color.White);
        }

        public virtual void Update() { }
    }
}
