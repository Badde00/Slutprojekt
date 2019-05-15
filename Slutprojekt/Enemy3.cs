using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Enemy3 : BaseEnemy
    {
        public Enemy3(int round, List<Vector2> tPoints)
        {
            roundModifier = 1 + ((round * round) / 100);
            baseHp = 1000;
            hp = baseHp * ((int)roundModifier * 2 + 2);
            maxHp = hp;
            baseVelocity = 0.7f;
            velocity = baseVelocity * (1.1f * (roundModifier - 1)) + 1;
            dmg = 50;
            gold = 350;
            dangerLevel = hp + 2 * dmg + (int)velocity / 2;
            turningPoints = tPoints;
            tex = Assets.Enemy3;
            pos = new Vector2(turningPoints[0].X, turningPoints[0].Y);
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            direction = CalcDirection(pos, turningPoints[currentTurningPoint + 1]);
        }

        public override void Update()
        {
            if (turningPoints.Count <= currentTurningPoint + 1)
            {
                Playing.Life -= dmg;
                isDead = true;
            }

            if (hp <= 0)
            {
                isDead = true;
            }


            //Kollar om enheten är inom 10 pixlar av nästa svängplats, så den inte missar den
            if (turningPoints.Count > currentTurningPoint + 1)
            {
                if (pos.X >= turningPoints[currentTurningPoint + 1].X - 5 && pos.Y >= turningPoints[currentTurningPoint + 1].Y - 5 &&
                    pos.X <= turningPoints[currentTurningPoint + 1].X + 5 && pos.Y <= turningPoints[currentTurningPoint + 1].Y + 5)
                {
                    currentTurningPoint++;
                    if (currentTurningPoint + 1 < turningPoints.Count)
                        direction = CalcDirection(pos, turningPoints[currentTurningPoint + 1]);
                }
            }

            pos.X += velocity * (float)Math.Cos(direction);
            pos.Y += velocity * (float)Math.Sin(-direction); //-dir pga omvänt y från vanlig matte
            distanceTraveled += (float)Math.Sqrt(velocity * (float)Math.Cos(direction) * velocity * (float)Math.Cos(direction)
                + velocity * (float)Math.Sin(-direction) * velocity * (float)Math.Sin(-direction)); //Pytagoras sats för att räkna ut hur långt den har flyttat på sig
            hitbox.Location = new Point((int)pos.X - 25, (int)pos.Y - 25);
        }
    }
}
