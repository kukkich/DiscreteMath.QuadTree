namespace DiscreteMath;

public class QuadTree(Rect bounds, int capacityPerNode, int maxDepth)
{
    private readonly List<Circle> _circles = [];
    private readonly QuadTreeNode _root = new(bounds, capacityPerNode, 0, maxDepth);

    public int AddCircle(in Circle circle)
    {
        var index = _circles.Count;
        _circles.Add(circle);
        _root.Insert(index, _circles);
        return index;
    }

    public Circle? Query(Vector2D point)
    {
        var index = _root.Query(point, _circles);
        return index switch
        {
            not null => _circles[index.Value],
            _ => null,
        };
    }

    public IReadOnlyList<Circle> Circles => _circles;
}