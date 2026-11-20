using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Core.Patterns.Observer;

public interface IGameObserver
{
    void OnGameChanged(GameState state);
}
