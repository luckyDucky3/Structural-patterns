namespace Bridge;

public abstract class Shape
{
    protected IRenderer renderer;

    public Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

public interface IRenderer
{
    void RenderCircle(float diametr);
    void RenderSquare(float side);
}

public class VectorRenderer : IRenderer
{
    public void RenderCircle(float diametr)
    {
        Console.WriteLine($"Рисуем круг диаметром {diametr} в векторном формате");
    }

    public void RenderSquare(float side)
    {
        Console.WriteLine($"Рисуем квадрат со стороной {side} в векторном формате");
    }
}

public class RasterRenderer : IRenderer
{
    public void RenderCircle(float diametr)
    {
        Console.WriteLine($"Рисуем круг диаметром {diametr} в растровом формате");
    }

    public void RenderSquare(float side)
    {
        Console.WriteLine($"Рисуем квадрат со стороной {side} в растровом формате");
    }
}

public class Circle : Shape
{
    private float diametr;

    public Circle(IRenderer renderer, float diametr) : base(renderer)
    {
        this.diametr = diametr;
    }

    public override void Draw()
    {
        renderer.RenderCircle(diametr);
    }
}

public class Square : Shape
{
    private float side;

    public Square(IRenderer renderer, float side) : base(renderer)
    {
        this.side = side;
    }

    public override void Draw()
    {
        renderer.RenderSquare(side);
    }
}

public static class ShapeFactory
{
    public static Circle CreateCircle(IRenderer renderer, float diametr)
    {
        return new Circle(renderer, diametr);
    }
    
    public static Square CreateSquare(IRenderer renderer, float side)
    {
        return new Square(renderer, side);
    }
}

class Program
{
    static void DrawAllShapes(params Shape[] shapes)
    {
        foreach (var shape in shapes)
        {
            shape.Draw();
        }
    }
    static void Main()
    {
        IRenderer vectorRenderer = new VectorRenderer();
        IRenderer rasterRenderer = new RasterRenderer();
        
        var vectorCircle = ShapeFactory.CreateCircle(vectorRenderer, 5);
        var vectorSquare = ShapeFactory.CreateSquare(vectorRenderer, 5);
        
        var rasterCircle = ShapeFactory.CreateCircle(rasterRenderer, 4);
        var rasterSquare = ShapeFactory.CreateSquare(rasterRenderer, 4);
        
        DrawAllShapes(vectorCircle, vectorSquare, rasterCircle, rasterSquare);
    }
}