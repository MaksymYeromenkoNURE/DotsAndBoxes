using System.Diagnostics;
using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.WinForms.Animations;

public sealed class LineAnimation
{
    private const double DurationMs = 250;

    private readonly Stopwatch _watch = new();

    public Edge? AnimatedEdge { get; private set; }

    public Player PlayedBy { get; private set; }

    public float Progress
    {
        get
        {
            if (AnimatedEdge is null)
                return 1f;

            return Math.Clamp(
                (float)(_watch.Elapsed.TotalMilliseconds / DurationMs),
                0f,
                1f);
        }
    }

    public bool IsActive => AnimatedEdge is not null && Progress < 1f;

    public void Start(Edge edge, Player player)
    {
        AnimatedEdge = edge;
        PlayedBy = player;
        _watch.Restart();
    }

    public void Stop()
    {
        _watch.Reset();
        AnimatedEdge = null;
    }
}
