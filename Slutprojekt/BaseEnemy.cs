using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    abstract class BaseEnemy : BaseUnit
    {
        protected int baseHp = 100;
        protected int hp;
        protected int maxHp;
        protected int dangerLevel; //Detta ska vara ett nummer som påverkar hur torn attakerar. 
        protected float baseVelocity;
        protected float velocity;
        protected float distanceTraveled = 0; //Så torn kan attakera fienden som har gått längst 
        protected int dmg;
        protected int gold;
        protected float roundModifier;
        protected float direction;
        protected List<Vector2> turningPoints;
        protected int currentTurningPoint = 0;
        protected bool isDead = false;

        public int Hp
        {
            get;
            set;
        }

        public bool IsDead
        {
            get;
            private set;
        }

        public int MaxHp
        {
            get;
            set;
        }

        public int DangerLevel
        {
            get;
            private set;
        }
        
        public int Gold
        {
            get;
            private set;
        }

        protected float CalcDirection(Vector2 currentPos, Vector2 target)
        {
            float d; //Kan inte skriva private innan float d eftersom VS klagar av någon anledning då
            d = (float)Math.Atan((target.Y - currentPos.Y) / (target.X - currentPos.X));

            return d;
        }
    }
}
