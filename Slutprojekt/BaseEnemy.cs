using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    abstract class BaseEnemy : BaseUnit
    {
        protected int hp;
        protected int dangerLevel;
        protected int round;
        protected float velocity;
        protected int dmg;
        protected int gold;
        protected Projectile p;

        public int Hp
        {
            get;
            private set;
        }

        public int DangerLevel
        {
            get;
            private set;
        }

        public int Round
        {
            get;
            private set;
        }

        public float Velocity
        {
            get;
            private set;
        }

        public int Dmg
        {
            get;
            private set;
        }

        public int Gold
        {
            get;
            private set;
        }

        public Projectile P
        {
            get;
            private set;
        }
    }
}
