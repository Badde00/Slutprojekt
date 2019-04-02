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
        protected double aps; //attacks per second. Mellanrummet mellan attacker kommer att vara t=(1/aps)
        protected double time = 0;
        protected double previousTime = Game1.Game.Time;
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

        public int UpgradeCost
        {
            get;
            private set;
        }

        public int DmgCaused
        {
            get;
            private set;
        }

        public virtual Projectile MakeProjectile()
        {
            p = new Projectile(pierce, MakeProjVector(projectileSpeed, projectileDegreeDir), dmg, 
                projectileTex, pos, new Rectangle((int)pos.X, (int)pos.Y, projectileTex.Width, projectileTex.Width));
            return p;
        }

        public override void Update()
        {
            time += Game1.Game.Time - previousTime;
            if (time >= (1 / aps))
            {
                time -= (1 / aps);
                Playing.UnitsWhenPlaying.Add(MakeProjectile());
            }
        }

        public Vector2 MakeProjVector(float speed, double angle) //Lättare att läsa
        {
            Vector2 temp = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
            return temp;
        }
        



        //Måste göras

        /*private void FindEnemy() 
        {

        }*/
    }
}
