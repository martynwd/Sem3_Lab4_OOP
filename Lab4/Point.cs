using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4
{
    [Serializable]
    public class Point
    {
        double x { get; }
        double y { get; }
        public int id;


        public Point() { }
        public Point (double x_,double y_)
        {
            x = x_;
            y = y_;
        }
    }
}
