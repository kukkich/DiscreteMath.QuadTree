using BenchmarkDotNet.Attributes;
using DiscreteMath;

namespace Benchmarks;

public class TreeBuildingBenchmarks
{
    [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
    public int _n;

    private readonly Rect _bounds = new
    (
        new Vector2D(-100_000, -100_000),
        new Vector2D(100_000, 100_000)
    );

    private IReadOnlyList<Circle> _circles = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _circles = TestHelper.GenerateRandomNonIntersectingCircles(_bounds, _n, 2, 2);
    }

    [Benchmark]
    public void LinearSearch()
    {
        var quadTree = new QuadTree(
            _bounds,
            capacityPerNode: 2,
            maxDepth: _n);

        foreach (var c in _circles)
            quadTree.AddCircle(c);
    }
}