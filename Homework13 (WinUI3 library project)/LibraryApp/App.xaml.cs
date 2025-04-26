using LibraryApp.Domain;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using WinUI3Localizer;

namespace LibraryApp;

public partial class App : Application
{
    internal static List<Book> Books = new();

    private Window? m_window;
    public static Window? MainWindow { get; private set; }

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
        MainWindow = m_window;
    }
}
