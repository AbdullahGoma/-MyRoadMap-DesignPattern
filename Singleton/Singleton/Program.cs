using Autofac;
using MoreLinq;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);        
    }

    public class SingletonDatabase : IDatabase 
        // We don't need more instance from this, because everytime we make instance, it will read the file again
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount; // To check how many instance we have
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            instanceCount++;
            Console.WriteLine("Initializing database");
            var directory = new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName
                ?? throw new InvalidOperationException("Directory name is null.");
            capitals = File.ReadAllLines(Path.Combine(directory, "capitals.txt"))
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );

        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        public static SingletonDatabase Instance => instance.Value;
    }

    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        public OrdinaryDatabase()
        {
            Console.WriteLine("Initializing database");
            var directory = new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName
                ?? throw new InvalidOperationException("Directory name is null.");
            capitals = File.ReadAllLines(Path.Combine(directory, "capitals.txt"))
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );

        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }

    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += SingletonDatabase.Instance.GetPopulation(name); // Hard coding the SingletonDatabase here
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    public class ConfigurableRecordFinder
    {
        private IDatabase database;
        public ConfigurableRecordFinder(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(paramName: nameof(database));
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += database.GetPopulation(name); 
            return result;
        }
    }



    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2)); // Check if db and db2 is the same instance
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1)); // Check if the instance is only 1
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }

        [Test]
        public void ConfigurablePopuableTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(4));
        }

        [Test]
        public void DIPopuableTest()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<OrdinaryDatabase>().As<IDatabase>().SingleInstance();
            cb.RegisterType<ConfigurableRecordFinder>();
            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurableRecordFinder>();
            }
        }

        [Test]
        public void MonostateTest()
        {
            var ceo = new CEO();
            ceo.Name = "Adam Smith";
            ceo.Age = 55;
            var ceo2 = new CEO();
            //Assert.That(ceo, Is.SameAs(ceo2)); // Fails here, because the Monostate pattern shares state (via static fields), not instance identity.
            Assert.That(ceo2.Age, Is.EqualTo(55)); // This passes because static fields (name and age) are shared across all CEO instances.
        }
    }

    // MonoState Pattern
    // Shared State: All instances of CEO share the same static fields (name, age).
    // Unique Instances: Each new CEO() creates a new object, but the state is global.
    public class CEO
    {
        private static string name;
        private static int age;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public override string ToString()
        {
            return $"{nameof(CEO)}: {nameof(name)}: {name}, {nameof(age)}: {age}";
        }
    }

    public sealed class PerThreadSingleton
    {
        private static ThreadLocal<PerThreadSingleton> threadInstance = 
                       new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton());
        public int Id;
        private PerThreadSingleton()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public static PerThreadSingleton Instance => threadInstance.Value;
    }

    // Ambient Context Pattern
    public sealed class BuildingContext : IDisposable
    {
        public int WallHeight;
        private static Stack<BuildingContext> stack = new();

        static BuildingContext() 
        {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1)
                stack.Pop();
        }
    }

    public class Building
    {
        public List<Wall> Walls = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
                sb.AppendLine(wall.ToString());
            return sb.ToString();
        }
    }

    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Wall
    {
        public Point Start, End;
        public int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Height)}: {Height}";
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.Instance;
            var city = "Seoul";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");

            // MonoState Pattern
            var ceo = new CEO();
            ceo.Name = "Adam Smith";
            ceo.Age = 55;

            var ceo2 = new CEO(); // Refers to the same data, because the properties are mapped to static fields
            Console.WriteLine(ceo2);

            // PerThreadSingleton
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t1: {PerThreadSingleton.Instance.Id}");
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t2: {PerThreadSingleton.Instance.Id}");
                Console.WriteLine($"t2: {PerThreadSingleton.Instance.Id}");
            }); // Two Instances are the same
            Task.WaitAll(t1, t2); // t1 and t2 are different instances

            // Ambient Context Pattern
            var house = new Building();
            // Ground Floor 3000 
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                // First Floor 3500
                using (new BuildingContext(3500)) 
                {

                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }

                // ground Floor 3000
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }

            Console.WriteLine(house);
        }
    }
}
