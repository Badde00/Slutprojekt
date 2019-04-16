using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    public abstract class BaseUnit
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Rectangle hitbox;
        protected string description;


        public string Description
        {
            get { return description; }
        }

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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, hitbox, Color.White);
        }

        public virtual void Update() { }
    }
}
