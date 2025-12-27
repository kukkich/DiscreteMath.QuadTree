namespace DiscreteMath;

public readonly record struct Circle(Vector2D Center, double Radius)
{
    public bool Contains(Vector2D p)
    {
        var d = p - Center;
        return d.LengthSquare <= Radius * Radius;
    }
    
    public bool Intersects(in Circle other)
    {
        var d = Center - other.Center;
        var r = Radius + other.Radius;
        return d.LengthSquare < r * r;
    }
}