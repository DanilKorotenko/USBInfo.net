using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    static void Main()
    {
        Console.WriteLine($"Device Serial: {USBHelper.getSerialNumber()}");
        Console.ReadLine();
    }
}
