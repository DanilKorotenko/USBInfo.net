using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{

    static void Main()
    {
        string usbSerial = USBInfo.USBInfo.getSerialNumber();
        Console.WriteLine(usbSerial);
    }
}
