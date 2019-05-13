using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Projectile : BaseUnit
    {   //Valde att skippa size
        private int pierce; //Antalet fiender som kan träffas
        private List<BaseEnemy> enemiesHit;
        private float speed;
        private int dmg;
        private bool isDead;
        Vector2 texCenter;
        float angle;
        Vector2 dir;
        BaseTower sentFrom


        public int Dmg
        {
            get { return dmg; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public Projectile(int zPierce, float zSpeed, float zAngle, int zDmg, Texture2D zTex, Vector2 zPos, Rectangle zHitBox, Vector2 zDir, BaseTower zSentFrom)
        {
            isDead = false;

            pierce = zPierce;
            speed = zSpeed;
            dmg = zDmg;
            tex = zTex;
            pos = zPos;
            angle = zAngle;
            dir = zDir;
            sentFrom = zSentFrom;

            texCenter = new Vector2(tex.Width / 2, tex.Height / 2);
            hitbox = new Rectangle((int)zPos.X, (int)zPos.Y, 20, 20);
            enemiesHit = new List<BaseEnemy>();
        }


        public override void Update() //Kommer att göra denna efter jag är klar med tornen och jag har ett collision system
        {
            if(enemiesHit.Count >= pierce)
            {
                isDead = true;
            }

            foreach (BaseUnit e in Playing.UnitsWhenPlaying)
            {
                if (e is BaseEnemy)
                {
                    if (!enemiesHit.Contains(e) && hitbox.Intersects(e.Hitbox))
                    {
                        enemiesHit.Add(e as BaseEnemy);
                        (e as BaseEnemy).Hp -= dmg;
                        sentFrom.DmgCaused += dmg;
                    }
                }
            }

            pos += dir * speed;
            hitbox.Location = new Point((int)pos.X, (int)pos.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle((int)pos.X - 25, (int)pos.Y - 25, hitbox.Width, hitbox.Height), null, Color.White, -angle-(float)Math.PI/2, texCenter, SpriteEffects.None,0); 
        }
    }
}
