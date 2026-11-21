using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.WinForms.Rendering;

public sealed class BoardGeometry
{
    public int Rows { get; }
    public int Columns { get; }

    public float CellSize { get; }
    public float OriginX { get; }
    public float OriginY { get; }

    private BoardGeometry(
        int rows,
        int columns,
        float cellSize,
        float originX,
        float originY)
    {
        Rows = rows;
        Columns = columns;
        CellSize = cellSize;
        OriginX = originX;
        OriginY = originY;
    }

    public static BoardGeometry Create(
        int rows,
        int columns,
        Size canvasSize)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentOutOfRangeException(nameof(rows));

        float availableWidth = Math.Max(1f, canvasSize.Width - 80f);
        float availableHeight = Math.Max(1f, canvasSize.Height - 80f);

        float cellSize = Math.Min(availableWidth / columns, availableHeight / rows);

        float originX = (canvasSize.Width - columns * cellSize) / 2f;
        float originY = (canvasSize.Height - rows * cellSize) / 2f;

        return new BoardGeometry(rows, columns, cellSize, originX, originY);
    }

    public PointF GetDot(int row, int column)
    {
        return new PointF(
            OriginX + column * CellSize,
            OriginY + row * CellSize);
    }

    public (PointF Start, PointF End) GetEdgePoints(Edge edge)
    {
        var start = GetDot(edge.Row, edge.Column);

        var end = edge.Orientation == EdgeOrientation.Horizontal
            ? GetDot(edge.Row, edge.Column + 1)
            : GetDot(edge.Row + 1, edge.Column);

        return (start, end);
    }

    public bool TryFindEdge(Point location, out Edge edge)
    {
        float bestDistance = float.MaxValue;
        Edge? bestEdge = null;

        for (int row = 0; row <= Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                Check(new Edge(
                    row, column, EdgeOrientation.Horizontal));
            }
        }

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column <= Columns; column++)
            {
                Check(new Edge(
                    row, column, EdgeOrientation.Vertical));
            }
        }

        float tolerance = Math.Min(18f, CellSize * 0.22f);

        if (bestEdge is Edge found && bestDistance <= tolerance)
        {
            edge = found;
            return true;
        }

        edge = default;
        return false;

        void Check(Edge candidate)
        {
            var (start, end) = GetEdgePoints(candidate);

            float distance = DistanceToSegment(
                location, start, end);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestEdge = candidate;
            }
        }
    }

    private static float DistanceToSegment(
        Point point,
        PointF start,
        PointF end)
    {
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;

        float lengthSquared = dx * dx + dy * dy;

        if (lengthSquared == 0)
            return float.MaxValue;

        float t = (
            (point.X - start.X) * dx +
            (point.Y - start.Y) * dy
        ) / lengthSquared;

        t = Math.Clamp(t, 0f, 1f);

        float nearestX = start.X + t * dx;
        float nearestY = start.Y + t * dy;

        float distanceX = point.X - nearestX;
        float distanceY = point.Y - nearestY;

        return MathF.Sqrt(
            distanceX * distanceX +
            distanceY * distanceY);
    }
}
