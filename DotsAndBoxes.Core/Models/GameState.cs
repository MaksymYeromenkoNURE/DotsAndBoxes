using System.Collections.ObjectModel;

namespace DotsAndBoxes.Core.Models;

public sealed class GameState
{
    public int Rows { get; }
    public int Columns { get; }

    public IReadOnlyList<Edge> DrawnEdges { get; }

    public IReadOnlyDictionary<BoxPosition, Player> BoxOwners { get; }

    public Player CurrentPlayer { get; }

    public int FirstPlayerScore { get; }
    public int SecondPlayerScore { get; }

    public bool IsFinished { get; }
    public Player? Winner { get; }

    internal GameState(
        int rows,
        int columns,
        IEnumerable<Edge> drawnEdges,
        IReadOnlyDictionary<BoxPosition, Player> boxOwners,
        Player currentPlayer,
        int firstPlayerScore,
        int secondPlayerScore,
        bool isFinished,
        Player? winner)
    {
        Rows = rows;
        Columns = columns;

        DrawnEdges = Array.AsReadOnly(drawnEdges.ToArray());

        BoxOwners = new ReadOnlyDictionary<BoxPosition, Player>(
            boxOwners.ToDictionary(
                item => item.Key,
                item => item.Value));

        CurrentPlayer = currentPlayer;
        FirstPlayerScore = firstPlayerScore;
        SecondPlayerScore = secondPlayerScore;

        IsFinished = isFinished;
        Winner = winner;
    }
}
