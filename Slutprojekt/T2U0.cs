using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class T2U0 : Tower2
    {
        public T2U0(Texture2D zTex, Vector2 zPos, Texture2D zProjectileTex)
        {
            upgradeCost = 350;
            tex = zTex;
            pos = zPos;
            //hitbox
            projectileTex = zProjectileTex;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
