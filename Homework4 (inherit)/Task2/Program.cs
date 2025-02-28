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
        var car = new Car
        (
            "Dodge challenger hellcat",
            "a powerfull american V8",
            "HellcatEngine",
            100
        );

        var steamer = new Steamer
        (
            "Liberty",
            "steamer named liberty, thats all i can say",
            "Steamer",
            500
        );

        var oven = new MicrowaveOven
        (
            "My oven",
            "just oven",
            "MicrowaveOven"
        );

        var kettle = new Kettle
        (
            "My kettle",
            "just kettle",
            "Kettle"
        );

        var anotherOneCar = new Car
        (
            "Nissan Skyline GTR",
            "japanese legendary",
            "SkylineEngine",
            50
        );

        Device[] devices = { car, anotherOneCar, steamer, oven, kettle };

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
