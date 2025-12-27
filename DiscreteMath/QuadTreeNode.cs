namespace DiscreteMath;

public record QuadTreeNode(Rect Bounds, int Capacity, int Depth, int MaxDepth)
{
    private readonly List<int> _circles = [];
    private QuadTreeNode[]? _children;

    public void Insert(int circleIndex, IReadOnlyList<Circle> circlesMap)
    {
        if (_children is not null)
        {
            InsertIntoChildren(circleIndex, circlesMap);
            return;
        }

        _circles.Add(circleIndex);

        if (_circles.Count > Capacity && Depth < MaxDepth)
            Split(circlesMap);
    }

    private void Split(IReadOnlyList<Circle> circlesMap)
    {
        var mid = (Bounds.Min + Bounds.Max) * 0.5;

        _children =
        [
            new QuadTreeNode(Bounds with { Max = mid }, Capacity, Depth + 1, MaxDepth),
            new QuadTreeNode
            (
                new Rect
                    (
                        new Vector2D(mid.X, Bounds.Min.Y),
                        new Vector2D(Bounds.Max.X, mid.Y)
                    ),
                Capacity, Depth + 1, MaxDepth
            ),
            new QuadTreeNode
            (
                new Rect
                (
                    new Vector2D(Bounds.Min.X, mid.Y),
                    new Vector2D(mid.X, Bounds.Max.Y)
                ),
                Capacity, Depth + 1, MaxDepth
            ),
            new QuadTreeNode(Bounds with { Min = mid }, Capacity, Depth + 1, MaxDepth)
        ];

        var toReinsert = _circles.ToArray();
        _circles.Clear();

        foreach (var index in toReinsert)
            InsertIntoChildren(index, circlesMap);
    }

    private void InsertIntoChildren(int circleIndex, IReadOnlyList<Circle> circlesMap)
    {
        var circle = circlesMap[circleIndex];
        var hits = 0;

        foreach (var child in _children!)
        {
            if (!child.Bounds.IntersectsCircle(circle))
                continue;

            hits++;
            child.Insert(circleIndex, circlesMap);
        }

        if (hits == 4)
            _circles.Add(circleIndex);
    }

    public int? Query(Vector2D point, IReadOnlyList<Circle> circlesMap)
    {
        if (!Bounds.ContainsPoint(point))
            return null;

        foreach (var index in _circles)
        {
            var circle = circlesMap[index];
            if (circle.Contains(point))
                return index;
        }

        if (_children is null)
            return null;

        foreach (var child in _children)
        {
            var hit = child.Query(point, circlesMap);
            if (hit is not null)
                return hit;
        }

        return null;
    }
}