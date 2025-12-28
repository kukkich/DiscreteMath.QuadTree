namespace DiscreteMath;

public readonly record struct Rect(Vector2D Min, Vector2D Max)
{
    public double Width => Max.X - Min.Y;
    public double Height => Max.Y - Min.Y;

    public bool ContainsPoint(Vector2D p)
    {
        return p.X >= Min.X && p.X <= Max.X &&
               p.Y >= Min.Y && p.Y <= Max.Y;
    }

    public bool IntersectsCircle(in Circle circle)
    {
        var c = new Vector2D
        (
            Math.Clamp(circle.Center.X, Min.X, Max.X), 
            Math.Clamp(circle.Center.Y, Min.Y, Max.Y)
        );

        var d = c - circle.Center;
        return d.LengthSquare <= circle.Radius * circle.Radius;
    }
}