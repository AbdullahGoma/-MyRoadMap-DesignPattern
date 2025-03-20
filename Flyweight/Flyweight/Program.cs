using JetBrains.dotMemoryUnit;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Text;

namespace Flyweight
{
    public class User
    {
        private string fullName;
        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }




    //var user1 = new User2("John Doe");
    //var user2 = new User2("Jane Doe");
    //var user3 = new User2("John Smith");
    // Object	names (Indexes)	         static List<string> strings (Shared)
    // user1    [0, 1]                   ["John", "Doe"]
    // user2    [2, 1]                   ["John", "Doe", "Jane"]
    // user3    [0, 3]                   ["John", "Doe", "Jane", "Smith"]
    public class User2
    {
        static List<string> strings = new(); // The strings list is static, meaning it belongs to the class itself,
                                             // not to individual objects.
                                             // Since static variables are shared across all instances of User2,
                                             // every User2 object refers to the same list of stored strings.
                                             // This allows User2 instances to reuse already stored names
                                             // instead of creating duplicate strings.

        private int[] names; // Saving indeces instead of saving names(string) 
        public int[] Names => names;

        public User2(string fullName)
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1; // index of last element
                }
            }

            names = fullName.Split(' ').Select(getOrAdd).ToArray(); // Split the full name into first and last names
                                                                    // and store their indexes in the names array. 
        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));

        public static void PrintSharedMemory()
        {
            Console.WriteLine("\nShared Name Parts:");
            foreach (var name in strings)
                Console.WriteLine($"- {name}");
        }

        public void DebugPrint()
        {
            Console.WriteLine($"Indexes: {string.Join(", ", names)} → FullName: {FullName}");
        }

        public static void PrintTotalNamesLength(List<User2> users)
        {
            int totalLength = users.Sum(u => u.Names.Length);
            Console.WriteLine($"Total Length of All Names: {totalLength}");
            foreach (var user in users)
            {
                Console.WriteLine(string.Join(", ", user.Names));
            }
        }

    }

    #region Text Formatting
    public class FormattedText
    {
        private readonly string plainText;
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i <= end; i++)
                capitalize[i] = true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }
            return sb.ToString();
        }
    } 

    public class BetterFromattedText
    {
        private string plainText;
        private List<TextRange> formatting = new List<TextRange>();
        public BetterFromattedText(string plainText)
        {
            this.plainText = plainText;
        }
        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);
            return range;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                foreach (var range in formatting)
                    if (range.Covers(i) && range.Capitalize)
                        c = char.ToUpper(c);
                sb.Append(c);
            }
            return sb.ToString();
        }
        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;
            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }
    }

    #endregion

    [TestFixture]
    public class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User2("John Doe");
            var user2 = new User2("Jane Doe");
            var user3 = new User2("John Smith");

            user1.DebugPrint();
            user2.DebugPrint();
            user3.DebugPrint();

            User2.PrintSharedMemory();

            var users = new List<User2>
            {
                new User2("John Doe"),
                new User2("Jane Doe"),
                new User2("John Smith"),
                new User2("Alice Johnson")
            };

            foreach (var user in users)
            {
                user.DebugPrint();
            }

            // Print total length of all names
            User2.PrintTotalNamesLength(users);

            #region Text Formatting
            var formattedText = new FormattedText("This is a brave new world");
            formattedText.Capitalize(10, 15);
            Console.WriteLine(formattedText);

            var betterFormattedText = new BetterFromattedText("This is a brave new world");
            betterFormattedText.GetRange(10, 15).Capitalize = true;
            betterFormattedText.GetRange(16, 19).Capitalize = true;
            Console.WriteLine(betterFormattedText);
            #endregion
        }

        [DotMemoryUnit(FailIfRunWithoutSupport = false)]  // Prevents crashes if dotMemory is missing
        [Test]
        public void TestUser()
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();
            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User($"{firstName} {lastName}"));
            ForceGC();

            dotMemory.Check(memory =>
            {
                Console.WriteLine(memory.SizeInBytes);
            });

        }

        [DotMemoryUnit(FailIfRunWithoutSupport = false)]  // Prevents crashes if dotMemory is missing
        [Test]
        public void TestUser2()
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var users = new List<User2>();
            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User2($"{firstName} {lastName}"));
            ForceGC();

            dotMemory.Check(memory =>
            {
                Console.WriteLine(memory.SizeInBytes);
            });
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private string RandomString()
        {
            var rand = new Random();
            return new string(Enumerable.Range(0, 10)
                .Select(i => (char)('a' + rand.Next(26)))
                .ToArray());
        }
    }
}
