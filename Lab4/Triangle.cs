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
using System.Runtime.Serialization.Formatters.Binary;


namespace Lab4
{
    [Serializable]
    class Triangle
    {
        List<Point> points;
        public int id { get; private set; }
        int a_id;
        int b_id;
        int c_id;

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
        public static void SerializeTriangleBin(Triangle triangle)
        {
            BinaryFormatter Bin = new BinaryFormatter();

            using (FileStream fs = new FileStream("TriangleSerialize.xml", FileMode.OpenOrCreate))
            {
                Bin.Serialize(fs, triangle);
            }

        }
        public static Triangle DeSerializeTriangle (Triangle triangle)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Triangle));

            using (FileStream fs = new FileStream("TriangleSerialize.dat", FileMode.OpenOrCreate))
            {
                Triangle returnned_trinagle = (Triangle)xml.Deserialize(fs);

                return returnned_trinagle;

            }

        }
        public static Triangle DeSerializeTriangleBin(Triangle triangle)
        {
            BinaryFormatter Bin = new BinaryFormatter();

            using (FileStream fs = new FileStream("TriangleSerialize.dat", FileMode.OpenOrCreate))
            {
                Triangle returnned_trinagle = (Triangle)Bin.Deserialize(fs);

                return returnned_trinagle;

            }

        }

        public int ToSql(string connectionstring)
        {
            if (id != -1)
                return -1;


            foreach (Point point in points)
            {
                point.ToSql(connectionstring);
            }
           
           
            var command = new SqlCommand($"insert into triangles " +
                                         $"values ({points[0].id}, {points[1].id}, {points[2].id}) " +
                                         $"select scope_identity()")
            { connection = new SqlConnection(connectionstring) };
            command.connection.open();
            id = Convert.ToInt32(command.executescalar());
            command.connection.close();
            return id;
        }
        public static Triangle fromsql(string connectionstring, int id)
        {

            var command = new SqlCommand("select x, y, points.id " +
                                         "from " +
                                         "points join triangles " +
                                         "on (triangles.point1 = points.id or " +
                                         "triangles.point2 = points.id or " +
                                         "triangles.point3 = points.id) " +
                                         $"where triangles.id = {id}")
            { connection = new SqlConnection(connectionstring) };
            command.connection.open();
            using (var reader = command.executereader())
            {
                if (!reader.hasrows)
                    return null;
                Point p1, p2, p3;
                reader.read();
                p1 = new Point(reader.getdouble(0), reader.getdouble(1));
                p1.id = reader.getint32(2);
                reader.read();
                p2 = new Point(reader.getdouble(0), reader.getdouble(1));
                p2.id = reader.getint32(2);
                reader.read();
                p3 = new Point(reader.getdouble(0), reader.getdouble(1));
                p3.id = reader.getint32(2);
                Triangle res = new Triangle(p1, p2, p3);
                res.id = id;
                return res;
            }
        }


    }
}
