using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class Line
    {
        Vector2 p1;
        Vector2 p2;

        public Line(Vector2 point1, Vector2 point2)
        {
            p1 = point1;
            p2 = point2;
        }

        public Line(Point point1, Point point2)
        {
            p1 = point1.ToVector2();
            p2 = point2.ToVector2();
        }

        public Line()
        {

        }
    }
}
