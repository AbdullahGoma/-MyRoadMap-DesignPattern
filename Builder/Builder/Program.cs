using System.Text;

namespace Builder
{
    #region HtmlElement
    public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            Name = name;
            Text = text;
        }

        // Formatted string representation with proper indentation
        private string ToStringImp1(int indent)
        {
            var sb = new StringBuilder();
            string indentation = new string(' ', indentSize * indent);

            sb.AppendLine($"{indentation}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.AppendLine($"{new string(' ', indentSize * (indent + 1))}{Text}");
            }

            if (Elements.Any())
            {
                sb.Append(string.Join("", Elements.Select(e => e.ToStringImp1(indent + 1)))); // It go to into this function again with Name, Text that comes from e(element from foreach) 
            }

            sb.AppendLine($"{indentation}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImp1(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string rootName;
        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement() { Name = rootName };
        }
    }
    #endregion


    #region Fluent Builder Inheritance with Recursive Generics
    //public class Person
    //{
    //    public string Name;
    //    public string Position;

    //    public class Builder : PersonJobBuilder<Builder>
    //    {

    //    }

    //    public static Builder New => new Builder();

    //    public override string ToString()
    //    {
    //        return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
    //    }
    //}

    //public abstract class PersonBuilder
    //{
    //    protected Person person = new Person();

    //    public Person Build() => person;
    //}

    //public class PersonInfoBuilder<SELF> : PersonBuilder where SELF : PersonInfoBuilder<SELF>
    //{
    //    public SELF Called(string name)
    //    {
    //        person.Name = name;
    //        return (SELF)this;
    //    }
    //}

    //public class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>> where SELF : PersonJobBuilder<SELF>
    //{
    //    public SELF WorksAsA(string position)
    //    {
    //        person.Position = position;
    //        return (SELF)this;
    //    }
    //}
    #endregion

    #region Stepwise Builder
    //public enum CarType
    //{
    //    Sedan,
    //    Crossover
    //}

    //public class Car
    //{
    //    public CarType Type;
    //    public int WheelSize;
    //}

    //public interface ISpecifyCarType
    //{
    //    ISpecifyWheelSize OfType(CarType type);
    //}

    //public interface ISpecifyWheelSize
    //{
    //    IBuildCar WithWheels(int size);
    //}

    //public interface IBuildCar
    //{
    //    public Car Build();
    //}

    //public class CarBuilder
    //{
    //    private class Imp1 : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
    //    {
    //        private Car car = new Car();
    //        public ISpecifyWheelSize OfType(CarType type)
    //        {
    //            car.Type = type;
    //            return this;
    //        }

    //        public IBuildCar WithWheels(int size)
    //        {
    //            switch (car.Type)
    //            {
    //                case CarType.Crossover when size < 17 || size > 20:
    //                case CarType.Sedan when size < 15 || size > 17:
    //                    throw new ArgumentException("Wrong size of wheel for {car.Type}.");
    //            }
    //            car.WheelSize = size;
    //            return this;
    //        }
    //        public Car Build()
    //        {
    //            return car;
    //        }

    //    }
    //    public static ISpecifyCarType Create()
    //    {
    //        return new Imp1();
    //    }
    //} 
    #endregion


    #region Functional Builder
    //public class Person
    //{
    //    public string Name, Position;
    //}

    ////public sealed class PersonBuilder // sealed => U can not inherit from it.
    ////{
    ////    private readonly List <Func<Person, Person>> actions = new List <Func<Person, Person>> ();

    ////    public PersonBuilder Called(string name) => Do(p => p.Name = name);
    ////    public Person Build() => actions.Aggregate(new Person(), (p, f) => f(p));

    ////    public PersonBuilder Do(Action<Person> action) => AddAction(action);

    ////    private PersonBuilder AddAction(Action<Person> action)
    ////    {
    ////        actions.Add(p => { action(p); return p; });
    ////        return this;
    ////    }
    ////}

    //public abstract class FunctionalBuilder<TSubject, TSelf>
    //    where TSelf : FunctionalBuilder<TSubject, TSelf>
    //    where TSubject : new()
    //{
    //    private readonly List<Func<Person, Person>> actions = new List<Func<Person, Person>>();

    //    public Person Build() => actions.Aggregate(new Person(), (p, f) => f(p));

    //    public TSelf Do(Action<Person> action) => AddAction(action);

    //    private TSelf AddAction(Action<Person> action)
    //    {
    //        actions.Add(p => { action(p); return p; });
    //        return (TSelf)this;
    //    }
    //}

    //public sealed class PersonBuilder : FunctionalBuilder<Person, PersonBuilder>
    //{
    //    public PersonBuilder Called(string name) => Do(p => p.Name = name);
    //}

    //public static class PersonBuilderExtensions
    //{
    //    public static PersonBuilder WorksAs(this PersonBuilder builder, string position) => builder.Do(p => p.Position = position);

    //} 
    #endregion

    public class Person
    {
        // address
        public string StreetAddress, Postcode, City;

        // employment 
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"Address: {StreetAddress}, {City}, {Postcode}\n" +
                   $"Employment: {Position} at {CompanyName}\n" +
                   $"Annual Income: ${AnnualIncome}";
        }
    }

    public class PersonBuilder // Facade
    {
        // referance
        protected Person person = new();

        public PersonJobBuilder Works => new(person);
        public PersonAddressBuilder Lives => new(person);

        public static implicit operator Person(PersonBuilder personBuilder)
        {
            return personBuilder.person;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        // might not work with a value type
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAdress)
        {
            person.StreetAddress = streetAdress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            //var hello = "Hello";
            //var sb = new StringBuilder();
            //sb.Append("<p>");
            //sb.Append(hello);
            //sb.Append("</p>");
            //Console.WriteLine(sb.ToString());

            //var words = new[] { "Hello", "World!" };
            //sb.Clear();
            //sb.Append("<ul>");
            //foreach (var word in words)
            //{
            //    sb.AppendFormat("<li>{0}</li>\n", word);
            //}
            //sb.Append("</ul>");
            //Console.WriteLine(sb.ToString());


            #region HtmlElement
            //var builder = new HtmlBuilder("ul");
            //builder.AddChild("li", "Hello").AddChild("li", "World!"); // Fluent Builder
            //Console.WriteLine(builder.ToString()); 
            #endregion

            #region Fluent Builder Inheritance with Recursive Generics
            //var Dal = Person.New.Called("Abdullah").WorksAsA("Software Engineer").Build();
            //Console.WriteLine(Dal); 
            #endregion

            #region Stepwise Builder
            //var car = CarBuilder.Create() // ISpecifyCarType
            //    .OfType(CarType.Crossover) // ISpecifyWheelSize
            //    .WithWheels(18) // IBuildCar
            //    .Build(); 
            #endregion

            #region Functional Builder
            //var person = new PersonBuilder().Called("Abdullah").WorksAs("Web Developer").Build(); 
            #endregion

            // Faceted Builder
            var personBuilder = new PersonBuilder();
            Person person = personBuilder
                .Works.At("Home").AsA("Software Engineer").Earning(150000)
                .Lives.At("MitSalsil").In("Dakahlia").WithPostcode("12345");
            Console.WriteLine(person);
        }
    }
}
