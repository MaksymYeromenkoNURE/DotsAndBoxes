using NAudio.Wave;

namespace DotsAndBoxes.WinForms.Services;

public sealed class SoundManager : IDisposable
{
    private sealed class AudioClip : IDisposable
    {
        private readonly AudioFileReader _reader;
        private readonly WaveOut _player;

        public AudioClip(string fileName)
        {
            string path = Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "Audio",
                fileName);

            _reader = new AudioFileReader(path)
            {
                Volume = 0.45f
            };

            _player = new WaveOut();
            _player.Init(_reader);
        }

        public void Play()
        {
            _player.Stop();
            _reader.Position = 0;
            _player.Play();
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void Dispose()
        {
            _player.Stop();
            _player.Dispose();
            _reader.Dispose();
        }
    }

    private readonly AudioClip _line = new("line-draw.mp3");
    private readonly AudioClip _capture = new("box-capture.mp3");
    private readonly AudioClip _finish = new("game-finish.mp3");

    private bool _enabled = true;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;

            if (!_enabled)
            {
                _line.Stop();
                _capture.Stop();
                _finish.Stop();
            }
        }
    }

    public void PlayLine()
    {
        if (Enabled)
            _line.Play();
    }

    public void PlayCapture()
    {
        if (Enabled)
            _capture.Play();
    }

    public void PlayFinish()
    {
        if (!Enabled)
            return;

        _line.Stop();
        _capture.Stop();
        _finish.Play();
    }

    public void StopAll()
    {
        _line.Stop();
        _capture.Stop();
        _finish.Stop();
    }

    public void Dispose()
    {
        _line.Dispose();
        _capture.Dispose();
        _finish.Dispose();
    }
}
