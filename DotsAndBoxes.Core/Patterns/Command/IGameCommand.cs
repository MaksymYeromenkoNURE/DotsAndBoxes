using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Core.Patterns.Command;

public interface IGameCommand
{
    MoveResult Execute();

    void Undo();
}
