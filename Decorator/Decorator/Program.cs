using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace Decorator
{
    #region Custom String Builder
    // Decorator around string builder class (U can not inherit from string builder because it sealed class)
    public class CodeBuilder
    {
        [JsonIgnore] // Prevents serialization of the internal StringBuilder
        private StringBuilder builder = new StringBuilder();

        #region Adapter-Decorator

        // This allows a string to be automatically converted into a CodeBuilder object.
        public static implicit operator CodeBuilder(string text)
        {
            var sb = new CodeBuilder();
            sb.Append(text);
            return sb;
        }

        // This allows the + operator to be used to append strings to a CodeBuilder object.
        public static CodeBuilder operator +(CodeBuilder sb, string text)
        {
            sb.Append(text);
            return sb;
        }

        override public string ToString()
        {
            return builder.ToString();
        }
        #endregion

        // Generate all delegating members

        public int EnsureCapacity(int capacity)
        {
            return builder.EnsureCapacity(capacity);
        }

        public void Append(string value)
        {
            builder.Append(value);
        }

        public CodeBuilder Append(char value)
        {
            builder.Append(value);
            return this;
        }

        public CodeBuilder Append(char value, int repeatCount)
        {
            builder.Append(value, repeatCount);
            return this;
        }

        public CodeBuilder Append(char[] value)
        {
            builder.Append(value);
            return this;
        }

        public CodeBuilder Append(char[] value, int startIndex, int charCount)
        {
            builder.Append(value, startIndex, charCount);
            return this;
        }

        public CodeBuilder Append(string value, int startIndex, int count)
        {
            builder.Append(value, startIndex, count);
            return this;
        }

        public CodeBuilder Append(StringBuilder value)
        {
            builder.Append(value);
            return this;
        }

        public CodeBuilder Append(StringBuilder value, int startIndex, int count)
        {
            builder.Append(value, startIndex, count);
            return this;
        }

        public CodeBuilder AppendFormat(string format, object arg0)
        {
            builder.AppendFormat(format, arg0);
            return this;
        }

        public CodeBuilder AppendFormat(string format, object arg0, object arg1)
        {
            builder.AppendFormat(format, arg0, arg1);
            return this;
        }

        public CodeBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            builder.AppendFormat(format, arg0, arg1, arg2);
            return this;
        }

        public CodeBuilder AppendFormat(string format, params object[] args)
        {
            builder.AppendFormat(format, args);
            return this;
        }

        public CodeBuilder AppendLine()
        {
            builder.AppendLine();
            return this;
        }

        public CodeBuilder AppendLine(string value)
        {
            builder.AppendLine(value);
            return this;
        }

        public CodeBuilder Clear()
        {
            builder.Clear();
            return this;
        }

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            builder.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public bool Equals(CodeBuilder sb)
        {
            return builder.Equals(sb);
        }

        public CodeBuilder Insert(int index, bool value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, byte value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, char value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, char[] value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            builder.Insert(index, value, startIndex, charCount);
            return this;
        }

        public CodeBuilder Insert(int index, decimal value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, double value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, short value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, int value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, long value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, object value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, float value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, string value)
        {
            builder.Insert(index, value);
            return this;
        }

        public CodeBuilder Insert(int index, string value, int count)
        {
            builder.Insert(index, value, count);
            return this;
        }

        public CodeBuilder Remove(int startIndex, int length)
        {
            builder.Remove(startIndex, length);
            return this;
        }

        public CodeBuilder Replace(char oldChar, char newChar)
        {
            builder.Replace(oldChar, newChar);
            return this;
        }

        public CodeBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            builder.Replace(oldChar, newChar, startIndex, count);
            return this;
        }

        public CodeBuilder Replace(string oldValue, string newValue)
        {
            builder.Replace(oldValue, newValue);
            return this;
        }

        public CodeBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            builder.Replace(oldValue, newValue, startIndex, count);
            return this;
        }

        public string ToString(int index, int length)
        {
            return builder.ToString(index, length);
        }

        public int Length
        {
            get => builder.Length;
            set => builder.Length = value;
        }

        public int MaxCapacity => builder.MaxCapacity;

        public int Capacity
        {
            get => builder.Capacity;
            set => builder.Capacity = value;
        }
    }
    #endregion

    #region Multiple Inheritance with Interfaces
    public interface IBird
    {
        public int Weight { get; set; }
        void Fly();
    }

    public class Bird : IBird
    {
        public int Weight { get; set; }
        public void Fly()
        {
            Console.WriteLine($"Soring in the sky with weight: {Weight}");
        }
    }

    public interface ILizard
    {
        public int Weight { get; set; }
        void Crawl();
    }

    public class Lizard : ILizard
    {
        public int Weight { get; set; }
        public void Crawl()
        {
            Console.WriteLine($"Crawling in the dirt with weight: {Weight}");
        }
    }

    // When we add class dragon it cannot inherit from both classes due to multiple inheritance
    // So we can use decorator pattern to add the functionality of both classes to the dragon class
    public class Dragon : IBird, ILizard
    {
        private int weight;
        private Bird bird = new Bird();
        private Lizard lizard = new Lizard();
        public int Weight
        {
            get => weight;
            set
            {
                weight = value;
                bird.Weight = value;
                lizard.Weight = value;
            }
        }

        public void Crawl()
        {
            lizard.Crawl();
        }
        public void Fly()
        {
            bird.Fly();
        }
    }
    #endregion

    public interface ICreature
    {
        int Age { get; set; }
    }

    public interface IBird2 : ICreature
    {
        void Fly()
        {
            if (Age >= 10)
                Console.WriteLine("Flying");
        }
    }

    public interface ILizard2 : ICreature
    {
        void Crawl()
        {
            if (Age < 10)
                Console.WriteLine("Crawling");
        }
    }

    public class Organism
    {

    }

    public class Dragon2 : Organism, IBird2, ILizard2
    {
        public int Age { get; set; }
    }

    #region Detecting decorator cycle
    #region Dynamic decorator composition
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float radius;

        public Circle() { radius = 0; }

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }

        public string AsString() => $"A circle with radius {radius}";
    }

    public class Square : IShape
    {
        private float side;

        public Square() {  side = 0; }

        public Square(float side)
        {
            this.side = side;
        }

        public void Resize(float factor)
        {
            side *= factor;
        }

        public string AsString() => $"A square with side {side}";
    }

    public enum Color
    {
        Red, Green, Blue, Black
    }

    public abstract class ShapeDecorator : IShape
    {
        protected internal readonly List<Type> types = new();
        protected internal readonly IShape shape;

        public ShapeDecorator(IShape shape)
        {
            this.shape = shape;
            if (shape is ShapeDecorator sd)
                types.AddRange(sd.types);
        }

        public virtual string AsString()
        {
            return shape.AsString();
        }
    }

    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
        where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected readonly TCyclePolicy policy = new();
        protected ShapeDecorator(IShape shape) : base(shape)
        {
            if (policy.TypeAdditionAllowed(typeof(TSelf), types))
                types.Add(typeof(TSelf));
        }
    }

    public class ShapeDecoratorWithPolicy<T> : ShapeDecorator<T, ThrowOnCyclePolicy>
    {
        public ShapeDecoratorWithPolicy(IShape shape) : base(shape)
        {
        }
    }

    // Decorate at runtime
    public class ColoredShape
    : ShapeDecorator<ColoredShape, ThrowOnCyclePolicy>, IShape
    //: ShapeDecorator<ColoredShape, AbsorbCyclePolicy>, IShape
    //: ShapeDecorator<ColoredShape, CyclesAllowedPolicy>, IShape
    //: ShapeDecoratorWithPolicy<ColoredShape>
    {
        private readonly IShape shape;
        private readonly Color color;
        public ColoredShape(IShape shape, Color color) : base(shape)
        {
            this.shape = shape;
            this.color = color;
        }
        public override string AsString()
        {
            var sb = new StringBuilder($"{shape.AsString()}");
            if (policy.ApplicationAllowed(types[0], types.Skip(1).ToList()))
                sb.Append($" has the color {color}");

            return sb.ToString();
        }
    }

    public class TransparentShape : ShapeDecorator<TransparentShape, ThrowOnCyclePolicy>, IShape
    {
        private readonly IShape shape;
        private readonly float transparency;
        public TransparentShape(IShape shape, float transparency) : base(shape)
        {
            this.transparency = transparency;
        }

        public override string AsString() => $"{base.AsString()} has {transparency * 100.0f}% transparency";
    }
    #endregion

    public abstract class ShapeDecoratorCyclePolicy
    {
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }


    // To allow cycles
    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => true;
    }

    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool handler(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
                throw new InvalidOperationException("Cycle detected");
            return true;
        }
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);

        }
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);
        }
    }

    public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => !allTypes.Contains(type);
    }
    #endregion


    #region Static decorator composition
    //public class ColeredShape<T> : T // Curiously recurring template pattern (CRTP)
    public abstract class Shape
    {
        public abstract string AsString();
    }

    public class Triangle : Shape
    {
        private float height;
        private float width;

        public float Height { get => height; set => height = value; }
        public float Width { get => width; set => width = value; }

        public Triangle() { height = 0; width = 0; }

        public Triangle(float height, float width)
        {
            this.height = height;
            this.width = width;
        }

        public void Resize(float width, float height)
        {
            this.width *= width;
            this.height *= height;
        }

        public override string AsString() => $"A triangle with height {height} and width {width}";
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private Color color;
        private T shape = new T();

        public ColoredShape() : this(Color.Black)
        {
        }

        public ColoredShape(Color color)
        {
            this.color = color;
        }
        public override string AsString() => $"{shape.AsString()} has the color {color}";
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private float transparency;
        private T shape = new T();

        public TransparentShape() : this(0)
        {
        }

        public TransparentShape(float transparency)
        {
            this.transparency = transparency;
        }
        public override string AsString() => $"{shape.AsString()} has has {transparency * 100.0f}% transparency";
    } 
    #endregion

    public class Program
    {
        static void Main(string[] args)
        {
            #region Custom String Builder
            var cb = new CodeBuilder();
            cb.AppendLine("class Foo")
              .AppendLine("{")
              .AppendLine("}");
            Console.WriteLine(cb);
            #endregion

            #region String Problem
            var ss = "hello";
            ss += " world"; // When we do += it not add it to the original string,
                            // it creates a new string and assigns it to the variable
                            // This is because strings are immutable
                            // This wasting alot of memory 
            Console.WriteLine(ss);

            int iterations = 10000;

            Stopwatch sw = Stopwatch.StartNew();
            string s = "";
            for (int i = 0; i < iterations; i++)
            {
                s += "a"; // Creates a new string every time!
            }
            sw.Stop();
            Console.WriteLine($"String took: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iterations; i++)
            {
                sb.Append("a"); // Modifies the existing buffer
            }
            sw.Stop();
            Console.WriteLine($"StringBuilder took: {sw.ElapsedMilliseconds} ms");
            #endregion

            #region Adapter-Decorator
            CodeBuilder sb2 = "hello ";
            sb2 += "world"; // This is possible because of the implicit conversion from string to StringBuilder
            Console.WriteLine(sb2);
            #endregion

            #region Multiple Inheritance with Interfaces
            var d = new Dragon();
            d.Weight = 123;
            d.Fly();
            d.Crawl(); 
            #endregion

            Dragon2 d2 = new Dragon2 { Age = 5};
            if (d2 is IBird2 bird)
                bird.Fly();
            if (d2 is ILizard2 lizard)
                lizard.Crawl();


            #region Dynamic decorator composition
            var square = new Square(1.23f);
            Console.WriteLine(square.AsString());

            var redSquare = new ColoredShape(square, Color.Red);
            Console.WriteLine(redSquare.AsString());

            var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
            //var redHalfTransparentSquare2 = new TransparentShape(redHalfTransparentSquare, 0.5f); // This is a problem, because we don't have a way to prevent this, We fix it by adding restriction in the constructor
            Console.WriteLine(redHalfTransparentSquare.AsString());
            #endregion

            #region Detecting decorator cycle
            // There is one problem with dynamic decorator: problem is cicle or repeatition 
            var circle = new Circle(2);

            var colored1 = new ColoredShape(circle, Color.Red);
            //var colored2 = new ColoredShape(colored1, Color.Blue); // This is a problem, because we don't have a way to prevent this, We fix it by adding restriction in the constructor
            //var colored3 = new ColoredShape(colored2, Color.Green); // The problem happened in the constructor
            Console.WriteLine(colored1.AsString());
            //Console.WriteLine(colored3.AsString());
            #endregion

            #region Static decorator composition
            var redTriangle = new ColoredShape<Triangle>(Color.Red);
            //redTriangle.Height = 10; // This is a problem in C#, because we don't use inheritance so we don't have access to it.
            Console.WriteLine(redTriangle.AsString());
            var halfTransparentTriangle = new TransparentShape<Triangle>(0.5f);
            Console.WriteLine(halfTransparentTriangle.AsString()); 
            #endregion
        }
    }
}
