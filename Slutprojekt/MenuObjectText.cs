using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class MenuObjectText : MenuObject
    {
        private string text;

        public string Text
        {
            get;
            set;
        }

        public MenuObjectText(string  t, Vector2 position, Rectangle size)
        {
            text = t;
            pos = position;
            hitbox = size;
        }

        public override void Update()
        {

        }
    }
}
