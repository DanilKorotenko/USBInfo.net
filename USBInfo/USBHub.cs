using System.CodeDom;
using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBHub : USBObject
{
    static public USBHub[] AllDevices
    {
        get 
        {
            List<USBHub> result = new List<USBHub>();
            string query = "SELECT * FROM Win32_USBHub";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject drive in searcher.Get())
            {
                result.Add(new USBHub(drive));
            }
            return result.ToArray();
        }
    }

    static public USBHub[] devicesWithDriveLetters
    {
        get 
        {
            List<USBHub> result = new List<USBHub>();
            USBHub[] all = USBHub.AllDevices;
            foreach (USBHub device in all)
            {
                string[] diskLetters = device.GetDiskNames();
                if(diskLetters.Length > 0)
                {
                    result.Add(device);
                }
            }
            return result.ToArray();
        }
    }
    

    public USBHub(ManagementObject aDrive) : base(aDrive)
    {
    }

    public string PnpDeviceID
    {
        get
        {
            return (string)this.ManagedObject.GetPropertyValue("PNPDeviceID");
        }
    }

    public string SerialNumber
    {
        get
        {
            string result = "No serial number found";

            Object PNPDeviceIDObject = this.ManagedObject["PNPDeviceID"];
            if (PNPDeviceIDObject is not null)
            {
                string? deviceSerial = PNPDeviceIDObject.ToString();
                if (deviceSerial is not null)
                {
                    string[] components = deviceSerial.Split('\\');
                    if (components.Length > 1)
                    {
                        result = components[components.Length-1];
                    }
                }
            }
            return result;
        }
    }

    public string[] GetDiskNames()
    {
        List<string> result = new List<string>();
        using (Device device = Device.Get(PnpDeviceID))
        {
            // get children devices
            foreach (string childDeviceId in device.ChildrenPnpDeviceIds)
            {
                // get the drive object that correspond to this id (escape the id)
                foreach (ManagementObject drive in new ManagementObjectSearcher("SELECT DeviceID FROM Win32_DiskDrive WHERE PNPDeviceID='" + childDeviceId.Replace(@"\", @"\\") + "'").Get())
                {
                    // associate physical disks with partitions
                    foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass=Win32_DiskDriveToDiskPartition").Get())
                    {
                        // associate partitions with logical disks (drive letter volumes)
                        foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass=Win32_LogicalDiskToPartition").Get())
                        {
                            result.Add((string)disk["DeviceID"]);
                        }
                    }
                }
            }
        }
        return result.ToArray();
    }

}
