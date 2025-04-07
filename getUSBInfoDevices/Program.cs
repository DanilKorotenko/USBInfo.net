using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    static void Main()
    {
        Console.WriteLine("Devices:");
        USBInfo.USBInfo.printPropertiesDevices();
        Console.ReadLine();
    }
}
