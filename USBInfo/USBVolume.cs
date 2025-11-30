using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBVolume : WMObject
{
    static public USBVolume[] getAllVolumes
    {
        get 
        {
            List<USBVolume> result = new List<USBVolume>();
            string query = "SELECT * FROM Win32_Volume WHERE DriveType = 2";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject drive in searcher.Get())
            {
                result.Add(new USBVolume(drive));
            }
            return result.ToArray();
        }
    }

    public USBVolume(ManagementObject aDrive) : base(aDrive)
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
