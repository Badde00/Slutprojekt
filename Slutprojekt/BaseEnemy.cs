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
        protected int maxHp;
        protected int dangerLevel; //Detta ska vara ett nummer som påverkar hur torn attakerar. 
        protected int round; //Fiender ska bli starkare senare om de har "attakera starkaste fienden" inställningen på
        protected float velocity;
        protected float distanceTraveled; //Så torn kan attakera fienden som har gått längst 
        protected int dmg;
        protected int gold;

        public int Hp
        {
            get;
            set;
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
        
    }
}
