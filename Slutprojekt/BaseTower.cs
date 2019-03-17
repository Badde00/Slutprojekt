using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    abstract class BaseTower : BaseUnit
    {
        protected List<BaseEnemy> enemiesClose = new List<BaseEnemy>(); //ska innehålla alla fiender inom tornets radie.
        protected float aps; //attacks per second. Mellanrummet mellan attacker kommer att vara t=1/aps
        protected int dmg;
        protected int radius; //Hur långt bort torner "ser"
        protected int pierce; //Hur många fiender det kan träffa
        protected int upgradeCost;
        protected float projectileSpeed; //double för att Math.cos() behöver det
        protected double projectileDegreeDir; //I grader för vart tornet ska skjuta. kommer att användas med cos och sin för förflyttning
        protected Projectile p; //För eventuella projektiltyper, kommer att göra projektiler i torn, sedan skicka till game.
        protected int dmgCaused;
        protected Texture2D projectileTex;


        public int Radius
        {
            get;
            private set;
        }

        public int Cost
        {
            get;
            private set;
        }

        public int DmgCaused
        {
            get;
            private set;
        }

        public virtual void MakeProjectile()
        {
            p = new Projectile(pierce, new Vector2((float)Math.Cos(projectileDegreeDir) * projectileSpeed, (float)Math.Sin(projectileDegreeDir) * projectileSpeed),dmg, tex, pos, )
        }



        //Måste göras

        /*private void FindEnemy() 
        {

        }*/
    }
}
