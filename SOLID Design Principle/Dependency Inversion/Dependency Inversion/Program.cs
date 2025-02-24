using System.Linq;

namespace Dependency_Inversion
{
    //public class FileLogger
    //{
    //    public void Log(string message)
    //    {
    //        Console.WriteLine($"Logging to file: {message}");
    //    }
    //}

    //public class OrderService
    //{
    //    private readonly FileLogger _logger;

    //    public OrderService()
    //    {
    //        _logger = new FileLogger(); // High-level depends on low-level
    //    }

    //    public void PlaceOrder()
    //    {
    //        Console.WriteLine("Order placed.");
    //        _logger.Log("Order placed successfully.");
    //    }
    //}

    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Logging to file: {message}");
        }
    }

    public class OrderService
    {
        private readonly ILogger _logger;

        public OrderService(ILogger logger) // Inject abstraction
        {
            _logger = logger;
        }

        public void PlaceOrder()
        {
            Console.WriteLine("Order placed.");
            _logger.Log("Order placed successfully.");
        }
    }

    public enum RelationShip
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
        //public DateTime DateOfBirth;
    }

    public interface IRelationShipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }
    // Low-level
    public class RelationShips : IRelationShipBrowser
    {
        private List<(Person, RelationShip, Person)> relations = new List<(Person, RelationShip, Person)> ();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, RelationShip.Parent, child));
            relations.Add((child, RelationShip.Child, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations.Where(x => x.Item1.Name == name && x.Item2 == RelationShip.Parent)
                .Select(rel => rel.Item3);
        }

        //public List<(Person, RelationShip, Person)> Relations => relations;
    }

    public class Program
    {
        //public Program(RelationShips relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach (var rel in relations.Where(x => x.Item1.Name == "Gomaa" && x.Item2 == RelationShip.Parent))
        //    {
        //        Console.WriteLine($"Gomaa has a child called {rel.Item3.Name}");
        //    } 
        //}

        public Program(IRelationShipBrowser browser)
        {
            foreach (var person in browser.FindAllChildrenOf("Gomaa"))
            {
                Console.WriteLine($"Gomaa has a child called: {person.Name}");
            }
        }
        static void Main(string[] args)
        {
            // High level part of the system should not depend on low level part of the system directly, instead
            // it should depent on abstraction
            ILogger logger = new FileLogger();  
            var orderService = new OrderService(logger);
            orderService.PlaceOrder();


            var child1 = new Person { Name = "Abdullah" };
            var child2 = new Person { Name = "Esmail" };
            var parent = new Person { Name = "Gomaa" };

            var relationships = new RelationShips();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);
            new Program(relationships);
        }
    }
}
