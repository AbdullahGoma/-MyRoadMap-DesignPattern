using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Prototype
{
    //public interface IPrototype<T>
    //{
    //    T DeepCopy();
    //}
    //public class Address : IPrototype<Address> //: ICloneable
    //{
    //    public string StreetName;
    //    public int HouseNumber;

    //    public Address(string streetName, int houseNumber)
    //    {
    //        this.StreetName = streetName;
    //        this.HouseNumber = houseNumber;
    //    }

    //    /// <summary>
    //    /// Copy Constructor - Deep Copy! 
    //    /// </summary>
    //    /// <param name="other"></param>
    //    public Address(Address other)
    //    {
    //        StreetName = other.StreetName;
    //        HouseNumber = other.HouseNumber;
    //    }

    //    /// <summary>
    //    /// Deep Copy!
    //    /// </summary>
    //    /// <returns></returns>
    //    //public object Clone()
    //    //{
    //    //    return new Address(StreetName, HouseNumber);
    //    //}

    //    public override string ToString()
    //    {
    //        return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
    //    }

    //    public Address DeepCopy()
    //    {
    //        return new Address(StreetName, HouseNumber);
    //    }
    //}
    //public class Person : IPrototype<Person> //: ICloneable
    //{
    //    public int Id;
    //    public string[] Names;
    //    public Address Address;

    //    public Person(int id, string[] names, Address address)
    //    {
    //        this.Id = id;
    //        this.Names = names;
    //        this.Address = address;
    //    }

    //    /// <summary>
    //    /// Copy Constructor - Deep Copy! 
    //    /// </summary>
    //    /// <param name="other"></param>
    //    public Person(Person other)
    //    {
    //        Id = other.Id;
    //        Names = other.Names;
    //        //Address = other.Address; // Shallow Copy
    //        Address = new Address(other.Address);
    //    }

    //    //public object Clone()
    //    //{
    //    //    return new Person(Id, (string[])Names.Clone(), (Address)Address.Clone());
    //    //}

    //    public override string ToString()
    //    {
    //        return $"{nameof(Id)}: {Id}, {nameof(Names)}: {string.Join(" ", Names)}, {nameof(Address)}: {Address}";
    //    }

    //    public Person DeepCopy()
    //    {
    //        return new Person(Id, (string[])Names.Clone(), Address.DeepCopy());
    //    }
    //}

    #region Prototype Inheritance
    //public interface IDeepCopyable<T> where T : new() // new() is a constraint that says T must have a parameterless constructor
    //{
    //    void CopyTo(T target);
    //    public T DeepCopy()
    //    {
    //        T t = new T();
    //        CopyTo(t); // Recursive deep copy
    //        return t;
    //    }
    //}

    //public class Address : IDeepCopyable<Address>
    //{
    //    public string StreetName;
    //    public int HouseNumber;

    //    public Address()
    //    {

    //    }

    //    public Address(string streetName, int houseNumber)
    //    {
    //        StreetName = streetName;
    //        HouseNumber = houseNumber;
    //    }

    //    public void CopyTo(Address target)
    //    {
    //        target.StreetName = StreetName;
    //        target.HouseNumber = HouseNumber;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
    //    }
    //}

    //public class Person : IDeepCopyable<Person>
    //{
    //    public string[] Names;
    //    public Address Address;

    //    public Person()
    //    {

    //    }

    //    public Person(string[] names, Address address)
    //    {
    //        Names = names;
    //        Address = address;
    //    }

    //    public void CopyTo(Person target)
    //    {
    //        target.Names = (string[])Names.Clone();
    //        target.Address = Address.DeepCopy();
    //    }

    //    public override string ToString()
    //    {
    //        return $"{nameof(Names)}: {string.Join(" ", Names)}, {nameof(Address)}: {Address}";
    //    }
    //}

    //public class Employee : Person, IDeepCopyable<Employee>
    //{
    //    public int Salary;

    //    public Employee()
    //    {

    //    }

    //    public Employee(string[] names, Address address, int salary) : base(names, address)
    //    {
    //        Salary = salary;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
    //    }

    //    public void CopyTo(Employee target)
    //    {
    //        base.CopyTo(target);
    //        target.Salary = Salary;
    //    }
    //}

    //public static class ExtensionMethods
    //{
    //    public static T DeepCopy<T>(this T self) where T : IDeepCopyable<T>, new()
    //    {
    //        return self.DeepCopy();
    //    }

    //    public static T DeepCopy<T>(this Person person) where T : new()
    //    {
    //        return ((IDeepCopyable<T>)person).DeepCopy();
    //    }
    //} 
    #endregion


    public static class ExtensionMethods
    {
        // Deep copy using JSON serialization
        public static T DeepCopy<T>(this T self)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true, // Enable field serialization
                WriteIndented = true  // Optional: makes the JSON pretty
            };

            var json = JsonSerializer.Serialize(self, options);
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static T DeepCopyXML<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                return (T)s.Deserialize(ms);
            }
        }
    }

    [JsonSerializable(typeof(Address))]
    public class Address
    {
        public string StreetName;
        public int HouseNumber;

        public Address() // parameterless constructor must in Deep Copy XML
        {

        }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }
    }

    [JsonSerializable(typeof(Person))]
    public class Person 
    {
        public string[] Names;
        public Address Address;

        public Person() // parameterless constructor must in Deep Copy XML
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(" ", Names)}, {nameof(Address)}: {Address}";
        }
    }

    [JsonSerializable(typeof(Employee))]
    public class Employee : Person
    {
        public int Salary;

        public Employee() // parameterless constructor must in Deep Copy XML
        {

        }

        public Employee(string[] names, Address address, int salary) : base(names, address)
        {
            Salary = salary;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
        }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            //var john = new Person(1, new[] { "John", "Smith" }, new Address("London Road", 123));

            //var jane = (Person)john.Clone();
            //jane.Id = 2;
            //jane.Names[0] = "Jane";
            //jane.Address.HouseNumber = 321;

            //Console.WriteLine(john);
            //Console.WriteLine(jane); 


            // Copy Constructor
            //var john = new Person(1, new[] { "John", "Smith" }, new Address("London Road", 123));
            //var jane = new Person(john);
            //jane.Id = 2;
            //jane.Names[0] = "Jane";
            //jane.Address.HouseNumber = 321;
            //Console.WriteLine(john);
            //Console.WriteLine(jane);

            // Deep Copy Interface
            //var john = new Person(1, new[] { "John", "Smith" }, new Address("London Road", 123));
            //var jane = john.DeepCopy();
            //jane.Id = 2;
            //jane.Names[0] = "Jane";
            //jane.Address.HouseNumber = 321;
            //Console.WriteLine(john);
            //Console.WriteLine(jane);

            //var john = new Employee();
            //john.Names = new[] { "John", "Smith" };
            //john.Address = new Address()
            //{
            //    HouseNumber = 123,
            //    StreetName = "London Road"
            //};
            //john.Salary = 100000;

            //var jane = john.DeepCopy();

            ////Employee e = john.DeepCopy<Employee>();
            ////Person p = john.DeepCopy<Person>();

            //jane.Names[0] = "Jane";
            //jane.Address.HouseNumber = 321;
            //jane.Salary = 200000;

            //Console.WriteLine(john);
            //Console.WriteLine(jane);

            var john = new Person(new[] { "John", "Smith" }, new Address("London Road", 123));
            var jane = john.DeepCopyXML();
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;
            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
}
