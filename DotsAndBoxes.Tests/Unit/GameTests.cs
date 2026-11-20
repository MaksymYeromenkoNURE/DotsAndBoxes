using DotsAndBoxes.Core.Models;
using Xunit;

namespace DotsAndBoxes.Tests.Unit;

public class GameTests
{
    [Fact]
    public void Constructor_CreatesGameWithInitialState()
    {
        var game = new Game(2, 2);

        Assert.Equal(Player.First, game.CurrentPlayer);
        Assert.Equal(0, game.FirstPlayerScore);
        Assert.Equal(0, game.SecondPlayerScore);
        Assert.False(game.IsFinished);
        Assert.Null(game.Winner);
    }

    [Fact]
    public void PlayMove_SwitchesPlayerWhenNoBoxCompleted()
    {
        var game = new Game(2, 2);
        var result = game.PlayMove(
    new Edge(0, 0, EdgeOrientation.Horizontal));

        Assert.Equal(Player.First, result.PlayedBy);
        Assert.Equal(Player.Second, result.NextPlayer);
        Assert.Empty(result.CompletedBoxes);
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }

    [Fact]
    public void PlayMove_AwardsPointForCompletedBox()
    {
        var game = new Game(1, 1);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));

        var result = game.PlayMove(
            new Edge(1, 0, EdgeOrientation.Horizontal));

        Assert.Single(result.CompletedBoxes);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(
            Player.Second,
            game.GetBoxOwner(new BoxPosition(0, 0)));
    }

    [Fact]
    public void PlayMove_KeepsTurnAfterCompletingBox()
    {
        var game = new Game(1, 2);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        var result = game.PlayMove(
            new Edge(0, 1, EdgeOrientation.Vertical));

        Assert.Single(result.CompletedBoxes);
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(1, game.SecondPlayerScore);
        Assert.False(game.IsFinished);
    }

    [Fact]
    public void PlayMove_CanCompleteTwoBoxes()
    {
        var game = new Game(1, 2);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));

        game.PlayMove(new Edge(0, 1, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(1, 1, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 2, EdgeOrientation.Vertical));

        var result = game.PlayMove(
            new Edge(0, 1, EdgeOrientation.Vertical));

        Assert.Equal(2, result.CompletedBoxes.Count);
        Assert.Equal(2, game.FirstPlayerScore);
        Assert.Equal(Player.First, game.CurrentPlayer);
        Assert.True(game.IsFinished);
        Assert.Equal(Player.First, game.Winner);
    }

    [Fact]
    public void PlayMove_RejectsDuplicateEdgeWithoutChangingTurn()
    {
        var game = new Game(2, 2);
        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        game.PlayMove(edge);

        Assert.Throws<InvalidOperationException>(
            () => game.PlayMove(edge));
        Assert.Equal(Player.Second, game.CurrentPlayer);
        Assert.Equal(1, game.DrawnEdgesCount);
    }

    [Fact]
    public void PlayMove_RejectsMoveAfterGameFinished()
    {
        var game = new Game(1, 1);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        Assert.True(game.IsFinished);
        Assert.Throws<InvalidOperationException>(
    () => game.PlayMove(
        new Edge(0, 0, EdgeOrientation.Horizontal)));
    }

    [Fact]
    public void GetBoxOwner_ReturnsNullForUnclaimedBox()
    {
        var game = new Game(2, 2);

        Assert.Null(game.GetBoxOwner(new BoxPosition(0, 0)));
    }
}
