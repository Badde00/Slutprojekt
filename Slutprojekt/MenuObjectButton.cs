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
        private Vector2 mousePos;
        private MouseState previousState = Mouse.GetState();
        private MouseState state = Mouse.GetState();
        protected Delegate delegateVar; //Vad knappen ska göra

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

        public override void Update()
        {
            state = Mouse.GetState();
            mousePos.X = state.X;
            mousePos.Y = state.Y;
            if (hitbox.Contains(mousePos) && state.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
            {
                
            }


            previousState = Mouse.GetState();
        }


        public MenuObjectButton(Texture2D t, Vector2 pos, Rectangle hitB, Delegate m)
        {
            delegateVar = m;
        }
    }
}
