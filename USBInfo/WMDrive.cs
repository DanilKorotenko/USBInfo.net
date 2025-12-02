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
                    WMDrive drive = new WMDrive(mo);
                    string[] diskLetters = drive.Letters;
                    if ((diskLetters.Length > 0) && 
                        (String.Compare(diskLetters[0], "C:") > 0))
                    {
                        yield return drive;
                    }
                    else
                    {
                        drive.Dispose();
                    }
                }
            }
        }
    }

    static public IEnumerable<WMDrive> AllUSBDrives
    {
        get
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    WMDrive drive = new WMDrive(mo);
                    string[] diskLetters = drive.Letters;
                    if (diskLetters.Length > 0)
                    {
                        yield return drive;
                    }
                    else
                    {
                        drive.Dispose();
                    }
                }
            }
        }
    }

    static public WMDrive? DriveWithLetter(string aLetter)
    {
        WMDrive? result = null;
        foreach (WMDrive drive in AllDrives)
        {
            string[] diskLetters = drive.Letters;
            if (diskLetters.Contains(aLetter))
            {
                result = drive;
                break;
            }
        }
        return result;
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

    public string? InterfaceType
    {
        get 
        {
            return GetStringProperty("InterfaceType");
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
                    //string escape(string value) => value.Replace("\\", "\\\\").Replace("'", "\\'");
                    try
                    {
                        string partitionQuery = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + driveDeviceID + "'} WHERE AssocClass=Win32_DiskDriveToDiskPartition";
                        // associate physical disks with partitions
                        foreach (WMObject partition in WMObject.searchObjectsWithQuery(partitionQuery))
                        {
                            using (partition)
                            {
                                string? partitionDeviceID = partition.DeviceID;
                                if (partitionDeviceID != null)
                                {
                                    string logicalDisksQuery = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partitionDeviceID + "'} WHERE AssocClass=Win32_LogicalDiskToPartition";
                                    // associate partitions with logical disks (drive letter volumes)
                                    foreach (WMObject disk in WMObject.searchObjectsWithQuery(logicalDisksQuery))
                                    {
                                        using (disk)
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
                    }
                    catch { }
                }
            }
            return letters.ToArray();
        }
    }

}
