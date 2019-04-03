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
        public PartialMenu(List<MenuObject> objects, Texture2D zTex)
        {
            tex = zTex;
            menuObjects = objects;
        }

        public override void Update()
        {
            foreach (MenuObject o in menuObjects)
            {
                o.Update();
            }
        }
    }
}
