using System.Diagnostics;

namespace SingleResponsibilty
{
    

    public class Program
    {
        // Every Class should have just a single reason to change
        // We applied separation of concerns
        public class Journal // Concerns with entries
        {
            private readonly List<string> entries = new();
            private static int count = 0;

            public int AddEntry(string text)
            {
                entries.Add($"{++count}: {text}");
                return count; // momento pattern
            }

            public void RemoveEntry(int index)
            {
                entries.RemoveAt(index);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, entries);
            }
        }
         
        public class Persistence // Concerns with saving object
        {
            public void SaveToFile(Journal journal, string filename, bool otherwise = false)
            {
                if (otherwise || !File.Exists(filename))
                    File.WriteAllText(filename, journal.ToString());
            }
        }
        static void Main(string[] args)
        {
            var journal = new Journal();
            journal.AddEntry("I Study Today!");
            journal.AddEntry("I ate Potato!");

            //Console.WriteLine(journal);
            var persistence = new Persistence();
            var filename = @"F:\My RoadMap\13- Design Pattern\SOLID Design Principle\test.txt";
            persistence.SaveToFile(journal, filename, true);
            // Open the file with the default text editor
            ProcessStartInfo startInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
    }
}