using NAudio.Wave;
using Microsoft.Extensions.Configuration;
using Task2.Entities;

namespace Task2;

internal static class Program
{
    private static IConfiguration Config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json")
            .Build();

    public static void Main(string[] args)
    {
        Device[] devices = 
        {
            new Car
            (
                "Dodge challenger hellcat",
                "a powerfull american V8",
                "HellcatEngine",
                100
            ),
            new Car
            (
                "Nissan Skyline GTR",
                "japanese legendary",
                "SkylineEngine",
                50
            ),
            new Steamer
            (
                "Liberty",
                "steamer named liberty, thats all i can say",
                "Steamer",
                500
            ),
            new MicrowaveOven
            (
                "My oven",
                "just oven",
                "MicrowaveOven"
            ),
            new Kettle
            (
                "My kettle",
                "just kettle",
                "Kettle"
            )

        };

        foreach (Device device in devices)
            TestDevice(device);
    }

    private static void TestDevice(Device device)
    {
        Console.WriteLine();
        Console.WriteLine(device.Title);
        Console.WriteLine(device.Description);

        string relativePath = Config["Sounds:" + device.SoundKey];

        using (var audioFile = new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)))
        using (var waveOut = new WaveOutEvent())
        {
            waveOut.Init(audioFile);
            waveOut.Play();

            while (waveOut.PlaybackState == PlaybackState.Playing)
                device.PrintSubtitlesToSound();
        }
    }
}
