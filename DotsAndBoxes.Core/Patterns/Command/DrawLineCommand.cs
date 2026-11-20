using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Core.Patterns.Memento;

namespace DotsAndBoxes.Core.Patterns.Command;

public sealed class DrawLineCommand(Game game, Edge edge) : IGameCommand
{
    private readonly Game _game = game ?? throw new ArgumentNullException(nameof(game));
    private readonly Edge _edge = edge;

    private GameMemento? _previousState;
    private bool _isExecuted;

    public MoveResult Execute()
    {
        if (_isExecuted)
            throw new InvalidOperationException("Command has already been executed.");

        var previousState = _game.SaveState();
        var result = _game.PlayMove(_edge);

        _previousState = previousState;
        _isExecuted = true;

        return result;
    }

    public void Undo()
    {
        if (!_isExecuted || _previousState is null)
            throw new InvalidOperationException("Command has not been executed.");

        _game.RestoreState(_previousState);

        _previousState = null;
        _isExecuted = false;
    }
}
