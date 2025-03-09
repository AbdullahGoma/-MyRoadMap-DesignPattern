using Autofac;
using Autofac.Features.Metadata;
using MoreLinq;
using System.Collections;
using System.Collections.ObjectModel;

namespace Adapter
{
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Point point)
            {
                return X == point.X && Y == point.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line(Point start, Point end)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
        }

        public override bool Equals(object? obj)
        {
            if (obj is Line line)
            {
                return Start.Equals(line.Start) && End.Equals(line.End);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }
    }


    public class VectorObject : Collection<Line>
    {
        
    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class LineToPointAdaper : IEnumerable<Point>
    {
        private static int count;
        static Dictionary<int, List<Point>> cache = new();

        public LineToPointAdaper(Line line)
        {
            var hash = line.GetHashCode();
            if (cache.ContainsKey(hash)) return;

            Console.WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X}][{line.End.Y}]");

            var points = new List<Point>();

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = top - bottom;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    points.Add(new Point(left, y));
                }
            }
            else if(dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    points.Add(new Point(x, top));
                }
            }

            cache.Add(hash, points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public interface ICommand
    {
        void Execute();
    }

    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving a file");
        }
    }

    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file");
        }
    }

    public class Button
    {
        private ICommand command;
        private string name;

        public Button(ICommand command, string name)
        {
            this.command = command;
            this.name = name;
        }

        public void Click()
        {
            command.Execute();
        }

        public void PrintMe()
        {
            Console.WriteLine($"I am a button called {name}");
        }
    }

    public class Editor
    {
        private IEnumerable<Button> buttons;
        public IEnumerable<Button> Buttons => buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            this.buttons = buttons;
        }

        public void ClickAll() => buttons.ForEach(btn => btn.Click());
    }

    public class Program
    {
        private static readonly List<VectorObject> vectorObject =
            new List<VectorObject>
            {
                new VectorRectangle(1, 1, 10, 10),
                new VectorRectangle(3, 3, 6, 6),

            };
            
        public static void DrawPoint(Point p)
        {
            Console.Write(".");
        }
        static void Main(string[] args)
        {
            //Draw();
            //Draw();

            var vector = new Vector2i(1, 2);
            vector[0] = 0;

            var vector2 = new Vector2i();

            var result = vector + vector2;


            VectorOfFloat<Vector3f, Dimensions.Three> u = Vector3f.Create(1.1f, 2.1f, 3.3f);

            // Adapter in DI
            var cb = new ContainerBuilder();
            cb.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");
            cb.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
            //cb.RegisterType<Button>();
            //cb.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd)); 
            cb.RegisterAdapter<Meta<ICommand>, Button>(cmd => new Button(cmd.Value, (string)cmd.Metadata["Name"]));
            cb.RegisterType<Editor>();

            using(var c = cb.Build())
            {
                var editor = c.Resolve<Editor>();
                //editor.ClickAll();  
                foreach (var btn in editor.Buttons)
                    btn.PrintMe();
            }
        }

        private static void Draw()
        {
            foreach (var vo in vectorObject) // Collection of vectors
            {
                foreach (var line in vo) // Collection of lines
                {
                    var adapter = new LineToPointAdaper(line);
                    adapter.ForEach(DrawPoint);
                }
            }
        }
    }
}
