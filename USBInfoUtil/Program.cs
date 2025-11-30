using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    public static void printPropertiesUSBDrives()
    {
        Console.WriteLine("USBDrives:");
        USBDiskDrive[] drives = USBDiskDrive.getAllDrives;
        foreach (USBDiskDrive drive in drives)
        {
            Console.WriteLine(drive.ToString());
            Console.WriteLine("***************************");
            USBPnPEntity? entity = drive.pnpEntity;
            if (entity is not null)
            {
                //Console.WriteLine(drive.ToString());
                Console.WriteLine("--PnPEntity--:");
                Console.WriteLine(entity.ToString());
                // Console.WriteLine($"--SerialNumber--: {entity.SerialNumber}");
                // Console.WriteLine("******************************************************");
            }
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
