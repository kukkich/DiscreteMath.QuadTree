using BenchmarkDotNet.Attributes;
using DiscreteMath;

namespace Benchmarks;

public class OneMillionPointBenchmarks
{
    private const int PointCount = 1_000_000;

    [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
    public int _n;

    private readonly Rect _bounds = new
    (
        new Vector2D(-100_000, -100_000),
        new Vector2D(100_000, 100_000)
    );

    private QuadTree _quadTree = null!;
    private IReadOnlyCollection<Vector2D> _points = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var circles = TestHelper.GenerateRandomNonIntersectingCircles(_bounds, _n, 2, 2);

        _quadTree = new QuadTree(
            _bounds,
            capacityPerNode: 2,
            maxDepth: _n);

        foreach (var c in circles)
            _quadTree.AddCircle(c);

        _points = TestHelper.GenerateRandomPoints(_bounds, PointCount);
    }

    [Benchmark]
    public void PointsSearch()
    {
        foreach (var point in _points)
        {
            _quadTree.Query(point);
        }
    }
}