
using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Command;
using Xunit;

namespace DotsAndBoxes.Tests.Integration;

public class CommandMementoTests
{
    [Fact]
    public void SaveAndRestore_RevertsGameState()
    {
        var game = new Game(2, 2);
        var firstEdge = new Edge(0, 0, EdgeOrientation.Horizontal);
        var secondEdge = new Edge(0, 0, EdgeOrientation.Vertical);

        game.PlayMove(firstEdge);

        var snapshot = game.SaveState();

        game.PlayMove(secondEdge);
        game.RestoreState(snapshot);

        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.True(game.IsEdgeDrawn(firstEdge));
        Assert.False(game.IsEdgeDrawn(secondEdge));
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }

    [Fact]
    public void ExecuteMove_RecordsCommand()
    {
        var game = new Game(2, 2);
        var history = new CommandHistory(game);

        history.ExecuteMove(
            new Edge(0, 0, EdgeOrientation.Horizontal));

        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.True(history.CanUndo);
        Assert.False(history.CanRedo);
    }

    [Fact]
    public void Undo_RestoresScoreTurnAndBoxOwner()
    {
        var game = new Game(1, 1);
        var history = new CommandHistory(game);

        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(0, 1, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        Assert.True(game.IsFinished);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.True(history.Undo());
        Assert.False(game.IsFinished);
        Assert.Equal(0, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Null(game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.True(history.CanRedo);
    }

    [Fact]
    public void Redo_ReappliesCompletedMove()
    {
        var game = new Game(1, 1);
        var history = new CommandHistory(game);

        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(0, 1, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        history.Undo();

        Assert.True(history.Redo());
        Assert.True(game.IsFinished);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.Winner);
        Assert.False(history.CanRedo);
    }

    [Fact]
    public void NewMoveAfterUndo_ClearsRedoHistory()
    {
        var game = new Game(2, 2);
        var history = new CommandHistory(game);

        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Vertical));

        history.Undo();

        history.ExecuteMove(new Edge(
            1, 0, EdgeOrientation.Horizontal));

        Assert.False(history.CanRedo);
        Assert.False(history.Redo());
        Assert.Equal(2, game.DrawnEdgesCount);
    }

    [Fact]
    public void InvalidMove_DoesNotEnterHistory()
    {
        var game = new Game(2, 2);
        var history = new CommandHistory(game);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        history.ExecuteMove(edge);

        Assert.Throws<InvalidOperationException>(() => history.ExecuteMove(edge));
        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.True(history.Undo());
        Assert.False(history.CanUndo);
        Assert.False(history.Undo());
        Assert.Equal(0, game.DrawnEdgesCount);
    }

    [Fact]
    public void SaveAndRestore_AfterClaimedBox_PreservesSnapshot()
    {
        var game = new Game(1, 2);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        Assert.Equal(Player.Second, game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.Equal(1, game.SecondPlayerScore);

        // The snapshot now contains an already claimed square.
        var snapshot = game.SaveState();

        game.PlayMove(new Edge(0, 1, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 2, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 1, EdgeOrientation.Horizontal));

        Assert.True(game.IsFinished);
        Assert.Equal(2, game.SecondPlayerScore);

        game.RestoreState(snapshot);

        Assert.False(game.IsFinished);
        Assert.Equal(4, game.DrawnEdgesCount);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(0, game.FirstPlayerScore);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.Null(game.GetBoxOwner(new BoxPosition(0, 1)));
        Assert.False(game.IsEdgeDrawn(new Edge(0, 1, EdgeOrientation.Horizontal)));
    }

    [Fact]
    public void UndoRedo_AfterClaimedBox_PreservesExistingOwner()
    {
        var game = new Game(1, 2);
        var history = new CommandHistory(game);

        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        history.ExecuteMove(new Edge(0, 0, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(0, 1, EdgeOrientation.Vertical));
        history.ExecuteMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        var nextEdge = new Edge(0, 1, EdgeOrientation.Horizontal);
        history.ExecuteMove(nextEdge);

        Assert.True(history.Undo());

        Assert.Equal(4, game.DrawnEdgesCount);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.False(game.IsEdgeDrawn(nextEdge));

        Assert.True(history.Redo());

        Assert.Equal(5, game.DrawnEdgesCount);
        Assert.Equal(Player.First, game.CurrentPlayer);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.GetBoxOwner(new BoxPosition(0, 0)));
        Assert.True(game.IsEdgeDrawn(nextEdge));
    }

}
