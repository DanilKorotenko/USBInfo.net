using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBControllerDevice : USBObject
{
    static public USBControllerDevice[] getAllDevices
    {
        get 
        {
            List<USBControllerDevice> result = new List<USBControllerDevice>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBControllerDevice");
            foreach (ManagementObject drive in searcher.Get())
            {
                result.Add(new USBControllerDevice(drive));
            }
            return result.ToArray();
        }
    }

    public USBControllerDevice(ManagementObject aDrive) : base(aDrive)
    {
    }

    public USBPnPEntity? pnpEntity
    {
        get
        {
            USBPnPEntity? result = null;

            // Get the associated PnPDeviceID
            Object? deviceIDObject = this.ManagedObject["Dependent"]; 
            if (deviceIDObject is not null)
            {
                string? deviceId = deviceIDObject.ToString();
                if (deviceId != null && deviceId.Length > 0)
                {
                    deviceId = deviceId.Split('=')[1].Trim('"');

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
