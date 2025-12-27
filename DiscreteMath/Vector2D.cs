namespace DiscreteMath;

public readonly record struct Vector2D(double X, double Y)
{
    public double Length => Math.Sqrt(LengthSquare);
    public double LengthSquare => X * X + Y * Y;
    
    public static Vector2D operator +(Vector2D left, Vector2D right) => new(left.X + right.X, left.Y + right.Y);
    public static Vector2D operator -(Vector2D left, Vector2D right) => new(left.X - right.X, left.Y - right.Y);
    public static Vector2D operator *(Vector2D v, double k) => new(v.X * k, v.Y * k);
}
