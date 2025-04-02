using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBInfo
{

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
                        // foreach (PropertyData prop in device.Properties)
                        // {
                        //     Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                        // }
                        // Console.WriteLine("***************************");
                    }
                    // try
                    // {
                    //     Console.WriteLine($"Device: {device["Name"]}");
                    //     Console.WriteLine($"Serial Number: {device["SerialNumber"]}");
                    // }
                    // catch(Exception ex)
                    // {
                    //     Console.WriteLine($"An error occurred: {ex.Message}");
                    // }
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
