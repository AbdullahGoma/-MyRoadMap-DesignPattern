using Autofac;

namespace Bridge
{
    // Problem It Solves
    // Imagine you have:

    // Multiple abstractions(e.g., shapes like Circle, Square).
    // Multiple implementations(e.g., rendering methods like VectorRenderer, RasterRenderer).

    // Without the Bridge pattern, you might end up with classes like
    // VectorCircle, RasterCircle, VectorSquare, RasterSquare, etc.
    // This leads to exponential class growth when adding new abstractions or implementations.
    public interface IRenderer
    {
        void RenderCircle(float radius);
        void RenderSquare(float side);
    }

    // Vector rendering
    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
            => Console.WriteLine($"Drawing a circle of radius {radius} using vectors.");

        public void RenderSquare(float side)
            => Console.WriteLine($"Drawing a square of side {side} using vectors.");
    }

    // Raster rendering
    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
            => Console.WriteLine($"Drawing a circle of radius {radius} using pixels.");

        public void RenderSquare(float side)
            => Console.WriteLine($"Drawing a square of side {side} using pixels.");
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
            => this.renderer = renderer;

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float radius;
        public Circle(IRenderer renderer, float radius)
            : base(renderer)
            => this.radius = radius;
        public override void Draw()
            => renderer.RenderCircle(radius);

        public override void Resize(float factor)
            => radius *= factor;
    }

    public class Square : Shape
    {
        private float side;
        public Square(IRenderer renderer, float side)
            : base(renderer)
            => this.side = side;
        public override void Draw()
            => renderer.RenderSquare(side);

        public override void Resize(float factor)
            => side *= factor;
    }



    public class Program
    {
        static void Main(string[] args)
        {
            IRenderer vectorRenderer = new VectorRenderer();
            IRenderer rasterRenderer = new RasterRenderer();

            Shape circle = new Circle(vectorRenderer, 5);
            circle.Draw();
            circle.Resize(2);
            circle.Draw();

            Shape circle1 = new Circle(rasterRenderer, 5);
            circle1.Draw();
            circle1.Resize(2);
            circle1.Draw();

            Shape square = new Square(vectorRenderer, 10);
            square.Draw();
            square.Resize(3);
            square.Draw();

            Shape square1 = new Square(rasterRenderer, 10);
            square1.Draw();
            square1.Resize(3);
            square1.Draw();

            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();
            cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));

            using (var c = cb.Build())
            {
                var circle2 = c.Resolve<Circle>(new PositionalParameter(0, 5.0f));
                circle2.Draw();
                circle2.Resize(2);
                circle2.Draw();
            }
        }
    }
}
