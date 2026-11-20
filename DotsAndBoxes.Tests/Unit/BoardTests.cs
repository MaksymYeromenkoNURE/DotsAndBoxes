using DotsAndBoxes.Core.Models;
using Xunit;

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
        var edge = new Edge(
            0, 0, EdgeOrientation.Horizontal);

        board.DrawEdge(edge);

        Assert.True(board.IsEdgeDrawn(edge));
        Assert.Equal(1, board.DrawnEdgesCount);
    }

    [Fact]
    public void DrawEdge_ThrowsWhenEdgeAlreadyExists()
    {
        var board = new Board(2, 2);
        var edge = new Edge(
            0, 0, EdgeOrientation.Horizontal);

        board.DrawEdge(edge);

        Assert.Throws<InvalidOperationException>(
            () => board.DrawEdge(edge));
    }

    [Fact]
    public void DrawEdge_ThrowsForInvalidPosition()
    {
        var board = new Board(2, 2);
        var edge = new Edge(
            -1, 0, EdgeOrientation.Horizontal);

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
}
