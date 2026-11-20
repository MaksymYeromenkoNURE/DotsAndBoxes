using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Application.Views;

public interface IGameView
{
    void Render(GameState state);

    void SetHistoryAvailability(bool canUndo, bool canRedo);

    void ShowError(string message);
}
