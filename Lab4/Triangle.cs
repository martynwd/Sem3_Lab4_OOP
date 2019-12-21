using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;   
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Data.SqlClient;


namespace Lab4
{
    [Serializable]
    class Triangle
    {
        List<Point> points;
        public int id { get; private set; }


        public Triangle (Point a,Point b,Point c)
        {
            this.points.Add(a);
            this.points.Add(b);
            this.points.Add(c);
        }


        public Triangle()
        {
            points = new List<Point>();
            id = -1;
        }
        public Triangle (List<Point> points_)
        {
            this.points = points_;
            if (points.Count!=3)
            {
                throw new InvalidOperationException("For triangle need 3 points");
            }
        }
        public IEnumerable<Point> getPoints()
        {
            return points;
        }
        public static void SerializeTriangle (Triangle triangle)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Triangle));

            using (FileStream fs = new FileStream("TriangleSerialize.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, triangle);
            }
             
        }
        public static Triangle DeSerializeTriangle (Triangle triangle)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Triangle));

            using (FileStream fs = new FileStream("TriangleSerialize.xml", FileMode.OpenOrCreate))
            {
                Triangle returnned_trinagle = (Triangle)xml.Deserialize(fs);

                return returnned_trinagle;

            }

        }
        public int tosql(string connectionstrin_)
        {
            if (id != -1)
                return -1;

            a.tosql(connectionstring);
            b.tosql(connectionstring);
            c.tosql(connectionstring);
            var command = new sqlcommand($"insert into triangles " +
                                         $"values ({a.id}, {b.id}, {c.id}) " +
                                         $"select scope_identity()")
            { connection = new sqlconnection(connectionstring) };
            command.connection.open();
            id = convert.toint32(command.executescalar());
            command.connection.close();
            return id;
        }
        public static triangle fromsql(string connectionstring, int id)
        {

            var command = new sqlcommand("select x, y, points.id " +
                                         "from " +
                                         "points join triangles " +
                                         "on (triangles.point1 = points.id or " +
                                         "triangles.point2 = points.id or " +
                                         "triangles.point3 = points.id) " +
                                         $"where triangles.id = {id}")
            { connection = new sqlconnection(connectionstring) };
            command.connection.open();
            using (var reader = command.executereader())
            {
                if (!reader.hasrows)
                    return null;
                point p1, p2, p3;
                reader.read();
                p1 = new point(reader.getdouble(0), reader.getdouble(1));
                p1.id = reader.getint32(2);
                reader.read();
                p2 = new point(reader.getdouble(0), reader.getdouble(1));
                p2.id = reader.getint32(2);
                reader.read();
                p3 = new point(reader.getdouble(0), reader.getdouble(1));
                p3.id = reader.getint32(2);
                triangle res = new triangle(p1, p2, p3);
                res.id = id;
                return res;
            }
        }


    }
}
