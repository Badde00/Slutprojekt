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
        private MouseState previousState;
        private MouseState state;
        private Func func;


        public MouseState State
        {
            get { return state; }
        }

        

        public override void Update()
        {
            state = Mouse.GetState();

            /*if (hitbox.Contains(state.Position) && state.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
            {
                OnClick();
            }*/

            
            if(hitbox.Contains(state.Position))
            {
               if(previousState.LeftButton == ButtonState.Pressed)
               {
                   if(state.LeftButton == ButtonState.Released)
                   {
                        OnClick();
                   }
               }
            }
            


            previousState = state;
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
