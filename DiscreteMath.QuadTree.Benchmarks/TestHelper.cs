using DiscreteMath;
using System.Drawing;

namespace Benchmarks;

public class TestHelper
{
    public static List<Circle> GenerateRandomNonIntersectingCircles
    (
        Rect bounds,
        int count,
        double minRadius,
        double maxRadius,
        int maxAttemptsPerCircle = 10_000
    )
    {
        var random = new Random(1);
        var circles = new List<Circle>(count);

        while (circles.Count < count)
        {
            var radius = random.NextDouble() * (maxRadius - minRadius) + minRadius;
            var x = random.NextDouble() *
                    (bounds.Max.X - bounds.Min.X - 2 * radius) +
                    bounds.Min.X + radius;

            var y = random.NextDouble() *
                    (bounds.Max.Y - bounds.Min.Y - 2 * radius) +
                    bounds.Min.Y + radius;

            var candidate = new Circle(new Vector2D(x, y), radius);
            var ok = circles.All(c => !candidate.Intersects(c));

            if (ok)
            {
                circles.Add(candidate);
                continue;
            }

            if (--maxAttemptsPerCircle <= 0)
                throw new InvalidOperationException("Не удалось разместить круги без пересечений");
        }

        return circles;
    }

    public static Vector2D[] GenerateRandomPoints(Rect bounds, int count)
    {
        var random = new Random(2);
        var points = new Vector2D[count];

        for (var i = 0; i < count; i++)
        {
            var x = bounds.Min.X + random.NextDouble() * bounds.Width;
            var y = bounds.Min.Y + random.NextDouble() * bounds.Height;

            points[i] = new Vector2D(x, y);
        }

        return points;
    }

    public static Circle? LinearSearch(IReadOnlyList<Circle> circles, Vector2D point)
    {
        foreach (var c in circles)
            if (c.Contains(point))
                return c;

        return null;
    }
}