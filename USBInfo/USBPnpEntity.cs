using System.CodeDom;
using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBPnPEntity : WMObject
{
    public static USBPnPEntity? GetDeviceWithID(string aDeviceID)
    {
        USBPnPEntity? result = null;
        try
        {
            // Query the PnPDevice for the serial number
            ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_PnPEntity WHERE DeviceID='{aDeviceID}'");
            foreach (ManagementObject device in deviceSearcher.Get())
            {
                Object deviceService = device["Service"];
                if (deviceService == null)
                {
                    continue;
                }
                if (deviceService.ToString() == "USBSTOR")
                {
                    result = new USBPnPEntity(device);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on getting pnpEntity: {ex.Message}");
        }
        return result;
    }

    public static USBPnPEntity[] GetAllEntities()
    {
        List<USBPnPEntity> result = new List<USBPnPEntity>();

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_PnPEntity");
        ManagementObjectCollection deviceCollection = searcher.Get();

        foreach (ManagementObject device in deviceCollection)
        {
            result.Add(new USBPnPEntity(device));
        }
        return result.ToArray();
    }

    public USBPnPEntity(ManagementObject aDrive) : base(aDrive)
    {
    }

}
