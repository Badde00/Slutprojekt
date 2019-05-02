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
        protected int baseHp;
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
            get { return hp; }
            set { hp = value; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public int MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }

        public int DangerLevel
        {
            get { return dangerLevel; }
        }
        
        public int Gold
        {
            get { return gold; }
        }

        public float DistanceTraveled
        {
            get { return distanceTraveled; }
        }

        protected float CalcDirection(Vector2 currentPos, Vector2 target)
        {
            float d;
            d = (float)Math.Atan2((currentPos.Y - target.Y), (target.X - currentPos.X));
            /* Vanligtvis för att räkna ut riktning så använder man (target.T-pos.Y)/(target.X-pos.X)
             * Men eftersom Y är invänt i detta program (Y ökar nedåt) så har jag bytt plats på de 2 för att beräkningarna ska bli rätt
             */

            return d;
        }

        protected void Died()
        {
            isDead = true;
            Playing.Money += gold;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
