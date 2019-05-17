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
    class MenuObjectButton : MenuObject, ICollision, IClick
    {
        private MouseState previousState;
        private MouseState state;
        private Func func;

        public bool Collide(Rectangle h)
        {
            return hitbox.Intersects(h) ? true : false;
        }

        public bool Collide(Vector2 p)
        {
            return hitbox.Contains(p) ? true : false;
        }

        public bool Collide(Point p)
        {
            return hitbox.Contains(p) ? true : false;
        }

        public bool Click(MouseState state, MouseState previousState)
        {
            return state.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed ? true : false;
        }

        public override void Update()
        {
            state = Mouse.GetState();

            
            if(Collide(state.Position) && Click(state, previousState))
            {
                OnClick();
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
