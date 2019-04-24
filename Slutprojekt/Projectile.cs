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
        private Vector2 speed;
        private int dmg;
        private bool isDead;
        Vector2 texCenter;


        public int Dmg
        {
            get { return dmg; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public Projectile(int zPierce, Vector2 zSpeed, int zDmg, Texture2D zTex, Vector2 zPos, Rectangle zHitBox)
        {
            description = null; //projektilerna behöver ingen description
            isDead = false;

            pierce = zPierce;
            speed = zSpeed;
            dmg = zDmg;
            tex = zTex;
            pos = zPos;

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
                    }
                }
            }

            pos += speed;
            hitbox.Location = new Point((int)pos.X - 25, (int)pos.Y - 25);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Måste fråga tim hur man roterar
            //spriteBatch.Draw(tex, new Rectangle(300, 300, tex.Width, tex.Height), null, Color.White, y, texCenter, SpriteEffects.None, 0); 
        }
    }
}
