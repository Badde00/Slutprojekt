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


        public int Dmg
        {
            get;
            private set;
        }

        public Projectile(int zPierce, Vector2 zSpeed, int zDmg, Texture2D zTex, Vector2 zPos, Rectangle zHitBox)
        {
            description = null; //projektilerna behöver ingen description
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.Game.ProjectileTex.Width, Game1.Game.ProjectileTex.Height);

            zPierce = pierce;
            zSpeed = speed;
            zDmg = dmg;
            zTex = tex;
            zPos = pos;
        }


        public override void Update() //Kommer att göra denna efter jag är klar med tornen och jag har ett collision system
        {
            
        }
    }
}
