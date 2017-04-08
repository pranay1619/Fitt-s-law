using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitts_Law
{
    class CircleObj
    {
        public Size diameter;
        public Point location;

        public CircleObj(Size d, Point p)
        {
            diameter = d;
            location = p;            
        }

        public Size Diameter
        {
            get { return diameter; }
            set { value = diameter;}
        }

        public Point Location
        {
            get { return location; }
            set { value = location; }           
        }

        public bool IsHit(int x, int y)
        {
            Rectangle rc = new Rectangle(this.Location, this.Diameter);
            return rc.Contains(x, y);
        }


    }
}
