using TaskFourAndFive.Services;

namespace TaskFourAndFive;

internal static class Program
{
    // Не знаю, як по мені тут взагалі не варіант використовувати mutex, бо потрібно зберегти порядок потоків, а при mutex.Release другий та третій потоки почнуть битися за нього

    private const string PATH_TO_WRITE_FILES_INFO = "files_info.txt";

    public static void Main()
    {
        var service = new NumbersService();
        var filesInfoService = new FilesInfoService();

        var t1 = new Thread(() =>
        {
            service.GenerateAndWriteNumbers().Wait();
        });

        var t2 = new Thread(() =>
        {
            service.WritePrimeNumbers().Wait();
        });

        var t3 = new Thread(() =>
        {
            service.WriteNumbersEndsWith7().Wait();
        });

        var t4 = new Thread(() =>
        {
            filesInfoService.AddFile(NumbersService.PATH_TO_WRITE_RANDOM_NUMBERS);
            filesInfoService.AddFile(NumbersService.PATH_TO_WRITE_PRIME_NUMBERS);
            filesInfoService.AddFile(NumbersService.PATH_TO_WRITE_ENDS_WITH_7_NUMBERS);

            filesInfoService.SaveFilesInfo(PATH_TO_WRITE_FILES_INFO);
        });

        t1.Start();
        t1.Join();

        t2.Start();
        t2.Join();

        t3.Start();
        t3.Join();

        t4.Start();
        t4.Join();
    }
}
