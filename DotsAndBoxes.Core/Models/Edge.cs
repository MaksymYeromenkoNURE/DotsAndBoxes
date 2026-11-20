namespace DotsAndBoxes.Core.Models;

public readonly record struct Edge(
    int Row,
    int Column,
    EdgeOrientation Orientation
);
