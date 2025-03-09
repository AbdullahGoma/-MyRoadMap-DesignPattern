namespace Adapter
{
    // Vector2f, Vector3i
    public interface IInteger
    {
        int Value { get; }
    }

    public static class Dimensions
    {
        public class Two : IInteger
        {
            public int Value => 2;
        }

        public class Three : IInteger
        {
            public int Value => 3;
        }
    }


    public class Vector<TSelf, Type, Dimension>
    where Dimension : IInteger, new()
    where TSelf : Vector<TSelf, Type, Dimension>, new() // new() == Default Constructor
        // Operator+ can not run on generic types
        // TSelf => Recursive Generic
    {
        protected Type[] data;
        public Vector()
        {
            data = new Type[new Dimension().Value];
        }

        public Vector(params Type[] values)
        {
            var requiredSize = new Dimension().Value;
            data = new Type[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
                data[i] = values[i];
        }
        // Factory Method pattern
        //public static Vector<Type, Dimension> Create(params Type[] values) // Vector<Type, Dimension> this will not work except => recursive generic
        //{
        //    return new Vector<Type, Dimension>(values);
        //}
        // Factory Method pattern
        public static TSelf Create(params Type[] values) // TSelf => Recursive Generic
        {
            var result = new TSelf();
            var requiredSize = new Dimension().Value;
            result.data = new Type[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
                result.data[i] = values[i];
            return result;
        }

        public Type this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public Type X
        {
            get => data[0];
            set => data[0] = value;
        }
    }

    public class VectorOfFloat<TSelf, Dimension> : Vector<TSelf, float, Dimension> 
        where Dimension : IInteger, new() 
        where TSelf : Vector<TSelf, float, Dimension>, new()
    {

    }



    public class VectorOfInt<Dimension> : Vector<VectorOfInt<Dimension>, int, Dimension> where Dimension : IInteger, new()
    {
        public VectorOfInt()
        {
        }

        public VectorOfInt(params int[] values) : base(values)
        {
        }

        public static VectorOfInt<Dimension> operator + (VectorOfInt<Dimension> lhs, VectorOfInt<Dimension> rhs)
        {
            var result = new VectorOfInt<Dimension>();
            var dimension = new Dimension().Value;
            for (int i = 0; i < dimension; i++)
            {
                result[i] = lhs[i] + rhs[i];
            }
            return result;
        }
    }

    public class Vector2i : VectorOfInt<Dimensions.Two>
    {
        public Vector2i()
        {
        }

        public Vector2i(params int[] values) : base(values)
        {
        }
    }

    public class Vector3f : VectorOfFloat<Vector3f, Dimensions.Three>
    {
        public override string ToString()
        {
            return $"{string.Join(",", data)}";
        }
    }

    class GenericValueAdapter
    {

    }
}
