using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    public abstract class BaseUnit : ICollision
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Rectangle hitbox;

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

        public bool Collide(Rectangle h)
        {
            return hitbox.Intersects(h) ? true : false;
        }

        public bool Collide(Vector2 p)
        {
            return hitbox.Contains(p) ? true : false;
        }

        public bool Collide(Point p)
        {
            return hitbox.Contains(p) ? true : false;
        }

        public virtual void Update() { }
    }
}
