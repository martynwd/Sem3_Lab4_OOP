using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using System.Threading;


namespace Lab4
{
    class Program
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        const string connectionString = @"Data Source=LAPTOP-92JIIRRB\SQLEXPRESS;
                                Initial Catalog=lab4_OOP;Integrated Security=True";
        Point a = new Point(0, 0);
        Point b = new Point(0, 1);
        Point c = new Point(1, 0);
        //var testlist = new list<point>{a,b,c};


        //triangle test = new triangle(testlist);
        //triangle.serializetriangle(test);
        //var t1 = triangle.fromsql(connectionstring, 11);
        
       
       Triangle.DeSerializeTriangle(test);

    }
}       
