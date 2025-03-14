using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace Composite
{
    public interface IFileSystemComponent
    {
        string Name { get; }
        void Display(string indent = "");
        long GetSize(); // Added for size calculation
    }

    public class File : IFileSystemComponent
    {
        public string Name { get; }
        public long Size { get; }

        public File(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}File: {Name} (Size: {Size} bytes)");
        }

        public long GetSize() => Size; // Return the file's size
    }

    public class SymbolicLink : IFileSystemComponent
    {
        public string Name { get; }
        public IFileSystemComponent Target { get; }

        public SymbolicLink(string name, IFileSystemComponent target)
        {
            Name = name;
            Target = target;
        }

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}Symbolic Link: {Name} -> {Target.Name}");
        }

        public long GetSize() => 0; // Symbolic links have negligible size
    }

    public class Folder : IFileSystemComponent
    {
        public string Name { get; }
        private List<IFileSystemComponent> _children = new();

        public Folder(string name)
            => Name = name;

        public void Add(IFileSystemComponent component)
            => _children.Add(component);

        public void Remove(IFileSystemComponent component)
            => _children.Remove(component);

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}Folder: {Name}");
            foreach (var child in _children)
                child.Display(indent + "  "); // Recursive call
        }

        public long GetSize()
        {
            return _children.Sum(child => child.GetSize()); // Sum all child sizes
        }
    }

    #region Geometric Shapes
    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;

        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
              .Append(string.IsNullOrWhiteSpace(Color) ? "" : $"{Color} ")
              .AppendLine(Name);
            foreach (var child in Children)
                child.Print(sb, depth + 1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";

    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    } 
    #endregion


    #region Neural Networks
    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }

    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;


        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }
    #endregion



    // Specification Pattern
    #region Specification Pattern
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Color = color;
            Size = size;
        }
    }
    public abstract class ISpecification<T>
    {
        public abstract bool IsSatisfied(T t);
        public static ISpecification<T> operator &(ISpecification<T> first, ISpecification<T> second)
        {
            return new AndSpecification<T>(first, second);
        }

        public static ISpecification<T> operator |(ISpecification<T> first, ISpecification<T> second)
        {
            return new OrSpecification<T>(first, second);
        }
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public override bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;
        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public override bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected readonly ISpecification<T>[] items;
        public CompositeSpecification(params ISpecification<T>[] items)
        {
            this.items = items;
        }
    }

    // Combinator
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params ISpecification<T>[] items) : base(items)
        {
        }

        public override bool IsSatisfied(T t)
        {
            // Any -> OrSpecification
            return items.All(i => i.IsSatisfied(t));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params ISpecification<T>[] items) : base(items)
        {
        }

        public override bool IsSatisfied(T t)
        {
            // Return true if ANY of the specifications are satisfied
            return items.Any(i => i.IsSatisfied(t));
        }
    }


    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class BetterFilter : IFilter<Product>
    {

        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
                if (spec.IsSatisfied(item))
                    yield return item;
        }
    } 
    #endregion


    public class Program
    {
        static void Main(string[] args)
        {
            var root = new Folder("Root");
            var folder1 = new Folder("Folder1");
            var folder2 = new Folder("Folder2");

            var file1 = new File("File1.txt", 100);
            var file2 = new File("File2.txt", 200);

            var linkToFile2 = new SymbolicLink("LinkToFile2", file2);
            var linkToFolder2 = new SymbolicLink("LinkToFolder2", folder2);

            folder1.Add(file1);
            folder1.Add(linkToFile2);
            folder2.Add(file2);

            root.Add(folder1);
            root.Add(folder2);
            root.Add(linkToFolder2);

            // Display the structure
            Console.WriteLine("File System Structure:");

            // In first call, it will display the root folder and
            // then it will call the display method of each child folder (Recursive)
            // then foreach file in folder it will call the display method in the file
            root.Display();

            // Calculate sizes
            Console.WriteLine("\nTotal Sizes:");
            Console.WriteLine($"Root: {root.GetSize()} bytes");
            Console.WriteLine($"Folder1: {folder1.GetSize()} bytes");
            Console.WriteLine($"Folder2: {folder2.GetSize()} bytes");
            Console.WriteLine($"LinkToFolder2: {linkToFolder2.GetSize()} bytes");


            #region Geometric Shapes
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circle { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new Circle { Color = "Blue" });
            group.Children.Add(new Square { Color = "Blue" });
            drawing.Children.Add(group);

            Console.WriteLine(drawing); 
            #endregion

            #region Neural Networks
            //var neuron1 = new Neuron();
            //var neuron2 = new Neuron();
            //neuron1.ConnectTo(neuron2);

            //var layer1 = new NeuronLayer();
            //var layer2 = new NeuronLayer();

            //neuron1.ConnectTo(layer1);
            //layer1.ConnectTo(layer2);
            #endregion

            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            var products = new List<Product> { apple, tree, house };

            var filter = new BetterFilter();

            Console.WriteLine("Green products:");
            var greenSpec = new ColorSpecification(Color.Green);
            foreach (var p in filter.Filter(products, greenSpec))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("\nLarge products:");
            var largeSpec = new SizeSpecification(Size.Large);
            foreach (var p in filter.Filter(products, largeSpec))
            {
                Console.WriteLine($" - {p.Name} is large");
            }

            Console.WriteLine("\nLarge and green products:");
            var largeGreenSpec = new AndSpecification<Product>(greenSpec, largeSpec);
            foreach (var p in filter.Filter(products, largeGreenSpec))
            {
                Console.WriteLine($" - {p.Name} is large and green");
            }

            Console.WriteLine("Green OR Large products:");
            var greenSpeci = new ColorSpecification(Color.Green);
            var largeSpeci = new SizeSpecification(Size.Large);

            var greenOrLargeSpec = new OrSpecification<Product>(greenSpeci, largeSpeci);
            //var greenOrLargeSpec = greenSpeci | largeSpeci; // Using the operator

            foreach (var p in filter.Filter(products, greenOrLargeSpec))
            {
                Console.WriteLine($" - {p.Name} is green or large");
            }
        }
    }
}
