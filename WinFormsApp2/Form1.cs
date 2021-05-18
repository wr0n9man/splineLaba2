using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {

        int x1, y1, x2, y2;
        private Bitmap bmp;
        private Bitmap bmp2;
        private Pen blackPen;
        private Pen greenPen;
        List<Point> points = new List<Point>();
        List<Point> points2 = new List<Point>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            reset();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Start();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
        }

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            blackPen = new Pen(Color.Blue, 1);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x2 = Convert.ToInt32(e.X); // координата по оси X
            y2 = Convert.ToInt32(e.Y);

        }

        private void reset()
        {
            //x2 = Convert.ToInt32(e.X); // координата по оси X
            //y2 = Convert.ToInt32(e.Y); // координата по оси Y
            points.Add(new Point(x2, y2));
            this.Invalidate();

            if ((x1 + y1) == 0)
            {
               
                x1 = x2;
                y1 = y2;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);

                blackPen = new Pen(Brushes.Red, 2);
                g.DrawLine(blackPen, x1, y1, x2, y2);

                blackPen.Dispose();
                g.Dispose();
                pictureBox1.Image = bmp;
                pictureBox1.Invalidate();

                x1 = x2;
                y1 = y2;
            }
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
     
            greenPen = new Pen(Brushes.Green, 2);
            Graphics br = Graphics.FromImage(bmp2);
            br.Clear(Color.White);
            points2 = DouglasPeuckerReduction(points, Convert.ToDouble( numericUpDown1.Value));
            for (int i = 1; i < points2.Count; i++)
            {
                br.DrawLine(greenPen, points2[i - 1], points2[i]);
                
            }
            pictureBox2.Image = bmp2;
            pictureBox2.Invalidate();

        }

 
        public static List<Point> DouglasPeuckerReduction (List<Point> Points, Double Tolerance)
        {
            if (Points == null || Points.Count < 3)
                return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint,  Tolerance, ref pointIndexsToKeep);

            List<Point> returnPoints = new List<Point>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        private static void DouglasPeuckerReduction(List<Point>points, Int32 firstPoint, Int32 lastPoint, Double tolerance, ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = PerpendicularDistance
                    (points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint,
                indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest,
                lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }


        public static Double PerpendicularDistance (Point Point1, Point Point2, Point Point)
        {
           

            Double area = Math.Abs(.5 * (Point1.X * Point2.Y + Point2.X *
            Point.Y + Point.X * Point1.Y - Point2.X * Point1.Y - Point.X *
            Point2.Y - Point1.X * Point.Y));
            Double bottom = Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) +
            Math.Pow(Point1.Y - Point2.Y, 2));
            Double height = area / bottom * 2;

            return height;

      
        }

    }
}
