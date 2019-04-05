using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Enemy1 : BaseEnemy
    {
        public Enemy1(int round, Texture2D zTex, Rectangle zHitBox, List<Vector2> tPoints)
        {
            roundModifier = 1 + ((round * round) / 100);
            hp = baseHp * (int)roundModifier;
            maxHp = hp;
            baseVelocity = 5;
            velocity = baseVelocity * (0.5f * (roundModifier - 1) + 1);
            dmg = 20;
            gold = 50;
            dangerLevel = hp + 2 * dmg + (int)velocity / 2;
            turningPoints = tPoints;
            direction = CalcDirection(pos, turningPoints[currentTurningPoint + 1]);

            tex = zTex;
            pos = new Vector2(turningPoints[0].X, turningPoints[0].Y);
            hitbox = zHitBox;
        }

        public override void Update()
        {
            if(turningPoints[currentTurningPoint + 1] == null)
            {
                Playing.Life -= dmg;
                isDead = true;
            }

            if(hp <= 0)
            {
                isDead = true;
            }
            
            //Kollar om enheten är inom 50 pixlar av nästa svängplats, så den inte missar den
            if (pos.X >= turningPoints[currentTurningPoint + 1].X - 25 && pos.Y >= turningPoints[currentTurningPoint + 1].Y -25 && 
                pos.X <= turningPoints[currentTurningPoint + 1].X + 25 && pos.Y <= turningPoints[currentTurningPoint + 1].Y + 25) 
            {
                currentTurningPoint++;
                direction = CalcDirection(pos, turningPoints[currentTurningPoint + 1]);
            }

            pos.X += velocity * (float)Math.Cos(direction);
            pos.Y += velocity * (float)Math.Sin(direction);
        }
    }
}
