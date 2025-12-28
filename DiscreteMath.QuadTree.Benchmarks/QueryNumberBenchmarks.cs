using BenchmarkDotNet.Attributes;
using DiscreteMath;

namespace Benchmarks;

public class QueryNumberBenchmarks
{
    private const int CirclesCount = 100_000;

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int _queryCount;

    private readonly Rect _bounds = new
    (
        new Vector2D(-100_000, -100_000),
        new Vector2D(100_000, 100_000)
    );

    private IReadOnlyList<Circle> _circles = null!;
    private IReadOnlyCollection<Vector2D> _points = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _circles = TestHelper.GenerateRandomNonIntersectingCircles(_bounds, CirclesCount, 2, 2);
        _points = TestHelper.GenerateRandomPoints(_bounds, _queryCount);
    }

    [Benchmark(Description = "Linear search")]
    public void LinearSearch()
    {
        foreach (var point in _points)
        {
            TestHelper.LinearSearch(_circles, point);
        }
    }

    [Benchmark(Description = "Quad tree building and search")]
    public void QuadTreeSearch()
    {
        var quadTree = new QuadTree(
            _bounds,
            capacityPerNode: 2,
            maxDepth: CirclesCount);

        foreach (var c in _circles)
            quadTree.AddCircle(c);

        foreach (var point in _points)
        {
            quadTree.Query(point);
        }
    }
}