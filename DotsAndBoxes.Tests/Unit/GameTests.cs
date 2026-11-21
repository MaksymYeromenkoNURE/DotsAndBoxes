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

    [Fact]
    public void Game_ExposesCorrectEdgesAndTotalCount()
    {
        var game = new Game(2, 3);

        var firstEdge = new Edge(
            0, 0, EdgeOrientation.Horizontal);

        var secondEdge = new Edge(
            1, 1, EdgeOrientation.Vertical);

        Assert.Equal(17, game.TotalEdges);
        Assert.Empty(game.DrawnEdges);

        game.PlayMove(firstEdge);
        game.PlayMove(secondEdge);

        Assert.Equal(17, game.TotalEdges);
        Assert.Equal(2, game.DrawnEdgesCount);
        Assert.Equal(2, game.DrawnEdges.Count);

        Assert.Contains(firstEdge, game.DrawnEdges);
        Assert.Contains(secondEdge, game.DrawnEdges);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(2, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 2)]
    public void GetBoxOwner_InvalidPosition_ThrowsException(int row, int column)
    {
        var game = new Game(2, 2);

        Assert.Throws<ArgumentOutOfRangeException>(() => game.GetBoxOwner(new BoxPosition(row, column)));
    }

    [Fact]
    public void RestoreState_Null_ThrowsArgumentNullException()
    {
        var game = new Game(2, 2);

        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        game.PlayMove(edge);

        var exception = Assert.Throws<ArgumentNullException>(() => game.RestoreState(null!));

        Assert.Equal("state", exception.ParamName);
        Assert.True(game.IsEdgeDrawn(edge));
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }

    [Theory]
    [InlineData(3, 2)]
    [InlineData(2, 3)]
    public void RestoreState_DifferentDimensions_ThrowsAndPreservesGame(
        int rows,
        int columns)
    {
        var game = new Game(2, 2);

        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        game.PlayMove(edge);

        var otherGame = new Game(rows, columns);
        var snapshot = otherGame.SaveState();

        var exception = Assert.Throws<ArgumentException>(() => game.RestoreState(snapshot));

        Assert.Equal("state", exception.ParamName);
        Assert.Equal(1, game.DrawnEdgesCount);
        Assert.True(game.IsEdgeDrawn(edge));
        Assert.Equal(Player.Second, game.CurrentPlayer);
    }
}
