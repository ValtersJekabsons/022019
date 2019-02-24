using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;


namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {

            string text = System.IO.File.ReadAllText("Linijas.csv");//Location of a CSV file !!!!!!!
            text = text.Replace(",,", ",");

            List<Line> lines = new List<Line>();
            List<CrossPoints> crossPoints = new List<CrossPoints>();
            string[] textVerticalStrings = text.Split(System.Environment.NewLine);
            string[] textValues;


            for (int i = 2; i < textVerticalStrings.Length; i++)
            {
                if (textVerticalStrings[i].Length >= 1)
                {
                    textValues = textVerticalStrings[i].Split(',');

                    lines.Add(new Line(int.Parse(textValues[0]),
                                       new Point(int.Parse(textValues[1]), int.Parse(textValues[2])),
                                       new Point(int.Parse(textValues[3]), int.Parse(textValues[4]))));
                }
            }

            if (lines.Count <= 1) Environment.Exit(0); //if list has less than one element in it > then quit

            for (int i = 0; i <= lines.Count-2; i++) //Check each line    
            {
                for (int k = i + 1; k <= lines.Count; k++)
                {
                    if (k >= lines.Count) break;

                    LineCrossPoint(lines[i], lines[k], ref crossPoints);

                }
                
            }
            
            Console.WriteLine("First line ID\t|Second Line ID\t|Location (x,y)");
            for (int i = 0; i <= crossPoints.Count-1; i++)
            {
                Console.WriteLine(crossPoints[i].ID + "\t\t|" + crossPoints[i].ID2 + "\t\t|(" + crossPoints[i].COORD.X + " , " + crossPoints[i].COORD.Y + ")");
            }

            Console.ReadKey();
        }

            /*
             * LineCrossPoint Function : Returns True if given lines intersect and outputs crosspoint
             *                       Returns False if given lines does not intersect
             *                       
             *                       d1_x - x Delta of first line
             *                       d1_y - y Delta of first line
             *                       d2_x - x Delta of second line
             *                       d2_y - y Delta of second line
             */
            public static void LineCrossPoint(Line first, Line second, ref List<CrossPoints> list)
        {

            float d1_x, d1_y, d2_x, d2_y;
            int i_x, i_y;

            d1_x = first.Start.X - first.End.X;
            d1_y = first.Start.Y - first.End.Y;

            d2_x = second.Start.X - second.End.X;
            d2_y = second.Start.Y - second.End.Y;

            float s, t;
            if (((-d2_x * d1_y + d1_x * d2_y) != 0)) //Check if not divided by 0
            {
                s = (-d1_y * (first.End.X - second.End.X) + d1_x * (first.End.Y - second.End.Y)) / (-d2_x * d1_y + d1_x * d2_y);
                t = (d2_x * (first.End.Y - second.End.Y) - d2_y * (first.End.X - second.End.X)) / (-d2_x * d1_y + d1_x * d2_y);
            }
            else { return; }

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1) // Collision detected
            {
                    i_x = (int) Math.Round(first.End.X + (t * d1_x)); //X collision point
                    i_y = (int) Math.Round(first.End.Y + (t * d1_y)); // Y collision point

                list.Add(new CrossPoints(first.ID, second.ID, new Point(i_x, i_y)));

                return;
            }

            return; // No collision 
        }


    }
    /*
     * Object Line Describes one line
     * 
     * 
     */
    public class Line
    {
        public int ID;
        public Point Start;
        public Point End;

        public Line(int id, Point start, Point end)
        {
            ID = id;
            Start = start;
            End = end;
        }
    }
    /*
     * Object Line Describes single crosspoint of two lines
     * 
     * 
     */
    public class CrossPoints
    {
        public int ID;
        public int ID2;
        public Point COORD;

        public CrossPoints(int id, int id2, Point coord)
        {
            ID = id;
            ID2 = id2;
            COORD = coord;
        }
    }
}
