using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class FrontMenu : BaseMenu
    {
        public FrontMenu(List<MenuObject> objects)
        {
            tex = Assets.FrontMenuTex;
            menuObjects = objects;
        }
    }
}
