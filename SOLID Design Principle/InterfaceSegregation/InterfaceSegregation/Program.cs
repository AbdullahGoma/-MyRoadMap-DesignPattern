namespace InterfaceSegregation
{
    public class Document
    {

    }

    public interface IMachine
    {
        void Print(Document document);
        void Scan(Document document);
        void Fax(Document document);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document document)
        {
            //
        }

        public void Print(Document document)
        {
            //
        }

        public void Scan(Document document)
        {
            //
        }
    }

    public class OldFashionedPrinter : IMachine
    {
        public void Print(Document document)
        {
            //
        }

        // Thats breaks our principle because we implement functions we don't need <================
        public void Scan(Document document)
        {
            throw new NotImplementedException();
        }
        public void Fax(Document document)
        {
            throw new NotImplementedException();
        }

    }

    // Solution
    public interface IPrinter
    {
        void Print(Document document);
    }
    public interface IScanner
    {
        void Scan(Document document);
    }
    public interface IFaxer
    {
        void Fax(Document document);    
    }

    public class PhotoCopier : IPrinter, IScanner
    {
        public void Print(Document document)
        {
            //
        }

        public void Scan(Document document)
        {
            //
        }
    }

    public interface IMultiFunctionMachine : IScanner, IPrinter
    {

    }

    public class MultiFunctionMachine : IMultiFunctionMachine
    {
        private IPrinter printer;
        private IScanner scanner;
        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            this.printer = printer;
            this.scanner = scanner;
        }
        public void Print(Document document)
        {
            printer.Print(document);
        }

        public void Scan(Document document)
        {
            scanner.Scan(document);
            // Decorator
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
