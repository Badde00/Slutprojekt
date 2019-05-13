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
        private Rectangle size;
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
            size = zSize;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager g)
        {
            if (size == new Rectangle(0, 0, 0, 0))
            {
                base.Draw(spriteBatch, g);
            }
            else
            {
                spriteBatch.Draw(tex, size, Color.White);
            }

            foreach (MenuObject i in menuObjects)
            {
                i.Draw(spriteBatch);
            }
        }

        public Rectangle Size
        {
            get { return size; }
        }
    }
}
