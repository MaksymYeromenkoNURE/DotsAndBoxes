using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Command;
using DotsAndBoxes.Core.Patterns.Observer;
using Moq;

namespace DotsAndBoxes.Tests.Integration;

public class ObserverCommandIntegrationTests
{
    [Fact]
    public void ExecuteUndoRedo_NotifiesCorrectStates()
    {
        var game = new Game(2, 2);
        var history = new CommandHistory(game);

        var observer = new Mock<IGameObserver>();
        var states = new List<GameState>();

        observer
            .Setup(x => x.OnGameChanged(It.IsAny<GameState>()))
            .Callback<GameState>(states.Add);

        game.Subscribe(observer.Object);

        var edge = new Edge(
            0, 0, EdgeOrientation.Horizontal);

        history.ExecuteMove(edge);

        Assert.True(history.Undo());
        Assert.True(history.Redo());

        Assert.Equal(3, states.Count);

        // Execute: the edge exists.
        Assert.Single(states[0].DrawnEdges);
        Assert.Equal(Player.Second, states[0].CurrentPlayer);

        // Undo: the initial state is restored.
        Assert.Empty(states[1].DrawnEdges);
        Assert.Equal(Player.First, states[1].CurrentPlayer);

        // Redo: the edge appears again.
        Assert.Single(states[2].DrawnEdges);
        Assert.Equal(Player.Second, states[2].CurrentPlayer);

        observer.Verify(
            x => x.OnGameChanged(It.IsAny<GameState>()),
            Times.Exactly(3));
    }
}
