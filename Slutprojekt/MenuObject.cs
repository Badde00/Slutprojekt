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
        protected int id; //Om jag vill identifiera ett specifikt objekt


        public Texture2D Tex
        {
            get { return tex; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public int Id
        {
            get { return id; }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, hitbox, Color.White);
        }

        public virtual void Update() { }
    }
}
