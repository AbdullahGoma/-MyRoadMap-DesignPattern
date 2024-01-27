using System.Diagnostics;

namespace SingleResponsibilty
{
    // Class is Responsiple for one thing...
    public class Journal
    {
        private readonly List<string> entries = new();

        private static int count = 0;   

        public int AddEntry(string entry)
        {
            entries.Add($"{++count}: {entry}");
            return count;
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }


        //public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        //{
        //    if (overwrite || !File.Exists(filename)) 
        //        File.WriteAllText(filename, journal.ToString());
        //}


    }


    public class Persistence
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, journal.ToString());
        }
    }
 


    internal class Program
    {
        static void Main(string[] args)
        {
            var journal = new Journal();
            journal.AddEntry("I write code now");
            journal.AddEntry("I ate egg today");
            Console.WriteLine(journal);


            var persistance = new Persistence();
            var filename = @"F:\ITI-Course\13- Design Pattern\SOLID Design Principle\test.txt";
            persistance.SaveToFile(journal, filename, true);
            new Process { StartInfo = new ProcessStartInfo(filename) { UseShellExecute = true } }.Start();
            //Process.Start(filename);

        }
    }
}