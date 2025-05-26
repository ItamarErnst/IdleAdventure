namespace IdleAdventure;

using System;
using System.Threading;

public class TimeInterpolator
{
    private readonly int _durationMs;
    private readonly int _stepMs;
    private readonly Func<bool> _isPaused;
    private readonly Action _onComplete;

    public TimeInterpolator(int durationMs, int stepMs, Func<bool> isPaused, Action onComplete)
    {
        _durationMs = durationMs;
        _stepMs = stepMs;
        _isPaused = isPaused;
        _onComplete = onComplete;
    }

    public void Run()
    {
        int elapsed = 0;

        while (elapsed < _durationMs)
        {
            if (!_isPaused())
            {
                Thread.Sleep(_stepMs);
                elapsed += _stepMs;
            }
            else
            {
                Thread.Sleep(_stepMs);
            }
        }

        _onComplete();
    }
}
