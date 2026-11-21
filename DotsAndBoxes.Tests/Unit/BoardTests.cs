using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Tests.Unit;

public class BoardTests
{
    [Fact]
    public void Constructor_CreatesBoardWithCorrectSize()
    {
        var board = new Board(2, 2);

        Assert.Equal(2, board.Rows);
        Assert.Equal(2, board.Columns);
        Assert.Equal(12, board.TotalEdges);
        Assert.Equal(0, board.DrawnEdgesCount);
    }

    [Fact]
    public void DrawEdge_AddsEdgeToBoard()
    {
        var board = new Board(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        board.DrawEdge(edge);

        Assert.True(board.IsEdgeDrawn(edge));
        Assert.Equal(1, board.DrawnEdgesCount);
    }

    [Fact]
    public void DrawEdge_ThrowsWhenEdgeAlreadyExists()
    {
        var board = new Board(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        board.DrawEdge(edge);

        Assert.Throws<InvalidOperationException>(
            () => board.DrawEdge(edge));
    }

    [Fact]
    public void DrawEdge_ThrowsForInvalidPosition()
    {
        var board = new Board(2, 2);
        var edge = new Edge(-1, 0, EdgeOrientation.Horizontal);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => board.DrawEdge(edge));
    }

    [Fact]
    public void DrawEdge_DetectsCompletedBox()
    {
        var board = new Board(2, 2);

        board.DrawEdge(new Edge(0, 0, EdgeOrientation.Horizontal));
        board.DrawEdge(new Edge(0, 0, EdgeOrientation.Vertical));
        board.DrawEdge(new Edge(0, 1, EdgeOrientation.Vertical));
        var completed = board.DrawEdge(new Edge(1, 0, EdgeOrientation.Horizontal));
        Assert.Equal(new BoxPosition(0, 0), Assert.Single(completed));
    }

    [Fact]
    public void DrawEdge_CanCompleteTwoBoxes()
    {
        var board = new Board(2, 2);

        board.DrawEdge(new Edge(0, 0, EdgeOrientation.Horizontal));
        board.DrawEdge(new Edge(1, 0, EdgeOrientation.Horizontal));
        board.DrawEdge(new Edge(0, 0, EdgeOrientation.Vertical));

        board.DrawEdge(new Edge(0, 1, EdgeOrientation.Horizontal));
        board.DrawEdge(new Edge(1, 1, EdgeOrientation.Horizontal));
        board.DrawEdge(new Edge(0, 2, EdgeOrientation.Vertical));

        var completed = board.DrawEdge(
            new Edge(0, 1, EdgeOrientation.Vertical));

        Assert.Equal(2, completed.Count);
        Assert.Contains(new BoxPosition(0, 0), completed);
        Assert.Contains(new BoxPosition(0, 1), completed);
    }

    [Fact]
    public void RestoreEdges_Null_ThrowsArgumentNullException()
    {
        var board = new Board(2, 2);

        var exception = Assert.Throws<ArgumentNullException>(() => board.RestoreEdges(null!));

        Assert.Equal("edges", exception.ParamName);
    }

    [Fact]
    public void RestoreEdges_EmptyCollection_ClearsBoard()
    {
        var board = new Board(2, 2);

        board.DrawEdge(new Edge(0, 0, EdgeOrientation.Horizontal));

        board.RestoreEdges([]);

        Assert.Equal(0, board.DrawnEdgesCount);
        Assert.Empty(board.DrawnEdges);
    }

    [Fact]
    public void RestoreEdges_ValidCollection_ReplacesEdges()
    {
        var board = new Board(2, 2);

        var oldEdge = new Edge(0, 0, EdgeOrientation.Horizontal);

        var newEdge = new Edge(1, 0, EdgeOrientation.Vertical);

        board.DrawEdge(oldEdge);
        board.RestoreEdges([newEdge]);

        Assert.False(board.IsEdgeDrawn(oldEdge));
        Assert.True(board.IsEdgeDrawn(newEdge));
        Assert.Equal(1, board.DrawnEdgesCount);
    }

    [Fact]
    public void RestoreEdges_Duplicate_ThrowsWithoutChangingBoard()
    {
        var board = new Board(2, 2);

        var existingEdge = new Edge(0, 0, EdgeOrientation.Horizontal);

        var newEdge = new Edge(1, 0, EdgeOrientation.Vertical);

        board.DrawEdge(existingEdge);

        var exception = Assert.Throws<ArgumentException>(
            () => board.RestoreEdges([newEdge, newEdge]));

        Assert.Equal("edges", exception.ParamName);
        Assert.True(board.IsEdgeDrawn(existingEdge));
        Assert.False(board.IsEdgeDrawn(newEdge));
        Assert.Equal(1, board.DrawnEdgesCount);
    }

    [Fact]
    public void RestoreEdges_InvalidEdge_ThrowsWithoutChangingBoard()
    {
        var board = new Board(2, 2);

        var existingEdge = new Edge(0, 0, EdgeOrientation.Horizontal);

        var newEdge = new Edge(1, 0, EdgeOrientation.Vertical);

        var invalidEdge = new Edge(3, 0, EdgeOrientation.Horizontal);

        board.DrawEdge(existingEdge);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => board.RestoreEdges([newEdge, invalidEdge]));

        Assert.True(board.IsEdgeDrawn(existingEdge));
        Assert.False(board.IsEdgeDrawn(newEdge));
        Assert.Equal(1, board.DrawnEdgesCount);
    }

    [Theory]
    [InlineData(-1, 0, EdgeOrientation.Horizontal)]
    [InlineData(3, 0, EdgeOrientation.Horizontal)]
    [InlineData(0, -1, EdgeOrientation.Horizontal)]
    [InlineData(0, 2, EdgeOrientation.Horizontal)]
    [InlineData(-1, 0, EdgeOrientation.Vertical)]
    [InlineData(2, 0, EdgeOrientation.Vertical)]
    [InlineData(0, -1, EdgeOrientation.Vertical)]
    [InlineData(0, 3, EdgeOrientation.Vertical)]
    [InlineData(0, 0, (EdgeOrientation)99)]
    public void IsEdgeDrawn_InvalidEdge_ThrowsArgumentOutOfRangeException(
        int row,
        int column,
        EdgeOrientation orientation)
    {
        var board = new Board(2, 2);

        var edge = new Edge(row, column, orientation);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => board.IsEdgeDrawn(edge));
    }

    [Fact]
    public void IsEdgeDrawn_UpperBoundaryEdges_AreValid()
    {
        var board = new Board(2, 2);

        var horizontal = new Edge(2, 1, EdgeOrientation.Horizontal);

        var vertical = new Edge(1, 2, EdgeOrientation.Vertical);

        board.DrawEdge(horizontal);
        board.DrawEdge(vertical);

        Assert.True(board.IsEdgeDrawn(horizontal));
        Assert.True(board.IsEdgeDrawn(vertical));
    }
}
