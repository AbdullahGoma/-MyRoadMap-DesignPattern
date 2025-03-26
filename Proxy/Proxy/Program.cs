
using ImpromptuInterface;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Proxy
{
    #region Protection Proxy
    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("Car is being driven");
        }
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    public class CarProxy : ICar
    {
        private Driver driver;
        private Car car = new();

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }

        public void Drive()
        {
            if (driver.Age >= 16) car.Drive();
            else Console.WriteLine("Too younge");
        }
    }
    #endregion

    #region Property Proxy
    public class Property<T> : IEquatable<Property<T>?> where T : new()
    {
        private T value;

        public T Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value)) return;
                Console.WriteLine($"Assigning value to {value}");
                this.value = value;
            }
        }

        public Property() : this(default(T)) { }
        public Property(T value) { this.value = value; }

        public static implicit operator T(Property<T> property) => property.value; // int n = property_int;
        public static implicit operator Property<T>(T value) => new Property<T>(value); // Property<int> p = 123;

        public bool Equals(Property<T>? other) => other is not null && EqualityComparer<T>.Default.Equals(value, other.value);
        public override int GetHashCode() => value.GetHashCode();
    }


    public class Creature
    {
        private Property<int> agility = new Property<int>();
        public int Agility
        {
            get => agility.Value;
            set => agility.Value = value;
        }
    }
    #endregion


    #region Lazy Initialization of Properties
    public class LazyProperty<T> where T : new()
    {
        private T value;
        private bool isInitialized = false;

        public T Value
        {
            get
            {
                if (!isInitialized)
                {
                    Console.WriteLine("[INFO] Initializing value...");
                    value = new T();
                    isInitialized = true;
                }
                return value;
            }
        }
    }

    public class DataLoader
    {
        public LazyProperty<List<string>> Data { get; set; } = new LazyProperty<List<string>>();
    }
    #endregion

    #region Value Proxy
    // Storing of percentage values
    [DebuggerDisplay("{value*100}%")]
    public struct Percentage : IEquatable<Percentage>
    {
        private readonly float value;
        public Percentage(float value)
        {
            this.value = value;
        }

        public static float operator *(float f, Percentage p) => f * p.value;
        public static Percentage operator +(Percentage a, Percentage b) => new Percentage(a.value + b.value);

        public static bool operator ==(Percentage left, Percentage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Percentage left, Percentage right)
        {
            return !(left == right);
        }

        public override string ToString() => $"{value * 100}%";

        public override bool Equals(object? obj)
        {
            return obj is Percentage percentage && Equals(percentage);
        }

        public bool Equals(Percentage other)
        {
            return value == other.value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value);
        }
    }

    public static class PercentageExtensions
    {
        public static Percentage Percent(this float value) => new Percentage(value / 100.0f);
        public static Percentage Percent(this int value) => new Percentage(value / 100.0f);
    }
    #endregion

    #region Composite Proxy SoAAoS
    public class Creature1
    {
        public byte Age { get; set; }
        public int X, Y;
    }

    // To do all of Ages in memory like in array, Xs in memory like in array, Ys in memory like in array
    // Age X Y Age X Y Age X Y
    // Age Age Age
    // X X X
    // Y Y Y
    public class Creatures
    {
        private readonly int size;
        private byte[] age;
        private int[] x, y;
        public Creatures(int size)
        {
            this.size = size;
            age = new byte[size];
            x = new int[size];
            y = new int[size];
        }

        public struct CreatureProxy
        {
            private Creatures creatures;
            private int index;
            public CreatureProxy(Creatures creatures, int index)
            {
                this.creatures = creatures;
                this.index = index;
            }
            public ref byte Age => ref creatures.age[index];
            public ref int X => ref creatures.x[index];
            public ref int Y => ref creatures.y[index];
        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < size; pos++)
            {
                yield return new CreatureProxy(this, pos);
            }
        }
    }
    #endregion

    #region Composite Proxy with Array-Backed Properties
    public class MasonrySettings
    {
        //public bool? All
        //{
        //    get
        //    {
        //        if (Pillars == Walls && Walls == Floors) return Pillars;
        //        return null;
        //    }
        //    set
        //    {
        //        if (!value.HasValue) return;
        //        Pillars = value.Value;
        //        Walls = value.Value;
        //        Floors = value.Value;
        //    }
        //}
        //public bool Pillars, Walls, Floors;

        // Array Backed Properties

        private bool[] flags = new bool[3];
        public bool Pillars
        {
            get => flags[0];
            set => flags[0] = value;
        }

        public bool Walls
        {
            get => flags[1];
            set => flags[1] = value;
        }

        public bool Floors
        {
            get => flags[2];
            set => flags[2] = value;
        }

        public bool? All
        {
            get
            {
                if (flags.Skip(1).All(f => f == flags[0])) return flags[0]; // Skip the first element and check if all elements are equal to the first element
                return null;
            }
            set
            {
                if (!value.HasValue) return;
                for (int i = 0; i < flags.Length; i++)
                {
                    flags[i] = value.Value;
                }
            }
        }
    }
    #endregion

    #region Dynamic Proxy for Logging
    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private int balance;
        private int overdraftLimit = -500;
        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
        }
        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }
        public override string ToString() => $"Balance: {balance}";
    }

    public class Log<T> : DynamicObject where T : class, new()
    {
        private readonly T subject;
        private Dictionary<string, int> methodCallCount = new Dictionary<string, int>();

        public Log(T subject)
        {
            this.subject = subject;
        }

        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
            {
                throw new ArgumentException("I must be an interface type");
            }

            return new Log<T>(new T()).ActLike<I>();
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            try
            {
                Console.WriteLine($"Invoking {subject.GetType().Name}.{binder.Name} with arguments [{string.Join(",", args)}]");
                if (methodCallCount.ContainsKey(binder.Name))
                {
                    methodCallCount[binder.Name]++;
                }
                else
                {
                    methodCallCount.Add(binder.Name, 1);
                }

                result = subject.GetType().GetMethod(binder.Name)?.Invoke(subject, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var keyValue in methodCallCount)
                {
                    sb.AppendLine($"{keyValue.Key} called {keyValue.Value} time(s)");
                }
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return $"{Info}\n{subject}";
        }
    }
    #endregion

    #region View Model(MVVM Pattern)
    public class Person
    {
        public string FirstName, LastName;
    }

    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Person person;
        public PersonViewModel(Person person)
        {
            this.person = person;
        }

        public string FirstName
        {
            get => person.FirstName;
            set
            {
                if (person.FirstName == value) return;
                person.FirstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => person.LastName;
            set
            {
                if (person.LastName == value) return;
                person.LastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    FirstName = LastName = string.Empty;
                    return;
                }

                var items = value.Split(' ');
                FirstName = items[0];
                LastName = string.Join(" ", items.Skip(1)); // Ensures multi-word last names work
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion

    #region Bit Fragging
    public enum Operator : byte
    {
        [Description("*")]
        Multiplication = 0,
        [Description("/")]
        Division = 1,
        [Description("+")]
        Addition = 2,
        [Description("-")]
        Subtraction = 3,
    }

    public static class OpImp1
    {
        static OpImp1()
        {
            var type = typeof(Operator);
            foreach (Operator op in Enum.GetValues(type))
            {
                MemberInfo[] memInfo = type.GetMember(op.ToString());
                if (memInfo.Length > 0)
                {
                    var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        opNames[op] = ((DescriptionAttribute)attrs[0]).Description[0];
                    }
                }
            }
        }

        private static readonly Dictionary<Operator, char> opNames = new Dictionary<Operator, char>();

        // notice the data types
        private static readonly Dictionary<Operator, Func<double, double, double>> opImp1 =
            new Dictionary<Operator, Func<double, double, double>>()
            {
                [Operator.Multiplication] = ((x, y) => x * y),
                [Operator.Division] = ((x, y) => x / y),
                [Operator.Addition] = ((x, y) => x + y),
                [Operator.Subtraction] = ((x, y) => x - y)
            };

        public static double Call(this Operator op, int x, int y)
        {
            return opImp1[op](x, y);
        }

        public static char Name(this Operator op)
        {
            return opNames[op];
        }
    }

    public class TwoBitSet
    {
        // 64 bits --> 32 values
        private readonly ulong data;
        public TwoBitSet(ulong data)
        {
            this.data = data;
        }

        // 00 10 01 01
        public byte this[int index]
        {
            get
            {
                // 00 10 01 01
                var shift = index << 1;
                ulong mask = (0b11U << shift); // 00 11 00 00 

                // 00 10 00 00 >> shift
                // 00 00 00 00 00 00 00 00 10

                return (byte)((data & mask) >> shift);
            }
        }
    }

    public class Problem
    {
        // 1 3 5 7
        // Op.Add Op.Mul Op.Add
        private readonly List<int> numbers;
        private readonly List<Operator> ops;

        public Problem(IEnumerable<int> numbers, IEnumerable<Operator> ops)
        {
            this.numbers = new List<int>(numbers);
            this.ops = new List<Operator>(ops);
        }

        public int Eval()
        {
            var opGroups = new[]
            {
                new[] {Operator.Multiplication, Operator.Division},
                new[] {Operator.Addition, Operator.Subtraction},
            };

        startAgain:
            foreach (var group in opGroups)
            {
                for (int idx = 0; idx < ops.Count; idx++)
                {
                    if (group.Contains(ops[idx]))
                    {
                        var op = ops[idx];
                        double result = op.Call(numbers[idx], numbers[idx + 1]);

                        if (result != (int)result) return int.MaxValue;
                        // 1 3 5 7
                        // + * +
                        numbers[idx] = (int)result;
                        numbers.RemoveAt(idx + 1);
                        ops.RemoveAt(idx);
                        if (numbers.Count == 1) return numbers[0];
                        goto startAgain;
                    }
                }
            }

            return numbers[0];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int i = 0;

            for (; i < ops.Count; i++)
            {
                sb.Append(numbers[i]);
                sb.Append(ops[i].Name());
            }

            sb.Append(numbers[i]);
            return sb.ToString();
        }
    } 
    #endregion

    public class Program
    {
        static void Main(string[] args)
        {
            ICar car = new CarProxy(new Driver(16));
            car.Drive();

            #region Property Proxy
            var creature = new Creature();
            creature.Agility = 10; // creature.set_Agility(10);
                                   // creature.Agility = new Property<int>(10);
            creature.Agility = 10;
            creature.Agility = 11;
            #endregion

            #region Lazy Initialization of Properties
            var loader = new DataLoader();
            Console.WriteLine("Before accessing Data");
            var data = loader.Data.Value;  // [INFO] Initializing value...
            Console.WriteLine("After accessing Data");
            #endregion


            #region Value Proxy
            Console.WriteLine(10f * 5.Percent());
            Console.WriteLine(2.Percent() + 3.Percent());
            #endregion

            #region Composite Proxy SoAAoS
            var creatures = new Creature1[100]; // Array of structures "AoS"
            // Age X Y Age X Y Age X Y
            //foreach (var c in creatures)
            //{
            //    c.X++;
            //}

            // Age Age Age
            // X X X
            // Y Y Y
            var creatures2 = new Creatures(100); // Structure of array "SoA"
            foreach (Creatures.CreatureProxy c in creatures2)
            {
                c.X++;
            }
            #endregion

            #region Dynamic Proxy for Logging
            //var ba = new BankAccount();
            var ba = Log<BankAccount>.As<IBankAccount>();
            ba.Deposit(100);
            ba.Withdraw(50);
            Console.WriteLine(ba);
            #endregion

            #region View Model(MVVM Pattern)
            Person person = new Person { FirstName = "John", LastName = "Doe" };

            // Create the ViewModel and subscribe to PropertyChanged event
            PersonViewModel viewModel = new PersonViewModel(person);
            viewModel.PropertyChanged += (sender, args) =>
            {
                Console.WriteLine($"Property '{args.PropertyName}' has changed!");
            };

            // Modify properties and see PropertyChanged event firing
            Console.WriteLine($"Initial Full Name: {viewModel.FullName}");

            viewModel.FirstName = "Jane";
            viewModel.LastName = "Smith";
            Console.WriteLine($"Updated Full Name: {viewModel.FullName}");

            viewModel.FullName = "Michael Johnson";
            Console.WriteLine($"Updated via FullName: {viewModel.FullName}");
            #endregion

            #region Bit Fragging
            // 1 3 5 7
            // 1-3-5+7
            // 0 1 2 ... 10
            // * / + - 
            // 00 01 10 11

            // * 
            // 00 00 00 00 00 00 00 00 00
            // 00 00 00 00 00 00 00 00 01
            // 00 00 00 00 00 00 00 00 10
            // 00 00 00 00 00 00 00 00 11
            // 00 00 00 00 00 00 00 01 00
            var numbers = new[] { 1, 3, 5, 7 };
            var numberOfOps = numbers.Length - 1;

            for (int result = 0; result <= 10; ++result)
            {
                for (var key = 0UL; key < (1UL << (2 * numberOfOps)); ++key)
                {
                    var tbs = new TwoBitSet(key);
                    var ops = Enumerable.Range(0, numberOfOps).Select(i => (Operator)tbs[i]).ToArray();

                    var problem = new Problem(numbers.ToArray(), ops); // Use a copy of `numbers`

                    int evalResult = problem.Eval();

                    if (evalResult == result)
                    {
                        Console.WriteLine($"{new Problem(numbers, ops)} = {result}");
                    }
                }
            }
            #endregion
        }
    }
}
