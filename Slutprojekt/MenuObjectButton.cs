using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slutprojekt
{
    class MenuObjectButton : MenuObject
    {
        private MouseState previousState = Mouse.GetState();
        private MouseState state = Mouse.GetState();
        private Func func;
        private int id;

        public Vector2 MousePos
        {
            get;
            private set;
        }

        public MouseState State
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            private set;
        }

        public override void Update()
        {
            state = Mouse.GetState();

            if (hitbox.Contains(state.Position) && state.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
            {
                OnClick();
            }


            previousState = Mouse.GetState();
        }


        public void OnClick()
        {
            func.Invoke();
        }

        public MenuObjectButton(Texture2D t, Rectangle hitB, Func f)
        {
            func = f;
            tex = t;
            hitbox = hitB;
        }

        public MenuObjectButton(Texture2D t, Rectangle hitB, Func f, int iD)
        {
            func = f;
            tex = t;
            hitbox = hitB;
            id = iD;
        }
    }
}
