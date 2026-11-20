using DotsAndBoxes.Core.Patterns.Memento;
using DotsAndBoxes.Core.Patterns.Observer;

namespace DotsAndBoxes.Core.Models;

public sealed class Game(int rows = 4, int columns = 4) : IGameSubject
{
    private readonly Board _board = new(rows, columns);
    private readonly Dictionary<BoxPosition, Player> _boxOwners = [];
    private readonly HashSet<IGameObserver> _observers = [];

    public Player CurrentPlayer { get; private set; } = Player.First;

    public int Rows => _board.Rows;
    public int Columns => _board.Columns;

    public int DrawnEdgesCount => _board.DrawnEdgesCount;
    public int TotalEdges => _board.TotalEdges;

    public IReadOnlyCollection<Edge> DrawnEdges => _board.DrawnEdges;

    public int FirstPlayerScore { get; private set; }
    public int SecondPlayerScore { get; private set; }

    public bool IsFinished => _board.IsFull;

    public Player? Winner
    {
        get
        {
            if (!IsFinished ||
                FirstPlayerScore == SecondPlayerScore)
                return null;

            return FirstPlayerScore > SecondPlayerScore
                ? Player.First
                : Player.Second;
        }
    }

    public void Subscribe(IGameObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Add(observer);
    }

    public void Unsubscribe(IGameObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Remove(observer);
    }

    public GameState GetState()
    {
        return new GameState(
            Rows,
            Columns,
            _board.DrawnEdges,
            _boxOwners,
            CurrentPlayer,
            FirstPlayerScore,
            SecondPlayerScore,
            IsFinished,
            Winner
        );
    }

    private void NotifyObservers()
    {
        var state = GetState();

        foreach (var observer in _observers.ToArray())
        {
            observer.OnGameChanged(state);
        }
    }

    public bool IsEdgeDrawn(Edge edge)
    {
        return _board.IsEdgeDrawn(edge);
    }

    public Player? GetBoxOwner(BoxPosition position)
    {
        if (position.Row < 0 || position.Row >= Rows ||
            position.Column < 0 || position.Column >= Columns)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        return _boxOwners.TryGetValue(position, out var owner)
            ? owner
            : null;
    }

    public MoveResult PlayMove(Edge edge)
    {
        if (IsFinished)
            throw new InvalidOperationException("The game has already finished.");

        var player = CurrentPlayer;
        var completedBoxes = _board.DrawEdge(edge);

        foreach (var box in completedBoxes)
        {
            _boxOwners.Add(box, player);
        }

        if (completedBoxes.Count > 0)
        {
            if (player == Player.First)
                FirstPlayerScore += completedBoxes.Count;
            else
                SecondPlayerScore += completedBoxes.Count;
        }
        else
        {
            CurrentPlayer = player == Player.First
                ? Player.Second
                : Player.First;
        }

        var result = new MoveResult(
            player,
            [.. completedBoxes],
            CurrentPlayer,
            IsFinished,
            Winner
        );

        NotifyObservers();

        return result;
    }

    public GameMemento SaveState()
    {
        return new GameMemento(
            Rows,
            Columns,
            _board.DrawnEdges,
            _boxOwners,
            CurrentPlayer,
            FirstPlayerScore,
            SecondPlayerScore
        );
    }

    public void RestoreState(GameMemento state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (state.Rows != Rows || state.Columns != Columns)
            throw new ArgumentException("Saved state has different board dimensions.", nameof(state));

        _board.RestoreEdges(state.Edges);

        _boxOwners.Clear();

        foreach (var item in state.BoxOwners)
        {
            _boxOwners.Add(item.Key, item.Value);
        }

        CurrentPlayer = state.CurrentPlayer;
        FirstPlayerScore = state.FirstPlayerScore;
        SecondPlayerScore = state.SecondPlayerScore;

        NotifyObservers();
    }
}
