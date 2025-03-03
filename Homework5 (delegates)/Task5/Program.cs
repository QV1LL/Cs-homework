namespace Task5;

internal static class Program
{
    public static void Main(string[] args)
    {
        Action helloToUser = () => Console.WriteLine("Hello, user!");
        helloToUser += HelloUserInFile;

        helloToUser();
    }

    public static void HelloUserInFile()
    {
        using (StreamWriter writer = new StreamWriter("hello_user.txt"))
        {
            writer.WriteLine("Hello, user!");
        }
    }
}
