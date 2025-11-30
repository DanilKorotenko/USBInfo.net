using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class WMDrive : WMObject
{
    static public IEnumerable<WMDrive> AllDrives
    {
        get
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    using (WMDrive drive = new WMDrive(mo))
                    {
                        string[] diskLetters = drive.Letters;
                        if (diskLetters.Length > 0)
                        {
                            yield return drive;
                        }
                    }
                }
            }
        }
    }

    public WMDrive(ManagementObject aDrive) : base(aDrive)
    {
    }

    public string? SerialNumber
    {
        get
        {
            return GetStringProperty("SerialNumber");
        }
    }

    private List<string>? letters = null;

    public string[] Letters
    {
        get
        {
            if (letters == null)
            {
                letters = new List<string>();

                string? driveDeviceID = this.DeviceID;
                if (driveDeviceID != null)
                {
                    string partitionQuery = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + driveDeviceID + "'} WHERE AssocClass=Win32_DiskDriveToDiskPartition";
                    // associate physical disks with partitions
                    foreach (WMObject partition in WMObject.searchObjectsWithQuery(partitionQuery))
                    {
                        string? partitionDeviceID = partition.DeviceID;
                        if (partitionDeviceID != null)
                        {
                            string logicalDisksQuery = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partitionDeviceID + "'} WHERE AssocClass=Win32_LogicalDiskToPartition";
                            // associate partitions with logical disks (drive letter volumes)
                            foreach (WMObject disk in WMObject.searchObjectsWithQuery(logicalDisksQuery))
                            {
                                string? diskDeviceID = disk.DeviceID;
                                if (diskDeviceID != null)
                                {
                                    letters.Add(diskDeviceID);
                                }
                            }
                        }
                    }
                }
            }
            return letters.ToArray();
        }
    }

}
