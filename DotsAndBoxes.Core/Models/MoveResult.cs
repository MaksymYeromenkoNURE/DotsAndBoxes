namespace DotsAndBoxes.Core.Models;

public sealed record MoveResult(
    Player PlayedBy,
    IReadOnlyList<BoxPosition> CompletedBoxes,
    Player NextPlayer,
    bool IsFinished,
    Player? Winner
);
