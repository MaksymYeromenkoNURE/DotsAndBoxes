using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Command;

namespace DotsAndBoxes.Tests.Unit;

public class DrawLineCommandTests
{
    [Fact]
    public void Constructor_NullGame_ThrowsArgumentNullException()
    {
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        var exception = Assert.Throws<ArgumentNullException>(
            () => new DrawLineCommand(null!, edge));

        Assert.Equal("game", exception.ParamName);
    }

    [Fact]
    public void Execute_ValidMove_ReturnsResultAndUpdatesGame()
    {
        var game = new Game(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);
        var command = new DrawLineCommand(game, edge);

        var result = command.Execute();

        Assert.Equal(Player.First, result.PlayedBy);
        Assert.Equal(Player.Second, result.NextPlayer);
        Assert.Empty(result.CompletedBoxes);
        Assert.False(result.IsFinished);
        Assert.Null(result.Winner);
        Assert.True(game.IsEdgeDrawn(edge));
        Assert.Equal(1, game.DrawnEdgesCount);
    }

    [Fact]
    public void Execute_AlreadyExecuted_ThrowsAndKeepsState()
    {
        var game = new Game(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);
        var command = new DrawLineCommand(game, edge);

        command.Execute();

        Assert.Throws<InvalidOperationException>(() => command.Execute());

        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }

    [Fact]
    public void Execute_InvalidEdge_DoesNotAllowUndo()
    {
        var game = new Game(2, 2);
        var command = new DrawLineCommand(
            game, new Edge(-1, 0, EdgeOrientation.Horizontal));

        Assert.Throws<ArgumentOutOfRangeException>(() => command.Execute());
        Assert.Throws<InvalidOperationException>(() => command.Undo());

        Assert.Equal(0, game.DrawnEdgesCount);
        Assert.Equal(Player.First, game.CurrentPlayer);
    }

    [Fact]
    public void Execute_AlreadyDrawnEdge_DoesNotAllowUndo()
    {
        var game = new Game(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        game.PlayMove(edge);

        var command = new DrawLineCommand(game, edge);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Throws<InvalidOperationException>(() => command.Undo());

        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }

    [Fact]
    public void Execute_AfterGameFinished_ThrowsWithoutChangingState()
    {
        var game = new Game(1, 1);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        var command = new DrawLineCommand(
            game, new Edge(0, 0, EdgeOrientation.Horizontal));

        Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.True(game.IsFinished);
        Assert.Equal(4, game.DrawnEdgesCount);
        Assert.Equal(1, game.SecondPlayerScore);
    }

    [Fact]
    public void Undo_BeforeExecution_Throws()
    {
        var game = new Game(2, 2);
        var command = new DrawLineCommand(
            game, new Edge(0, 0, EdgeOrientation.Horizontal));

        Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Equal(0, game.DrawnEdgesCount);
    }

    [Fact]
    public void Undo_Twice_Throws()
    {
        var game = new Game(2, 2);
        var command = new DrawLineCommand(
            game, new Edge(0, 0, EdgeOrientation.Horizontal));

        command.Execute();
        command.Undo();

        Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Equal(0, game.DrawnEdgesCount);
    }

    [Fact]
    public void Execute_AfterUndo_CanRunAgain()
    {
        var game = new Game(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);
        var command = new DrawLineCommand(game, edge);

        command.Execute();
        command.Undo();

        Assert.False(game.IsEdgeDrawn(edge));
        Assert.Equal(Player.First, game.CurrentPlayer);

        var result = command.Execute();

        Assert.Equal(Player.First, result.PlayedBy);
        Assert.Equal(Player.Second, result.NextPlayer);
        Assert.True(game.IsEdgeDrawn(edge));
        Assert.Equal(1, game.DrawnEdgesCount);
    }

    [Fact]
    public void Undo_CompletedBox_RestoresScoreOwnerAndTurn()
    {
        var game = new Game(1, 1);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));

        var edge = new Edge(1, 0, EdgeOrientation.Horizontal);
        var command = new DrawLineCommand(game, edge);

        var result = command.Execute();

        Assert.Single(result.CompletedBoxes);
        Assert.True(result.IsFinished);
        Assert.Equal(Player.Second, result.Winner);
        Assert.Equal(1, game.SecondPlayerScore);

        command.Undo();

        Assert.False(game.IsFinished);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(0, game.SecondPlayerScore);
        Assert.Null(game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.False(game.IsEdgeDrawn(edge));
        Assert.Equal(3, game.DrawnEdgesCount);
    }
}
