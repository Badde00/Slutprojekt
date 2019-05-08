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

        private Vector2 iPoint; //Punkten där linjer korsar
        private Vector2 p4; //som p2 för p3(Rektanglens hörn)


        public bool Intersect(Rectangle rectangle)
        {
            List<Vector2> corners = new List<Vector2>//Lånade detta från min cirkel, men bytte till Vector2 och lista då jag inte är van vid arrays
            {
            new Vector2(rectangle.Top, rectangle.Left),
            new Vector2(rectangle.Top, rectangle.Right),
            new Vector2(rectangle.Bottom, rectangle.Right),
            new Vector2(rectangle.Bottom, rectangle.Left)
            };
            


            foreach (Vector2 p3 in corners) //Kommer behandla 2 hörn i rektangeln som t1 & t2 för en annan linje
            {
                int i = corners.IndexOf(p3);
                if (i + 1 == corners.Count) //Om foreach är på den sista i listan så ska jag använda den första som t2
                {
                    p4 = corners[0];
                }
                else
                {
                    p4 = corners[i + 1];
                }

                if (((t1.X - t2.X) * (p3.Y - p4.Y) - (t1.Y - t2.Y) * (p3.X - p4.X)) == 0) //Om det nedanför skulle dela på 0 så är linjerna parallella och korsar ej
                    return false;

                iPoint = new Vector2(((t1.X * t2.Y - t1.Y * t1.X) * (p3.X - p4.X) - (t1.X - t2.X) * (p3.X * p4.Y - p3.Y * p4.X) /
                    ((t1.X - t2.X) * (p3.Y - p4.Y) - (t1.Y - t2.Y) * (p3.X - p4.X))), 
                    ((t1.X * t2.Y - t1.Y * t1.X) * (p3.Y - p4.Y) - (t1.Y - t2.Y) * (p3.X * p4.Y - p3.Y * p4.X) / 
                    ((t1.X - t2.X) * (p3.Y - p4.Y) - (t1.Y - t2.Y) * (p3.X - p4.X))));
                //Fick från Wikipedia för korsande linjer
                //https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
                //Skaffar punkten där linjerna korsar om de vore oändliga. Ska sedan kolla om punkten ligger mellan t1 och t2

                Vector2 temp;

                if (t1.X == t2.X && ((iPoint.Y > t1.Y && iPoint.Y < t2.Y) || (iPoint.Y > t2.Y && iPoint.Y < t1.Y))) //Om t1.X == t2.X så måste iPointY ligga mellan t1.Y och t2.Y om linjerna korsar
                {
                    return true;
                }
                else if (t1.X > t2.X)//Med detta så är t2.x altid större än t1.X, vilket för det lättare för mig senare
                {
                    temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                if (iPoint.X > t1.X && iPoint.X < t2.X) //Om iPoint ligger på samma oändliga linje som t1 och t2 skapar samt mellan t1.X och t2.X så korsas linjerna
                    return true;
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
