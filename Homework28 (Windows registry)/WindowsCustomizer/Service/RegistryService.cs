using Microsoft.Win32;

namespace WindowsCustomizer.Service;

public static class RegistryService
{
    public static void SetValue(string path, string name, object value)
    {
        if (path.StartsWith("HKEY_"))
            Registry.SetValue(path, name, value);
    }

    public static object? GetValue(string path, string name)
    {
        return Registry.GetValue(path, name, null);
    }
}
