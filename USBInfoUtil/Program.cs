using USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

class Program
{
    public static void printPropertiesVolumes()
    {
        Console.WriteLine("Volumes:");
        USBVolume[] volumes = USBVolume.getAllVolumes;
        foreach (USBVolume volume in volumes)
        {
            // volume.PrintAllProperties();
            Console.WriteLine("***************************");
            USBPnPEntity? entity = volume.pnpEntity;
            if (entity is not null)
            {
                volume.PrintAllProperties();
                Console.WriteLine("--PnPEntity--:");
                entity.PrintAllProperties();
                // Console.WriteLine($"--SerialNumber--: {entity.SerialNumber}");
                // Console.WriteLine("******************************************************");
            }
        }
    }

    public static void printPropertiesUSBDrives()
    {
        Console.WriteLine("USBDrives:");
        USBDiskDrive[] drives = USBDiskDrive.getAllDrives;
        foreach (USBDiskDrive drive in drives)
        {
            drive.PrintAllProperties();
            Console.WriteLine("***************************");
            USBPnPEntity? entity = drive.pnpEntity;
            if (entity is not null)
            {
                drive.PrintAllProperties();
                Console.WriteLine("--PnPEntity--:");
                entity.PrintAllProperties();
                // Console.WriteLine($"--SerialNumber--: {entity.SerialNumber}");
                // Console.WriteLine("******************************************************");
            }
        }
    }


    public static void printPropertiesDevices()
    {
        Console.WriteLine("Devices:");
        USBControllerDevice[] devices = USBControllerDevice.getAllDevices;
        foreach (USBControllerDevice usbDevice in devices)
        {
            USBPnPEntity? entity = usbDevice.pnpEntity;
            if (entity is not null)
            {
                usbDevice.PrintAllProperties();
                Console.WriteLine("--PnPEntity--:");
                entity.PrintAllProperties();
                Console.WriteLine($"--SerialNumber--: {entity.SerialNumber}");
                Console.WriteLine("******************************************************");
            }
        }
    }

    public static void printPnPEntities()
    {
        Console.WriteLine("PnPEntities:");
        USBPnPEntity[] devices = USBPnPEntity.GetAllEntities();
        foreach (USBPnPEntity usbDevice in devices)
        {
            usbDevice.PrintAllProperties();
            // Console.WriteLine($"Caption: {usbDevice.Caption}");
            // Console.WriteLine($"Description: {usbDevice.Description}");
            // Console.WriteLine($"Number of hardware IDs: {usbDevice.HardwareID.Length}");

            // foreach (string hardwareID in usbDevice.HardwareID)
            // {
            //     Console.WriteLine(hardwareID);
            // }

            Console.WriteLine("******************************************************");
        }
    }

    public static void printUSBHub()
    {
        Console.WriteLine("USBHub drives:");
        USBHub[] devices = USBHub.devicesWithDriveLetters;
        foreach (USBHub usbDevice in devices)
        {
            usbDevice.PrintAllProperties();
            Console.WriteLine("Drives: ");
            foreach (string driveLetter in usbDevice.DiskNames)
            {
                Console.WriteLine(driveLetter);
            }
            Console.WriteLine("******************************************************");
        }
    }

    public static void printDrivesInfo()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            Console.WriteLine($"Drive Name: {d.Name}");
            Console.WriteLine($"  Drive Type: {d.DriveType}");
            if (d.IsReady == true)
            {
                Console.WriteLine($"  Volume Label: {d.VolumeLabel}");
                Console.WriteLine($"  File System: {d.DriveFormat}");
                Console.WriteLine($"  Available Space: {d.AvailableFreeSpace / (1024 * 1024 * 1024)} GB");
                Console.WriteLine($"  Total Size: {d.TotalSize / (1024 * 1024 * 1024)} GB");
            }
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        //Console.WriteLine("****DEVICES**************************************************");
        //printPropertiesDevices();

        //Console.WriteLine("****VOLUMES**************************************************");
        //printPropertiesVolumes();

        Console.WriteLine("****USBDrives**************************************************");
        printPropertiesUSBDrives();

        //Console.WriteLine("****PnpEntities**************************************************");
        //printPnPEntities();

        Console.WriteLine("****USBHub**************************************************");
        printUSBHub();

        //printDrivesInfo();

        Console.ReadLine();
    }
}
