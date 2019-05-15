using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class MenuObjectTex : MenuObject
    {
        public MenuObjectTex(Texture2D t, Rectangle hitB)
        {
            tex = t;
            hitbox = hitB;
        }

        public MenuObjectTex(Texture2D t, Rectangle hitB, int iD)
        {
            tex = t;
            hitbox = hitB;
            id = iD;
        }
    }
}
