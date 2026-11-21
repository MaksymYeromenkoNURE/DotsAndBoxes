using System.Diagnostics;
using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.WinForms.Animations;

public sealed class HoverAnimation
{
    private const double DurationMs = 900;

    private readonly Stopwatch _watch = new();

    public Edge? HoveredEdge { get; private set; }

    public bool IsActive => HoveredEdge.HasValue;

    public float Progress =>
        !IsActive
            ? 0f : (float)(_watch.Elapsed.TotalMilliseconds % DurationMs / DurationMs);

    public void SetEdge(Edge? edge)
    {
        if (HoveredEdge == edge)
            return;

        HoveredEdge = edge;

        if (edge.HasValue)
            _watch.Restart();
        else
            _watch.Reset();
    }

    public void Stop()
    {
        HoveredEdge = null;
        _watch.Reset();
    }
}
