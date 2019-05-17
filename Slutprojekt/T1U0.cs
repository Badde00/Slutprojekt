using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class T1U0 : BaseTower
    {
        public T1U0(Vector2 zPos, int zDmgCaused)
        {
            pos = zPos;
            dmgCaused = zDmgCaused;
            Init();
        }

        public T1U0(Vector2 zPos)
        {
            pos = zPos;
            dmgCaused = 0;
            Init();
        }

        private void Init()
        {
            tex = Assets.T1U0;
            projectileTex = Assets.T1Projectile;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);

            upgradeCost = 350;
            aps = 0.8;
            dmg = 40;
            radius = 150;
            pierce = 2;
            projectileSpeed = 7.5f;
            dArea = new Circle((int)pos.X, (int)pos.Y, radius);
            targetMode = AttackMode.first;
        }
    }
}
