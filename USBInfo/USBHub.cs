using System.CodeDom;
using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBHub : WMObject
{

    static public IEnumerable<USBHub> DevicesWithDriveLetters()
    {
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub"))
        {
            foreach (ManagementObject mo in searcher.Get())
            {
                using (USBHub device = new USBHub(mo))
                {
                    string[] diskLetters = device.DiskNames;
                    if (diskLetters.Length > 0)
                    {
                        yield return device;
                    }
                }
            }
        }
    }

    static public USBHub? DeviceWithDriveLetter(string aDriveLetter)
    {
        USBHub? result = null;
        foreach (USBHub device in USBHub.DevicesWithDriveLetters())
        {
            string[] diskLetters = device.DiskNames;
            if (diskLetters.Contains(aDriveLetter))
            {
                result = device;
                break;
            }
        }
        return result;
    }

    public USBHub(ManagementObject aDrive) : base(aDrive)
    {
    }

    private string? PnpDeviceID
    {
        get
        {
            return GetStringProperty("PNPDeviceID");
        }
    }

    public string SerialNumber
    {
        get
        {
            string result = "No serial number found";

            string? deviceSerial = this.PnpDeviceID;
            if (deviceSerial is not null)
            {
                string[] components = deviceSerial.Split('\\');
                if (components.Length > 1)
                {
                    result = components[components.Length-1];
                }
            }

            return result;
        }
    }

    private List<string>? diskNames = null;

    public string[] DiskNames
    {
        get
        {
            if (diskNames == null)
            {
                diskNames = new List<string>();
                if (this.PnpDeviceID != null)
                {
                    Device? device = Device.Get(PnpDeviceID);
                    if (device is not null)
                    {
                        // get children devices
                        foreach (string childDeviceId in device.ChildrenPnpDeviceIds)
                        {
                            string childPnpDeviceId = childDeviceId.Replace(@"\", @"\\");
                            string driveQuerry = $"SELECT DeviceID FROM Win32_DiskDrive WHERE PNPDeviceID='{childPnpDeviceId}'";
                            
                            ManagementObjectSearcher driveSearcher = new ManagementObjectSearcher(driveQuerry);
                            // get the drive object that correspond to this id (escape the id)
                            foreach (ManagementObject drive in driveSearcher.Get())
                            {
                                USBDiskDrive usbDrive = new USBDiskDrive(drive);
                                string? driveDeviceID = usbDrive.DeviceID;
                                if (driveDeviceID != null)
                                {
                                    // associate physical disks with partitions
                                    foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + driveDeviceID + "'} WHERE AssocClass=Win32_DiskDriveToDiskPartition").Get())
                                    {
                                        WMObject usbPartition = new WMObject(partition);
                                        string? partitionDeviceID = usbPartition.DeviceID;
                                        if (partitionDeviceID != null)
                                        {
                                            // associate partitions with logical disks (drive letter volumes)
                                            foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partitionDeviceID + "'} WHERE AssocClass=Win32_LogicalDiskToPartition").Get())
                                            {
                                                WMObject usbDisk = new WMObject(disk);
                                                string? diskDeviceID = usbDisk.DeviceID;
                                                if (diskDeviceID != null)
                                                {
                                                    diskNames.Add(diskDeviceID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return diskNames.ToArray();
        }
    }

}
