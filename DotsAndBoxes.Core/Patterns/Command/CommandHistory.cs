using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Core.Patterns.Command;

public sealed class CommandHistory(Game game)
{
    private readonly Game _game = game ?? throw new ArgumentNullException(nameof(game));

    private readonly Stack<IGameCommand> _undoStack = new();
    private readonly Stack<IGameCommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public MoveResult ExecuteMove(Edge edge)
    {
        var command = new DrawLineCommand(_game, edge);
        var result = command.Execute();

        _undoStack.Push(command);
        _redoStack.Clear();

        return result;
    }

    public bool Undo()
    {
        if (!CanUndo)
            return false;

        var command = _undoStack.Peek();
        command.Undo();

        _undoStack.Pop();
        _redoStack.Push(command);

        return true;
    }

    public bool Redo()
    {
        if (!CanRedo)
            return false;

        var command = _redoStack.Peek();
        command.Execute();

        _redoStack.Pop();
        _undoStack.Push(command);

        return true;
    }
}
