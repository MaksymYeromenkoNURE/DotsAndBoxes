using DotsAndBoxes.Application.Controllers;
using DotsAndBoxes.Application.Views;
using DotsAndBoxes.Core.Models;
using Moq;
using Xunit;

namespace DotsAndBoxes.Tests.Component;

public class GameControllerTests
{
    [Fact]
    public void Constructor_RendersInitialGameState()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        view.Verify(
            v => v.Render(It.Is<GameState>(
                s => s.Rows == 4 &&
                     s.Columns == 4 &&
                     s.DrawnEdges.Count == 0 &&
                     s.CurrentPlayer == Player.First)),
            Times.Once);
        view.Verify(v => v.SetHistoryAvailability(false, false), Times.Once);
    }

    [Fact]
    public void DrawLine_UpdatesView()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        controller.DrawLine(new Edge(0, 0, EdgeOrientation.Horizontal));

        view.Verify(
            v => v.Render(It.Is<GameState>(
                s => s.DrawnEdges.Count == 1 &&
                     s.CurrentPlayer == Player.Second)),
            Times.Once);
        view.Verify(v => v.SetHistoryAvailability(true, false), Times.Once);
    }

    [Fact]
    public void DrawLine_InvalidMove_ShowsError()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        var edge = new Edge(0, 0, EdgeOrientation.Horizontal);

        controller.DrawLine(edge);
        controller.DrawLine(edge);

        view.Verify(v => v.ShowError(It.IsAny<string>()), Times.Once);
        // Initial state + one successful move.
        view.Verify(v => v.Render(It.IsAny<GameState>()), Times.Exactly(2));
    }

    [Fact]
    public void Undo_RestoresPreviousState()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        controller.DrawLine(new Edge(0, 0, EdgeOrientation.Horizontal));

        Assert.True(controller.Undo());

        view.Verify(
            v => v.Render(It.Is<GameState>(
                s => s.DrawnEdges.Count == 0 &&
                     s.CurrentPlayer == Player.First)),
            Times.Exactly(2));
        view.Verify(v => v.SetHistoryAvailability(false, true), Times.Once);
    }

    [Fact]
    public void Redo_ReappliesMove()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        controller.DrawLine(new Edge(0, 0, EdgeOrientation.Horizontal));
        controller.Undo();

        Assert.True(controller.Redo());

        view.Verify(
            v => v.Render(It.Is<GameState>(
                s => s.DrawnEdges.Count == 1 &&
                     s.CurrentPlayer == Player.Second)),
            Times.Exactly(2));
        view.Verify(v => v.SetHistoryAvailability(true, false), Times.Exactly(2));
    }

    [Fact]
    public void NewGame_ResetsBoardAndHistory()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        controller.DrawLine(new Edge(0, 0, EdgeOrientation.Horizontal));
        controller.NewGame(2, 3);

        view.Verify(
            v => v.Render(It.Is<GameState>(
                s => s.Rows == 2 &&
                     s.Columns == 3 &&
                     s.DrawnEdges.Count == 0 &&
                     s.CurrentPlayer == Player.First)),
            Times.Once);

        Assert.False(controller.Undo());
        Assert.False(controller.Redo());
    }

    [Fact]
    public void UndoRedo_ReturnFalseWhenHistoryIsEmpty()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        Assert.False(controller.Undo());
        Assert.False(controller.Redo());

        view.Verify(v => v.Render(It.IsAny<GameState>()), Times.Once);
        view.Verify(v => v.ShowError(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Constructor_NullView_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new GameController(null!));

        Assert.Equal("view", exception.ParamName);
    }

    [Theory]
    [InlineData(0, 4)]
    [InlineData(4, 0)]
    public void Constructor_InvalidBoardSize_ThrowsException(
        int rows,
        int columns)
    {
        var view = new Mock<IGameView>();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GameController(view.Object, rows, columns));
    }

    [Fact]
    public void DrawLine_InvalidCoordinates_ShowsError()
    {
        var view = new Mock<IGameView>();

        using var controller = new GameController(view.Object);

        var invalidEdge = new Edge(-1, 0, EdgeOrientation.Horizontal);

        controller.DrawLine(invalidEdge);

        view.Verify(v => v.ShowError(It.IsAny<string>()), Times.Once);
        view.Verify(v => v.Render(It.IsAny<GameState>()), Times.Once);
        view.Verify(v => v.SetHistoryAvailability(true, false), Times.Never);
        view.Verify(v => v.SetHistoryAvailability(false, false), Times.Once);
    }
}
