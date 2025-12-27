using DiscreteMath;

var bounds = new Rect(
    new Vector2D(-1, -1),
    new Vector2D(8, 8));

// var circles = GenerateRandomNonIntersectingCircles(
//     bounds,
//     count: 10_000,
//     minRadius: 2,
//     maxRadius: 5);

var circles = CreateUniformCircles();

IReadOnlyList<Circle> CreateUniformCircles()
{
    var circles = new List<Circle>();
    
    for (var row = 0; row < 8; row+= 2)
    {
        for (var column = 0; column < 8; column += 2)
        {
            circles.Add(new Circle(new Vector2D(row, column), 1));
        }
    }
    
    return circles;
}

var quadTree = new QuadTree(
    bounds,
    capacityPerNode: 2,
    maxDepth: 20);

foreach (var c in circles)
    quadTree.AddCircle(c);

var t1 = quadTree.Query(new Vector2D(-1, -1)); // not
var t2 = quadTree.Query(new Vector2D(0, 0)); // has
var t3 = quadTree.Query(new Vector2D(1, 1)); // not
var t4 = quadTree.Query(new Vector2D(4, 6)); // has
var t5 = quadTree.Query(new Vector2D(3, 5)); // not
var t6 = quadTree.Query(new Vector2D(double.Sqrt(2)/2d - 0.001, double.Sqrt(2)/2d - 0.001)); // has

// for (var i = 0; i < 10; i++)
// {
//     var fromTree = quadTree.Query(point);
//     var fromLinear = LinearSearch(circles, point);
//
//     Console.WriteLine(
//         $"Point {point}: " +
//         $"QuadTree={(fromTree is null ? "none" : "hit")}, " +
//         $"Linear={(fromLinear is null ? "none" : "hit")}");
// }


Console.WriteLine("Hello, World!");
return;

static List<Circle> GenerateRandomNonIntersectingCircles(
    Rect bounds,
    int count,
    double minRadius,
    double maxRadius,
    int maxAttemptsPerCircle = 10_000)
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

        var ok = true;
        foreach (var c in circles)
        {
            if (candidate.Intersects(c))
            {
                ok = false;
                break;
            }
        }

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

static Circle? LinearSearch(
    IReadOnlyList<Circle> circles,
    Vector2D point)
{
    foreach (var c in circles)
        if (c.Contains(point))
            return c;

    return null;
}