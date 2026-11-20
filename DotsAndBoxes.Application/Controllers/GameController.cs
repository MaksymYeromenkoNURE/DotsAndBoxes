using DotsAndBoxes.Application.Views;
using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Command;
using DotsAndBoxes.Core.Patterns.Observer;

namespace DotsAndBoxes.Application.Controllers;

public sealed class GameController : IGameObserver, IDisposable
{
    private readonly IGameView _view;

    private Game _game;
    private CommandHistory _history;

    public GameController(IGameView view, int rows = 4, int columns = 4)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));

        _game = new Game(rows, columns);
        _history = new CommandHistory(_game);

        _game.Subscribe(this);

        RefreshView();
    }

    public void DrawLine(Edge edge)
    {
        try
        {
            _history.ExecuteMove(edge);
        }
        catch (Exception ex) when (
            ex is InvalidOperationException ||
            ex is ArgumentException)
        {
            _view.ShowError(ex.Message);
            return;
        }

        UpdateHistoryButtons();
    }

    public bool Undo()
    {
        if (!_history.Undo())
            return false;

        UpdateHistoryButtons();
        return true;
    }

    public bool Redo()
    {
        if (!_history.Redo())
            return false;

        UpdateHistoryButtons();
        return true;
    }

    public void NewGame(int rows = 4, int columns = 4)
    {
        var newGame = new Game(rows, columns);

        _game.Unsubscribe(this);

        _game = newGame;
        _history = new CommandHistory(_game);

        _game.Subscribe(this);

        RefreshView();
    }

    public void OnGameChanged(GameState state) => _view.Render(state);

    private void RefreshView()
    {
        _view.Render(_game.GetState());
        UpdateHistoryButtons();
    }

    private void UpdateHistoryButtons()
    {
        _view.SetHistoryAvailability(
            _history.CanUndo,
            _history.CanRedo);
    }

    public void Dispose() => _game.Unsubscribe(this);
}
