using System.Text;
using System.Threading.Tasks;

namespace Factories
{
    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    // Constructors are not good
    //public class Point
    //{
    //    private double x, y;

    //    //public Point(double x, double y)
    //    //{
    //    //    this.x = x;
    //    //    this.y = y;
    //    //}

    //    //public Point(double rho, double theta)


    //    /// <summary>
    //    /// Initializes a point from EITHER cartesian or polar 
    //    /// </summary>
    //    /// <param name="a">x if cartesian, rho if polar</param>
    //    /// <param name="b">y if cartesian, theta if polar</param>
    //    /// <param name="system">Type of Coordinate.</param>
    //    /// <exception cref="ArgumentOutOfRangeException"></exception>
    //    public Point(double a, double b, 
    //        CoordinateSystem system = CoordinateSystem.Cartesian) // You still tied with the name Point
    //    {
    //        switch (system)
    //        {
    //            case CoordinateSystem.Cartesian:
    //                x = a;
    //                y = b;
    //                break;
    //            case CoordinateSystem.Polar:
    //                x = a * Math.Cos(b);
    //                y = a * Math.Sin(b);
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException(nameof(system), system, null);
    //        }
    //    }
    //}

    #region Factory Ex.
    //public static class PointFactory
    //{
    //    // Facatory Method Design Pattern
    //    public static Point NewCartisianPoint(double x, double y)
    //    {
    //        return new Point(x, y);
    //    }

    //    public static Point NewPolarPoint(double rho, double theta)
    //    {
    //        return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
    //    }

    //}

    //public class Point
    //{
    //    private double x, y;

    //    public Point(double x, double y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
    //    }
    //} 
    #endregion

    public class Foo
    {
        private Foo()
        {

        }

        public async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }

        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }

    public interface ITheme
    {
        string TextColor { get; }
        string BgrColor { get; } // Background
    }

    class LightTheme : ITheme
    {
        public string TextColor => "black";
        public string BgrColor => "white";
    }

    class DarkTheme : ITheme
    {
        public string TextColor => "white";
        public string BgrColor => "dark gray";
    }

    public class TrackingThemeFactory
    {
        private readonly List<WeakReference<ITheme>> _themes = new();
        public ITheme CreateTheme(bool dark)
        {
            ITheme theme = dark ? new DarkTheme() : new LightTheme();
            _themes.Add(new WeakReference<ITheme>(theme));
            return theme;
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var reference in _themes)
                {
                    if (reference.TryGetTarget(out var theme))
                    {
                        bool dark = theme is DarkTheme;
                        sb.Append(dark ? "Dark" : "Light").AppendLine(" theme");
                    }
                }
                return sb.ToString();
            }
        }
    }


    #region Object Tracking and Bulk Replacement
    public class Ref<T> where T : class
    {
        public T Value;

        public Ref(T value)
        {
            Value = value;
        }
    }

    public class ReplaceableThemeFactory
    {
        private readonly List<WeakReference<Ref<ITheme>>> themes = new();
        private ITheme CreateThemeImp1(bool dark)
        {
            return dark ? new DarkTheme() : new LightTheme();
        }

        public Ref<ITheme> CreateTheme(bool dark)
        {
            var r = new Ref<ITheme>(CreateThemeImp1(dark));
            themes.Add(new(r));
            return r;
        }

        public void ReplaceTheme(bool dark)
        {
            foreach (var weakReference in themes)
            {
                if (weakReference.TryGetTarget(out var referance))
                {
                    referance.Value = CreateThemeImp1(dark);
                }
            }
        }
    }
    #endregion


    #region Inner Factory

    public class Point
    {
        private double X, Y;

        private Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        public static Point Origin => new(0, 0);
        public static Point Origin2 = new(0, 0); // Better

        public class Factory
        {
            // Facatory Method Design Pattern
            public static Point NewCartisianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }

        }
    } 
    #endregion


    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This coffee is sensational!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        //public enum AvailableDrink
        //{
        //    Coffee, Tea
        //}

        //private Dictionary<AvailableDrink, IHotDrinkFactory> _factories = new();

        //public HotDrinkMachine()
        //{
        //    foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
        //    {
        //        var factory = (IHotDrinkFactory)Activator
        //                 .CreateInstance(Type.GetType("Factories." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")); // Factories.TeaFactory
        //        _factories.Add(drink, factory);
        //    }
        //}

        //public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        //{
        //    return _factories[drink].Prepare(amount);
        //}

        private List<Tuple<string, IHotDrinkFactory>> _factories = new();
        public HotDrinkMachine()
        {
            foreach (var type in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(type) && !type.IsInterface) // All classes that implement IHotDrinkFactory
                {
                    // Ex:Factories.TeaFactory
                    // Tee, new TeaFactory()
                    _factories.Add(Tuple.Create(type.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(type)));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            Console.WriteLine("Available Drinks: ");
            for (int i = 0; i < _factories.Count; i++)
            {
                var factory = _factories[i];
                Console.WriteLine($"{i}: {factory.Item1}");
            }
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null && int.TryParse(s, out int i) && i >= 0 && i < _factories.Count)
                {
                    Console.WriteLine("Specify amount: ");
                    s = Console.ReadLine();
                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                    {
                        return _factories[i].Item2.Prepare(amount);
                    }
                }
                Console.WriteLine("Incorrect input, try again!");
            }
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            Console.WriteLine(point);

            //var foo = new Foo();
            //await foo.InitAsync();

            Foo foo = await Foo.CreateAsync();

            var factory = new TrackingThemeFactory();
            var theme1 = factory.CreateTheme(false);
            var theme2 = factory.CreateTheme(true);
            Console.WriteLine(factory.Info);

            var fac2 = new ReplaceableThemeFactory();   
            var magicTheme = fac2.CreateTheme(true);
            Console.WriteLine(magicTheme.Value.BgrColor);
            fac2.ReplaceTheme(false);
            Console.WriteLine(magicTheme.Value.BgrColor);

            // Abstract Factory
            //var machine = new HotDrinkMachine();
            //var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 100);
            //drink.Consume();

            // Abstract Factory Without breaking OCP
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}
