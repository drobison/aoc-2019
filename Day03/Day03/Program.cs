using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day03
{
    public class Program
    {
        static void Main(string[] args)
        {
            // translate input to coordinates
            LoadData("input.txt", out var points1, out var points2);
            var result = FindMinManPath(points1, points2);
        }

        public static int FindMinManPath(List<Point> points1, List<Point> points2)
        {
            var intersections = FindIntersections(points1, points2);
            var min = Int32.MaxValue;
            foreach (var intersection in intersections)
            {
                var inter = new Point()
                {
                    X = Convert.ToInt32(intersection.X),
                    Y = Convert.ToInt32(intersection.Y)
                };

                min = Math.Min(min, ManhattanDistanceToPoint(points1, inter) + ManhattanDistanceToPoint(points2, inter));
            }

            return min;
        }

        public static int ManhattanDistanceFromCenter(PointF first)
        {
            return Convert.ToInt32(Math.Abs(first.X) + Math.Abs(first.Y));
        }

        public static bool IsVerticalLine(Point first, Point second)
        {
            if (first.X == second.X) return true;
            return false;
        }

        public static bool BetweenTwoNumbers(int start, int end, int input)
        {
            var min = Math.Min(start, end);
            var max = Math.Max(start, end);
            return (input <= max && input >= min);
        }

        public static int ManhattanDistanceToPoint(List<Point> path, Point destination)
        {
            var distance = 0;
            var startPoint = new Point(){X = 0, Y = 0};

            foreach (var endPoint in path)
            {
                // does point exist in route to next point

                if (IsVerticalLine(startPoint, endPoint))
                {
                    if ((endPoint.X == destination.X) && BetweenTwoNumbers(startPoint.Y, endPoint.Y, destination.Y))
                    {
                        distance += Math.Abs(destination.Y - startPoint.Y);
                        return distance;
                    }
                    distance += Math.Abs(endPoint.Y - startPoint.Y);

                }
                else
                {
                    if ((endPoint.Y == destination.Y) && BetweenTwoNumbers(startPoint.X, endPoint.X, destination.X))
                    {
                        distance += Math.Abs(destination.X - startPoint.X);
                        return distance;
                    }

                    distance += Math.Abs(endPoint.X - startPoint.X);
                }

                startPoint = endPoint;
            }

            return distance;
        }

        public static int LineLength(PointF first, PointF second)
        {
            // Formula sqrt((x2 - x1)^2 + (y2 - y1)^2)
            return Convert.ToInt32(Math.Sqrt(Math.Pow((second.Y - first.Y), 2) + Math.Pow((second.X - first.X), 2)));
        }

        private static List<PointF> FindIntersections(List<Point> points1, List<Point> points2)
        {
            List<PointF> intersections = new List<PointF>();
            for (var point1 = 0; point1 < points1.Count - 1; point1++)
            {
                for (var point2 = 0; point2 < points2.Count - 1; point2++)
                {
                    if (doIntersect(points1[point1], points1[point1 + 1], points2[point2], points2[point2 + 1]))
                    {
                        Console.WriteLine($"Found an intersection:");
                        Console.WriteLine($"Line 1: ({points1[point1]}, {points1[point1 + 1]})\nLine 2: {points2[point2]}, {points2[point2 + 1]}");

                        FindIntersection(ConvertPoint(points1[point1]), ConvertPoint(points1[point1 + 1]), ConvertPoint(points2[point2]),
                            ConvertPoint(points2[point2 + 1]), out var linesIntersect, out var segmentsIntersect, out var intersection, out var closeP1, out var closeP2);

                        intersections.Add(intersection);
                    }
                }
            }
            return intersections;
        }

        private static PointF ConvertPoint(Point point)
        {
            var pointF = new PointF((float)point.X, (float)point.Y);
            return pointF;
        }

        //private static Point CalculateIntersection(Point point1, Point point2, Point point3, Point point4)
        //{
        //    //var slope1 = CalculateSlope(point1, point2);
        //    // slope intercept form
        //    // y = mx + b
        //    // m = change x/ change 
        //    // b = y - mx 

        //    //https://stackoverflow.com/a/4543530/1674958
        //    var delta = 

        //}

        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        private static void FindIntersection(
            PointF p1, PointF p2, PointF p3, PointF p4,
            out bool lines_intersect, out bool segments_intersect,
            out PointF intersection,
            out PointF close_p1, out PointF close_p2)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                close_p1 = new PointF(float.NaN, float.NaN);
                close_p2 = new PointF(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                / -denominator;

            // Find the point of intersection.
            intersection = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new PointF(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }

        private static int CalculateSlope(Point point1, Point point2)
        {
            var slope = (point2.Y - point1.Y) / (point2.X - point1.X);
            return slope;
        }

        private static int CalculateSlopeIntercept(Point point, int slope)
        {
            // b = y - mx 
            var slopeIntercept = point.Y - (slope * point.X);
            return slopeIntercept;
        }

        public static void LoadData(string fileName, out List<Point> points1, out List<Point> points2)
        {
            using (TextReader reader = File.OpenText(fileName))
            {
                var firstLine = reader.ReadLine();
                var firstLineList = firstLine.Split(",").ToList();
                points1 = ParsePoints(firstLineList);

                var secondLine = reader.ReadLine();
                var secondLineList = secondLine.Split(",").ToList();
                points2 = ParsePoints(secondLineList);
            }
        }

        public static List<Point> ParsePoints(List<string> input)
        {
            var points = new List<Point>();
            var lastPoint = new Point(){X = 0, Y = 0};
            foreach (var command in input)
            {
                var direction = command[0].ToString().ToUpper();
                var distance = Convert.ToInt32(command.Substring(1));
                Point nextPoint = new Point(){X = lastPoint.X, Y = lastPoint.Y};
                switch (direction)
                {
                    case "U":
                        nextPoint.Y += distance;
                        break;
                    case "L":
                        nextPoint.X -= distance;
                        break;
                    case "D":
                        nextPoint.Y -= distance;
                        break;
                    case "R":
                        nextPoint.X += distance;
                        break;
                    default:
                        throw new ArgumentException($"Unknown direction - {direction}");
                }
                points.Add(nextPoint);
                lastPoint = nextPoint;
            }

            return points;
        }

        // Given three colinear points p, q, r, the function checks if 
        // point q lies on line segment 'pr' 
        static Boolean onSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r). 
        // The function returns following values 
        // 0 --> p, q and r are colinear 
        // 1 --> Clockwise 
        // 2 --> Counterclockwise 
        static int orientation(Point p, Point q, Point r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
            // for details of below formula. 
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0; // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }

        // The main function that returns true if line segment 'p1q1' 
        // and 'p2q2' intersect. 
        static Boolean doIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases 
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"({X} , {Y})";
        }
    }
}
