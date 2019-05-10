using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Slutprojekt
{
    class Line
    {
        private Vector2 p1; //Linjens punkt 1
        private Vector2 p2;
        private Vector2 t1; //t1 == t1. För interaktion, så jag inte behöver ändra linjen
        private Vector2 t2;
        public float t;
        public float u;
        private float n;

        private Vector2 iPoint; //Punkten där linjer korsar
        private Vector2 t4; //som p2 för p3(Rektanglens hörn)


        public bool Intersect(Rectangle rectangle)
        {
            List<Vector2> corners = new List<Vector2>//Lånade detta från min cirkel, men bytte till Vector2 och lista då jag inte är van vid arrays
            {
            new Vector2(rectangle.Top, rectangle.Left),
            new Vector2(rectangle.Top, rectangle.Right),
            new Vector2(rectangle.Bottom, rectangle.Right),
            new Vector2(rectangle.Bottom, rectangle.Left)
            };
            


            foreach (Vector2 t3 in corners) //Kommer behandla 2 hörn i rektangeln som t1 & t2 för en annan linje
            {
                int i = corners.IndexOf(t3);
                if (i + 1 == corners.Count) //Om foreach är på den sista i listan så ska jag använda den första som t2
                {
                    t4 = corners[0];
                }
                else
                {
                    t4 = corners[i + 1];
                }



                n = ((t1.X - t2.X) * (t3.Y - t4.Y) - (t1.Y - t2.Y) * (t3.X - t4.X));
                t = ((t1.X - t3.X) * (t3.Y - t4.Y) - (t1.Y - t3.Y) * (t3.X - t4.X)) / n;
                u = -((t1.X - t2.X) * (t1.Y - t3.Y) - (t1.Y - t2.Y) * (t1.X - t3.X)) / n;
                //Fick från Wikipedia för korsande linjer
                //https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
                //Skaffar punkten där linjerna korsar om de vore oändliga. Ska sedan kolla om punkten ligger mellan t1 och t2


                if (n == 0) //Om det nedanför skulle dela på 0 så är linjerna parallella och korsar ej
                    continue;
                
                if(t > 0 && t < 1 && u < 1 && u > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public Line(Vector2 point1, Vector2 point2)
        {
            p1 = point1;
            p2 = point2;
            t1 = p1;
            t2 = p2;
        }

        public Line(Point point1, Point point2)
        {
            p1 = point1.ToVector2();
            t2 = point2.ToVector2();
            t1 = p1;
            t2 = p2;
        }

        public Line()
        {

        }

        public Vector2 P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        public Vector2 P2
        {
            get { return p2; }
            set { p2 = value; }
        }

        public Vector2 IPoint
        {
            get { return iPoint; }
        }
    }
}
