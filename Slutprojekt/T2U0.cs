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
        public T2U0(Vector2 zPos, int zDmgCaused)
        {
            upgradeCost = 475;
            tex = Assets.T2U0;
            pos = zPos;


            //hitbox
            projectileTex = Assets.T2Projectile;
        }

        public T2U0(Vector2 zPos)
        {
            tex = Assets.T1U0;
            pos = zPos;
            projectileTex = Assets.T1Projectile;
            

            upgradeCost = 350;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
