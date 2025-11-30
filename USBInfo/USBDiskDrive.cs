using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBDiskDrive : WMObject
{
    static public USBDiskDrive[] getAllDrives
    {
        get
        {
            List<USBDiskDrive> result = new List<USBDiskDrive>();
            //string query = "SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'";
            string query = "SELECT * FROM Win32_DiskDrive";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject drive in searcher.Get())
            {
                result.Add(new USBDiskDrive(drive));
            }
            return result.ToArray();
        }
    }

    public USBDiskDrive(ManagementObject aDrive) : base(aDrive)
    {
    }

    public USBPnPEntity? pnpEntity
    {
        get
        {
            USBPnPEntity? result = null;

            // Get the associated PnPDeviceID
            Object? deviceIDObject = this.ManagementObject["DeviceID"];
            if (deviceIDObject is not null)
            {
                string? deviceId = deviceIDObject.ToString();
                if (deviceId != null && deviceId.Length > 0)
                {
                    USBPnPEntity? entity = USBPnPEntity.GetDeviceWithID(deviceId);
                    if (entity is not null)
                    {
                        result = entity;
                    }
                }
            }
            return result;
        }
    }

}
