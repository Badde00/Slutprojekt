using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private SpriteFont spriteFont;

        public string Text
        {
            get;
            set;
        }

        public MenuObjectText(string  t, Vector2 position)
        {
            text = t;
            pos = position;
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Assets.Text, text, pos, Color.Black);
        }
    }
}
