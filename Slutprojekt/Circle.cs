using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Circle
    {
        //Kopierat från https://stackoverflow.com/questions/24559585/how-to-create-a-circle-variable-in-monogame-and-detect-collision-with-other-circ


        public Circle(int x, int y, int radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public int Radius { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool Intersects(Rectangle rectangle)
        {
            //Ser om cirkeln korsar rektanglen genom att se om rektangelns hörn är i cirkeln
            var corners = new[]
            {
            new Point(rectangle.Top, rectangle.Left),
            new Point(rectangle.Top, rectangle.Right),
            new Point(rectangle.Bottom, rectangle.Right),
            new Point(rectangle.Bottom, rectangle.Left)
            };

            foreach (var corner in corners)
            {
                if (ContainsPoint(corner))
                    return true;
            }

            // next we want to know if the left, top, right or bottom edges overlap
            if (X - Radius > rectangle.Right || X + Radius < rectangle.Left)
                return false;

            if (Y - Radius > rectangle.Bottom || Y + Radius < rectangle.Top)
                return false;

            return true;
        }

        public bool Intersects(Circle circle)
        {
            // put simply, if the distance between the circle centre's is less than
            // their combined radius
            var centre0 = new Vector2(circle.X, circle.Y);
            var centre1 = new Vector2(X, Y);
            return Vector2.Distance(centre0, centre1) < Radius + circle.Radius;
        }

        public bool ContainsPoint(Point point)
        {
            var vector2 = new Vector2(point.X - X, point.Y - Y);
            return vector2.Length() <= Radius;
        }
    }
}
