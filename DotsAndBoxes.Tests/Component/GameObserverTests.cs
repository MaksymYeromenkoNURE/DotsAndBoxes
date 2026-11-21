using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Observer;
using Moq;

namespace DotsAndBoxes.Tests.Component;

public class GameObserverTests
{
    [Fact]
    public void PlayMove_NotifiesObserver()
    {
        var game = new Game(2, 2);
        var observer = new Mock<IGameObserver>();

        game.Subscribe(observer.Object);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));

        observer.Verify(
            x => x.OnGameChanged(It.Is<GameState>(
                state =>
                    state.DrawnEdges.Count == 1 &&
                    state.CurrentPlayer == Player.Second)),
            Times.Once);
    }

    [Fact]
    public void Unsubscribe_StopsNotifications()
    {
        var game = new Game(2, 2);
        var observer = new Mock<IGameObserver>();

        game.Subscribe(observer.Object);
        game.Unsubscribe(observer.Object);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));

        observer.Verify(x => x.OnGameChanged(It.IsAny<GameState>()), Times.Never);
    }

    [Fact]
    public void InvalidMove_DoesNotNotifyObserver()
    {
        var game = new Game(2, 2);
        var observer = new Mock<IGameObserver>();

        game.Subscribe(observer.Object);

        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        game.PlayMove(edge);

        Assert.Throws<InvalidOperationException>(() => game.PlayMove(edge));

        observer.Verify(x => x.OnGameChanged(It.IsAny<GameState>()), Times.Once);
    }

    [Fact]
    public void DuplicateSubscription_NotifiesOnlyOnce()
    {
        var game = new Game(2, 2);
        var observer = new Mock<IGameObserver>();

        game.Subscribe(observer.Object);
        game.Subscribe(observer.Object);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));

        observer.Verify(x => x.OnGameChanged(It.IsAny<GameState>()), Times.Once);
    }

    [Fact]
    public void GameState_IsIndependentFromFutureMoves()
    {
        var game = new Game(2, 2);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));

        var snapshot = game.GetState();

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));

        Assert.Single(snapshot.DrawnEdges);
        Assert.Equal(Player.Second, snapshot.CurrentPlayer);
        Assert.Equal(2, game.DrawnEdgesCount);
    }

    [Fact]
    public void GameState_ContainsCompletedBoxAndWinner()
    {
        var game = new Game(1, 1);

        game.PlayMove(new Edge(0, 0, EdgeOrientation.Horizontal));
        game.PlayMove(new Edge(0, 0, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(0, 1, EdgeOrientation.Vertical));
        game.PlayMove(new Edge(1, 0, EdgeOrientation.Horizontal));

        var state = game.GetState();

        Assert.Equal(1, state.SecondPlayerScore);
        Assert.Equal(
            Player.Second,
            state.BoxOwners[new BoxPosition(0, 0)]);
        Assert.True(state.IsFinished);
        Assert.Equal(Player.Second, state.Winner);
    }
}
