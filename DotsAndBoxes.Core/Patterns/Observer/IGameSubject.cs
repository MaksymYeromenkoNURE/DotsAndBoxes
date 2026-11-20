namespace DotsAndBoxes.Core.Patterns.Observer;

public interface IGameSubject
{
    void Subscribe(IGameObserver observer);

    void Unsubscribe(IGameObserver observer);
}
