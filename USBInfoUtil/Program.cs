using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    public static void printPropertiesUSBDrives()
    {
        Console.WriteLine("Win32_DiskDrives:");
        //string query = "SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'";
        string query = "SELECT * FROM Win32_DiskDrive";

        foreach (WMObject drive in WMObject.searchObjectsWithQuery(query))
        {
            Console.WriteLine(drive.ToString());
        }
    }

    public static void printUSBHub()
    {
        Console.WriteLine("USBHub drives:");

        foreach (USBHub usbDevice in USBHub.DevicesWithDriveLetters())
        {
            Console.WriteLine(usbDevice.ToString());
            Console.WriteLine("Drives: ");
            foreach (string driveLetter in usbDevice.DiskNames)
            {
                Console.WriteLine(driveLetter);
            }
            Console.WriteLine("******************************************************");
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("****USBDrives**************************************************");
        printPropertiesUSBDrives();

        Console.WriteLine("****USBHub**************************************************");
        printUSBHub();

        //Console.ReadLine();
    }
}
