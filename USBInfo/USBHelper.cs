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
        foreach (USBHub device in USBHub.DevicesWithDriveLetters())
        {
            result = device;
            break;
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
