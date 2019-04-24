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
            hitbox = new Rectangle((int)zPos.X, (int)zPos.Y, 50, 50);
            //description

            dmgCaused = zDmgCaused;
            upgradeCost = 350;
            aps = 0.8;
            dmg = 40;
            radius = 150;
            pierce = 2;
            projectileSpeed = 10;
            dArea = new Circle((int)pos.X, (int)pos.Y, radius);
            targetMode = AttackMode.first;
        }

        public T1U0(Vector2 zPos)
        {
            tex = Assets.T1U0;
            pos = zPos;
            projectileTex = Assets.T1Projectile;
            hitbox = new Rectangle((int)zPos.X, (int)zPos.Y, 50, 50);
            //description


            dmgCaused = 0;
            upgradeCost = 350;
            aps = 0.8;
            dmg = 40;
            radius = 150;
            pierce = 2;
            projectileSpeed = 10;
            dArea = new Circle((int)pos.X, (int)pos.Y, radius);
            targetMode = AttackMode.first;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
