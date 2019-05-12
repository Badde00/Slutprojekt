using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    public enum AttackMode
    {
        first,
        strong,
        last
    }

    abstract class BaseTower : BaseUnit
    {
        protected List<BaseEnemy> enemiesClose = new List<BaseEnemy>(); //ska innehålla alla fiender inom tornets radie.
        protected List<BaseEnemy> tempEClose = new List<BaseEnemy>(); //För borttagning
        protected double aps; //attacks per second. Mellanrummet mellan attacker kommer att vara t=(1/aps)
        protected double time = 0;
        protected double previousTime = Game1.Game.Time;
        protected int dmg;
        protected int radius; //Hur långt bort torner "ser"
        protected Circle dArea;
        protected int pierce; //Hur många fiender det kan träffa
        protected int upgradeCost;
        protected float projectileSpeed; 
        protected double projectileDir; //Vart tornet ska skjuta
        protected int dmgCaused;
        protected Texture2D projectileTex;
        protected AttackMode targetMode;
        protected BaseEnemy target;
        protected bool willShoot = false;


        public int Radius
        {
            get { return radius; }
        }

        public int UpgradeCost
        {
            get { return upgradeCost; }
        }

        public int DmgCaused
        {
            get { return dmgCaused; }
        }

        public bool WillShoot
        {
            get { return willShoot; }
        }

        public virtual void Shoot()
        {
            willShoot = false;
            float longestDistance = 0;
            float shortestDistance = float.MaxValue;
            float mostDanger = 0;
            foreach (BaseEnemy e in enemiesClose)
            {
                if(targetMode == AttackMode.first)
                {
                    if(e.DistanceTraveled > longestDistance)
                    {
                        longestDistance = e.DistanceTraveled;
                        target = e;
                    }
                }
                else if(targetMode == AttackMode.last)
                {
                    if(e.DistanceTraveled < shortestDistance)
                    {
                        shortestDistance = e.DistanceTraveled;
                        target = e;
                    }
                }
                else
                {
                    if(e.DangerLevel > mostDanger)
                    {
                        mostDanger = e.DangerLevel;
                        target = e;
                    }
                }
            }
            if(target != null)
                projectileDir = FindEnemy(target.Pos);


            Playing.UnitsWhenPlaying.Add(new Projectile(pierce, projectileSpeed, (float)projectileDir, dmg,
                projectileTex, new Vector2(pos.X + 25, pos.Y + 25), new Rectangle((int)pos.X + 25, (int)pos.Y +25, projectileTex.Width, projectileTex.Width), ProjDir()));
        }

        public override void Update()
        {
            time += Game1.Game.Time - previousTime;

            AddEClose();

            if (time >= (1 / aps) && enemiesClose.Count > 0)
            {
                time = 0;
                willShoot = true;
            }

            previousTime = Game1.Game.Time;
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected float FindEnemy(Vector2 target)
        {
            float d;
            d = (float)Math.Atan2((pos.Y - target.Y), (target.X - pos.X));

            return d;
        }

        protected void AddEClose()
        {
            enemiesClose.Clear();
            foreach (BaseUnit u in Playing.UnitsWhenPlaying)
            {
                if (u is BaseEnemy)
                {
                    if (dArea.Intersects(u.Hitbox))
                    {
                        enemiesClose.Add(u as BaseEnemy);
                    }
                }
            }
        }

        protected Vector2 ProjDir()
        {
            Vector2 d = target.Pos - pos;
            d.Normalize();
            return d;
        }
    }
}
