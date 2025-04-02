using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBInfo
{

    static void printPropertiesVolumes()
    {
        string query = "SELECT * FROM Win32_Volume WHERE DriveType = 2";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

        foreach (ManagementObject drive in searcher.Get())
        {
            foreach (PropertyData prop in drive.Properties)
            {
                Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
            }
            Console.WriteLine("***************************");
        }
    }

    static void printPropertiesDevices()
    {
        // Create a ManagementObjectSearcher to query USB devices
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBControllerDevice");

        foreach (ManagementObject usbDevice in searcher.Get())
        {
            // Get the associated PnPDeviceID
            Object? deviceIDObject = usbDevice["Dependent"]; 
            if (deviceIDObject == null)
            {
                continue;
            }

            string? deviceId = deviceIDObject.ToString();
            if (deviceId == null || deviceId.Length == 0)
            {
                break;
            }
            deviceId = deviceId.Split('=')[1].Trim('"');

            // Query the PnPDevice for the serial number
            ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_PnPEntity WHERE DeviceID='{deviceId}'");
            foreach (ManagementObject device in deviceSearcher.Get())
            {
                Object deviceService = device["Service"];
                if (deviceService == null)
                {
                    continue;
                }
                if (deviceService.ToString() == "USBSTOR")
                {
                    foreach (PropertyData prop in device.Properties)
                    {
                        Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                    }
                    Console.WriteLine("***************************");
                }
            }
        }
    }

    public static string getSerialNumber()
    {
        try
        {
            // Create a ManagementObjectSearcher to query USB devices
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBControllerDevice");

            foreach (ManagementObject usbDevice in searcher.Get())
            {
                // Get the associated PnPDeviceID
                string deviceId = usbDevice["Dependent"].ToString() ?? "";
                if (deviceId.Length == 0)
                {
                    break;
                }
                deviceId = deviceId.Split('=')[1].Trim('"');

                // Query the PnPDevice for the serial number
                ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_PnPEntity WHERE DeviceID='{deviceId}'");
                foreach (ManagementObject device in deviceSearcher.Get())
                {
                    if (device["Service"].ToString() == "USBSTOR")
                    {
                        try
                        {
                            string deviceSerial = device["PNPDeviceID"].ToString() ?? "";
                            if (deviceSerial.Length == 0)
                            {
                                break;
                            }
                            string[] components = deviceSerial.Split('\\');
                            if (components.Length > 1)
                            {
                                deviceSerial = components[components.Length-1];
                            }
                            return deviceSerial;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return "No serial number found";
    }

}
