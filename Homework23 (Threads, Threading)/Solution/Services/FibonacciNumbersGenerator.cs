namespace Solution.Services;

internal class FibonacciNumbersGenerator : INumbersGenerator
{
    public event Action<int> NextNumberGenerated;

    private bool _isRunning = false;
    private int _prevNumber;
    private int _nextNumber;
    private readonly int _maxValue;

    private readonly Thread _generationThread;
    private readonly ManualResetEventSlim _pauseEvent = new(true);

    public FibonacciNumbersGenerator(int startNumber = 2, int maxValue = int.MaxValue)
    {
        _maxValue = maxValue;

        if (startNumber <= 0)
        {
            _prevNumber = 0;
            _nextNumber = 1;
        }
        else
        {
            long a = 0;
            long b = 1;
            while (b < startNumber && b <= int.MaxValue)
            {
                long c = a + b;
                a = b;
                b = c;
                if (b > int.MaxValue) break;
            }

            long next = a + b;
            if (b > int.MaxValue) b = int.MaxValue;
            if (next > int.MaxValue) next = int.MaxValue;

            _prevNumber = (int)b;
            _nextNumber = (int)next;
        }

        _generationThread = new Thread(GenerateNext)
        {
            IsBackground = true
        };
    }

    public void Run()
    {
        if (_isRunning) return;
        _isRunning = true;
        _generationThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _pauseEvent.Set();
    }

    public void Pause()
    {
        _pauseEvent.Reset();
    }

    public void Resume()
    {
        _pauseEvent.Set();
    }

    private void GenerateNext()
    {
        if (_prevNumber > _maxValue)
        {
            Stop();
            return;
        }

        if (_prevNumber <= _maxValue)
            NextNumberGenerated?.Invoke(_prevNumber);

        if (_nextNumber <= _maxValue)
            NextNumberGenerated?.Invoke(_nextNumber);

        while (_isRunning)
        {
            _pauseEvent.Wait();

            long next = (long)_prevNumber + _nextNumber;
            if (next > _maxValue || next > int.MaxValue)
            {
                Stop();
                break;
            }

            _prevNumber = _nextNumber;
            _nextNumber = (int)next;

            NextNumberGenerated?.Invoke(_nextNumber);
            Thread.Sleep(500);
        }
    }
}
