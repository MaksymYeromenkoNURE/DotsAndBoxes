namespace DotsAndBoxes.Core.Models;

public sealed class Board
{
    private readonly HashSet<Edge> _edges = [];

    public int Rows { get; }
    public int Columns { get; }

    public int DrawnEdgesCount => _edges.Count;

    public int TotalEdges =>
        (Rows + 1) * Columns + Rows * (Columns + 1);

    public bool IsFull => DrawnEdgesCount == TotalEdges;

    public IReadOnlyCollection<Edge> DrawnEdges =>
        [.. _edges];

    public Board(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);

        Rows = rows;
        Columns = columns;
    }

    public bool IsEdgeDrawn(Edge edge)
    {
        ValidateEdge(edge);
        return _edges.Contains(edge);
    }

    public IReadOnlyList<BoxPosition> DrawEdge(Edge edge)
    {
        ValidateEdge(edge);

        if (!_edges.Add(edge))
            throw new InvalidOperationException("This edge has already been drawn.");

        var completedBoxes = new List<BoxPosition>();

        if (edge.Orientation == EdgeOrientation.Horizontal)
        {
            CheckBox(edge.Row - 1, edge.Column);
            CheckBox(edge.Row, edge.Column);
        }
        else
        {
            CheckBox(edge.Row, edge.Column - 1);
            CheckBox(edge.Row, edge.Column);
        }

        return completedBoxes;

        void CheckBox(int row, int column)
        {
            if (row >= 0 && row < Rows &&
                column >= 0 && column < Columns &&
                IsBoxCompleted(row, column))
            {
                completedBoxes.Add(new BoxPosition(row, column));
            }
        }
    }

    private bool IsBoxCompleted(int row, int column)
    {
        return
            _edges.Contains(new Edge(row, column, EdgeOrientation.Horizontal)) &&
            _edges.Contains(new Edge(row + 1, column, EdgeOrientation.Horizontal)) &&
            _edges.Contains(new Edge(row, column, EdgeOrientation.Vertical)) &&
            _edges.Contains(new Edge(row, column + 1, EdgeOrientation.Vertical));
    }


    internal void RestoreEdges(IEnumerable<Edge> edges)
    {
        ArgumentNullException.ThrowIfNull(edges);

        var restored = new HashSet<Edge>();

        foreach (var edge in edges)
        {
            ValidateEdge(edge);

            if (!restored.Add(edge))
                throw new ArgumentException("Duplicate edge in saved state.", nameof(edges));
        }

        _edges.Clear();
        _edges.UnionWith(restored);
    }


    private void ValidateEdge(Edge edge)
    {
        bool valid = edge.Orientation switch
        {
            EdgeOrientation.Horizontal =>
                edge.Row >= 0 && edge.Row <= Rows &&
                edge.Column >= 0 && edge.Column < Columns,

            EdgeOrientation.Vertical =>
                edge.Row >= 0 && edge.Row < Rows &&
                edge.Column >= 0 && edge.Column <= Columns,

            _ => false
        };

        if (!valid) throw new ArgumentOutOfRangeException(nameof(edge), "Edge is outside the board.");
    }
}
