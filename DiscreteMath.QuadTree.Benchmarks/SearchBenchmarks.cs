using BenchmarkDotNet.Attributes;
using DiscreteMath;

namespace Benchmarks;

public class SearchBenchmarks
{
    private const int PointCount = 1_000_000;

    [Params(10, 100, 1000, 10_000)]
    public int _n;

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
        _circles = TestHelper.GenerateRandomNonIntersectingCircles(_bounds, _n, 2, 2);
        _points = TestHelper.GenerateRandomPoints(_bounds, PointCount);
    }

    [Benchmark]
    public void LinearSearch()
    {
        foreach (var point in _points)
        {
            TestHelper.LinearSearch(_circles, point);
        }
    }
}