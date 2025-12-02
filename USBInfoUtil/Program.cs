using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    public static void printWMDrives()
    {
        foreach (WMDrive drive in WMDrive.AllDrives)
        {
            Console.WriteLine($"Drive Letters: {String.Join(", ", drive.Letters)}");
            Console.WriteLine($"Drive Serial: {drive.SerialNumber}");
            Console.WriteLine($"Drive InterfaceType: {drive.InterfaceType}");
            Console.WriteLine("******************************************************");
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("****WMDrives**************************************************");
        printWMDrives();
    }
}
