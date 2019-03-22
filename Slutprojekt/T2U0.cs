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
        public T2U0(Vector2 zPos, int? zDmgCaused)
        {
            upgradeCost = 475;
            tex = Assets.T2U0;
            pos = zPos;

            if (zDmgCaused == null)
                zDmgCaused = 0;
            //hitbox
            projectileTex = Assets.T2Projectile;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
