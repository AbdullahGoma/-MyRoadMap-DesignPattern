using System.Diagnostics;
using System.IO.Pipes;

namespace OpenClosed
{
    // Open-CLosed Principle(OCP): Software entities(classes, modules, functions, etc.)
    // should be open for extension, but closed for modification

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


    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (Product product in products)
                if (product.Size == size) yield return product;
        }
        
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (Product product in products)
                if (product.Color == color) yield return product;
        }

        public IEnumerable<Product> FilterBySizeandColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (Product product in products)
                if (product.Color == color && product.Color == color) yield return product;
        }
    }


    // Specification Pattern
    public interface ISpecification<T> 
    {
        bool IsSatisfied(T t);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product t)
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
        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> first, second;
        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;   
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
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
                if(spec.IsSatisfied(item))
                    yield return item;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = {apple, tree, house };
            var productFilter = new ProductFilter();
            Console.WriteLine("Green Produts (old):");

            foreach (Product product in productFilter.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {product.Name} is green");
            }

            var productFilterBetter = new BetterFilter();
            Console.WriteLine("Green Produts (new):");
            foreach (var product in productFilterBetter.Filter(products, new ColorSpecification(Color.Green)))
                Console.WriteLine($" - {product.Name} is green");

            Console.WriteLine("Large blue items:");
            foreach (var product in productFilterBetter.Filter(products, new AndSpecification<Product>(
                new ColorSpecification(Color.Blue),
                new SizeSpecification(Size.Large))))
                Console.WriteLine($" - {product.Name} is big and blue");
            
        }
    }
}