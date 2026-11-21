using System.Diagnostics;
using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.WinForms.Animations;

public sealed class CaptureAnimation
{
    private const double DurationMs = 650;

    private readonly Stopwatch _watch = new();

    private readonly List<(BoxPosition Position, Player Owner)> _boxes = [];

    public IReadOnlyList<(BoxPosition Position, Player Owner)> Boxes => _boxes;

    public bool IsPending { get; private set; }

    public bool HasStarted => _watch.IsRunning;

    public bool IsActive => HasStarted && Progress < 1f;

    public bool IsBusy => IsPending || IsActive;

    public float Progress =>
        HasStarted
            ? Math.Clamp((float)(_watch.Elapsed.TotalMilliseconds / DurationMs), 0f, 1f)
            : 0f;

    public bool Contains(BoxPosition position)
    {
        return IsBusy && _boxes.Exists(box => box.Position == position);
    }

    public void Prepare(
        IEnumerable<KeyValuePair<BoxPosition, Player>> boxes)
    {
        Stop();

        foreach (var box in boxes)
            _boxes.Add((box.Key, box.Value));

        IsPending = _boxes.Count > 0;
    }

    public void Start()
    {
        if (!IsPending)
            return;

        IsPending = false;
        _watch.Restart();
    }

    public void Stop()
    {
        _watch.Reset();
        _boxes.Clear();
        IsPending = false;
    }
}
