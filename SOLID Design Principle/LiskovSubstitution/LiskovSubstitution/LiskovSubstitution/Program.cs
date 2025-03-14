namespace LiskovSubstitution
{
    //public class Bird
    //{
    //    public virtual void Fly()
    //    {
    //        Console.WriteLine("The bird is flying.");
    //    }
    //}

    //public class Penguin : Bird
    //{
    //    public override void Fly()
    //    {
    //        throw new InvalidOperationException("Penguins can't fly!");
    //    }
    //}

    // You should able to substitute a base type for a sub type
    public abstract class Bird
    {
        public abstract void Move();
    }

    public class FlyingBird : Bird
    {
        public override void Move()
        {
            Console.WriteLine("The bird is flying.");
        }
    }

    public class Penguin : Bird
    {
        public override void Move()
        {
            Console.WriteLine("The penguin is swimming.");
        }
    }

    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public Rectangle()
        {

        }
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            set
            {
                base.Width = base.Height = value;
            }
        }
        public override int Height
        {
            set
            {
                base.Height = base.Width = value;
            }
        }
    }


    public class Program
    {
        static public int Area(Rectangle r) => r.Width * r.Height;
        static void Main(string[] args)
        {
            //Bird bird = new Penguin();
            //bird.Fly(); // Throws an exception!

            //Bird bird1 = new FlyingBird();
            //bird1.Move(); 

            //Bird bird2 = new Penguin();
            //bird2.Move(); 

            Rectangle rectangle = new Rectangle(2, 3);
            Console.WriteLine($"{rectangle} has area {Area(rectangle)}");

            Square square = new Square();
            square.Width = 4;
            Console.WriteLine($"{square} has area {Area(square)}");

            // The problem is Square must behave as square even if it has another reference
            Rectangle sq = new Square();
            sq.Width = 4;
            sq.Height = 5;
            Console.WriteLine($"{sq} has area {Area(sq)}");

            // The solve is make Rectangle props virtual, and override it in Square and it Solved
        }
    }
}
