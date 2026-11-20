using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.Core.Patterns.Memento;

public sealed class GameMemento
{
    private readonly Edge[] _edges;
    private readonly Dictionary<BoxPosition, Player> _boxOwners;

    internal int Rows { get; }
    internal int Columns { get; }
    internal Player CurrentPlayer { get; }
    internal int FirstPlayerScore { get; }
    internal int SecondPlayerScore { get; }

    internal IReadOnlyCollection<Edge> Edges => _edges;

    internal IReadOnlyDictionary<BoxPosition, Player> BoxOwners
        => _boxOwners;

    internal GameMemento(
        int rows,
        int columns,
        IEnumerable<Edge> edges,
        IReadOnlyDictionary<BoxPosition, Player> boxOwners,
        Player currentPlayer,
        int firstPlayerScore,
        int secondPlayerScore)
    {
        Rows = rows;
        Columns = columns;
        _edges = [.. edges];
        _boxOwners = boxOwners.ToDictionary(
            item => item.Key,
            item => item.Value);

        CurrentPlayer = currentPlayer;
        FirstPlayerScore = firstPlayerScore;
        SecondPlayerScore = secondPlayerScore;
    }
}
