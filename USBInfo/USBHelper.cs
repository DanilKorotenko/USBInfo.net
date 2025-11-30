using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBInfo;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBHelper 
{
    public static WMDrive? findUSBHub() 
    {
        WMDrive? result = null;
        foreach (WMDrive device in WMDrive.AllDrives)
        {
            result = device;
            break;
        }
        return result;
    }

    public static string getSerialNumber() 
    {
        WMDrive? usbHub = findUSBHub();
        if (usbHub is not null) 
        {
            string? result = usbHub.SerialNumber;
            if (string.IsNullOrEmpty(result))
            {
                result = "No serial number found";
            }
            return result;
        }
        Console.WriteLine("No devices found");
        return string.Empty;
    }
}
