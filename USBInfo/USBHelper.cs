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
    public static USBHub? findUSBHub() 
    {
        USBHub? result = null;
        USBHub[] devices = USBHub.devicesWithDriveLetters;
        if (devices.Length > 0) 
        {
            result = devices[0];
        }
        return result;
    }

    public static string getSerialNumber() 
    {
        USBHub? usbHub = findUSBHub();
        if (usbHub is not null) 
        {
            return usbHub.SerialNumber;
        }
        Console.WriteLine("No devices found");
        return string.Empty;
    }
}
