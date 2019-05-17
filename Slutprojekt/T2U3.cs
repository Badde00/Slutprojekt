using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class T2U3 : BaseTower
    {
        public T2U3(Vector2 zPos, int zDmgCaused)
        {
            pos = zPos;
            dmgCaused = zDmgCaused;
            Init();
        }

        public T2U3(Vector2 zPos)
        {
            pos = zPos;
            dmgCaused = 0;
            Init();
        }

        private void Init()
        {
            tex = Assets.T2U3;
            projectileTex = Assets.T1Projectile;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);

            aps = 13;
            dmg = 60;
            radius = 350;
            pierce = 3;
            projectileSpeed = 10;
            dArea = new Circle((int)pos.X, (int)pos.Y, radius);
            targetMode = AttackMode.first;
        }
    }
}
