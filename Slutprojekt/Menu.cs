using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Menu : BaseMenu
    {


        public Menu(Texture2D t, List<MenuObject> objects)
        {
            tex = t;
            menuObjects = objects;


        }

        public override void Update()
        {
            foreach(MenuObject o in menuObjects)
            {
                o.Update();
            }
        }
    }
}
