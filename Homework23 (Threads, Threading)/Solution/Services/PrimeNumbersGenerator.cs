namespace Solution.Services;

internal class PrimeNumbersGenerator : INumbersGenerator
{
    public event Action<int> NextNumberGenerated;

    private bool _isRunning = false;
    private int _nextNumber;
    private readonly int _maxValue;

    private readonly Thread _generationThread;
    private readonly ManualResetEventSlim _pauseEvent = new (true);

    public PrimeNumbersGenerator(int startNumber = 2, int maxValue = int.MaxValue)
    {
        _nextNumber = startNumber;
        _maxValue = maxValue;
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
        while (_isRunning)
        {
            _pauseEvent.Wait();

            _nextNumber++;

            if (_nextNumber > _maxValue)
            {
                Stop();
                break;
            }

            if (IsPrime(_nextNumber))
            {
                NextNumberGenerated?.Invoke(_nextNumber);
                Thread.Sleep(500);
            }
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        if (number == 2) return true;

        int limit = (int)Math.Sqrt(number);
        for (int i = 2; i <= limit; i++)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }
}