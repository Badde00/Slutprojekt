﻿using Microsoft.Xna.Framework;
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
        public Enemy1(int round, List<Vector2> tPoints)
        {
            roundModifier = 1 + ((round * round) / 100);
            baseHp = 200;
            hp = baseHp * (int)roundModifier;
            maxHp = hp;
            baseVelocity = 2.25f;
            velocity = baseVelocity * (0.75f * (roundModifier - 1) + 1);
            dmg = 15;
            gold = 50;
            dangerLevel = hp + 2 * dmg + (int)velocity / 2;
            turningPoints = tPoints;
            tex = Assets.Enemy1;
            pos = new Vector2(turningPoints[0].X, turningPoints[0].Y);
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            direction = CalcDirection(pos, turningPoints[currentTurningPoint + 1]);
        }

        public override void Update()
        {
            if(turningPoints.Count <= currentTurningPoint + 1)
            {
                Playing.Life -= dmg;
                isDead = true;
            }

            if(hp <= 0)
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
