using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class T1U1 : Tower1
    {
        public T1U1(Vector2 zPos, int zDmgCaused)
        {
            tex = Assets.T1U0;
            pos = zPos;
            projectileTex = Assets.T1Projectile;

            dmgCaused = zDmgCaused;
            //hitbox
            upgradeCost = 500;
        }
    }
}
