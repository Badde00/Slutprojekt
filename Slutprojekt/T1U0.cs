using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class T1U0 : Tower1
    {
        public T1U0(Vector2 zPos, int zDmgCaused)
        {
            tex = Assets.T1U0;
            pos = zPos;
            projectileTex = Assets.T1Projectile;

            zDmgCaused = 0;
            //hitbox
            upgradeCost = 350;
        }

        public T1U0(Vector2 zPos)
        {
            tex = Assets.T1U0;
            pos = zPos;
            projectileTex = Assets.T1Projectile;


            upgradeCost = 350;
        }

        /*public override T1U0(Texture2D zTex, Vector2 zPos, Texture2D zProjectileTex, int zDmgCaused) //För efter jag sparat spelet och ska starta igen
        {
            tex = zTex;
            pos = zPos;
            projectileTex = zProjectileTex;

            //hitbox
            upgradeCost = 350;
        }*/

        public override void Update()
        {
            base.Update();
        }
    }
}
