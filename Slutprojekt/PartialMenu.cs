using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class PartialMenu : BaseMenu
    {
        private Rectangle hitbox;
        private Vector2 pos;

        public PartialMenu(List<MenuObject> objects, Texture2D zTex)
        {
            tex = zTex;
            menuObjects = objects;
        }

        public PartialMenu(List<MenuObject> objects, Texture2D zTex, Vector2 zPos, Rectangle zSize)
        {
            tex = zTex;
            menuObjects = objects;
            pos = zPos;
            hitbox = zSize;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager g)
        {
            if (hitbox == new Rectangle(0, 0, 0, 0))
            {
                base.Draw(spriteBatch, g);
            }
            else
            {
                spriteBatch.Draw(tex, hitbox, Color.White);
            }

            foreach (MenuObject i in menuObjects)
            {
                i.Draw(spriteBatch);
            }
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

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }
    }
}
