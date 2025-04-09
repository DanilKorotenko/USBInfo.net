using System.CodeDom;
using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBPnPEntity : USBObject
{
    public static USBPnPEntity? GetDeviceWithID(string aDeviceID)
    {
        USBPnPEntity? result = null;
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

    public string SerialNumber
    {
        get
        {
            string result = "No serial number found";

            Object PNPDeviceIDObject = this.ManagedObject["PNPDeviceID"];
            if (PNPDeviceIDObject is not null)
            {
                string? deviceSerial = PNPDeviceIDObject.ToString();
                if (deviceSerial is not null)
                {
                    string[] components = deviceSerial.Split('\\');
                    if (components.Length > 1)
                    {
                        result = components[components.Length-1];
                    }
                }
            }
            return result;
        }
    }

    public void printDeviceProperties()
    {
        
    }

    public enum AvailabilityEnum
    {
        /// <summary>
        /// Represents the meaning "Other".
        /// </summary>
        Other = 0x01,

        /// <summary>
        /// Represents the meaning "Unknown".
        /// </summary>
        Unknown = 0x02,

        /// <summary>
        /// Represents the meaning "Running or Full Power".
        /// </summary>
        RunningOrFullPower = 0x03,

        /// <summary>
        // Represents the meaning "Warning".
        /// </summary>
        Warning = 0x04,

        /// <summary>
        /// Represents the meaning "In Test".
        /// </summary>
        InTest = 0x05,

        /// <summary>
        /// Represents the meaning "Not Applicable".
        /// </summary>
        NotApplicable = 0x06,

        /// <summary>
        /// Represents the maning "Power Off".
        /// </summary>
        PowerOff = 0x07,

        /// <summary>
        /// Represents the meaning "Off Line".
        /// </summary>
        OffLine = 0x08,

        /// <summary>
        /// Represents the meaning "Off Duty".
        /// </summary>
        OffDuty = 0x09,

        /// <summary>
        /// Represents the meaning "Degraded".
        /// </summary>
        Degraded = 0x0A,

        /// <summary>
        /// Represents the meaning "Not Installed".
        /// </summary>
        NotInstalled = 0x0B,

        /// <summary>
        /// Represents the meaning "Install Error".
        /// </summary>
        InstallError = 0x0C,

        /// <summary>
        /// Represents the meaning "Power Save - Unknown".
        /// The device is known to be in a power save mode, but its exact status is unknown.
        /// </summary>
        PowerSave_Unknown = 0x0D,

        /// <summary>
        /// Represents the meaning "Power Save - Low Power Mode".
        /// The device is in a power save state but still functioning, and may exhibit degraded performance.
        /// </summary>
        PowerSave_LowPowerMode = 0x0E,

        /// <summary>
        /// Represents the meaning "Power Save - Standby".
        /// The device is not functioning, but could be brought to full power quickly.
        /// </summary>
        PowerSave_Standyby = 0x0F,

        /// <summary>
        /// Represents the meaning "Power Cycle".
        /// </summary>
        PowerCycle = 0x10,

        /// <summary>
        /// Represents the meaning "Power Save - Warning".
        /// The device is in a warning state, though also in a power save mode.
        /// </summary>
        PowerSave_Warning = 0x11
    }

    public enum ConfigManagerErrorCodeEnum
    {
        /// <summary>
        /// Represents the meaning "Unknown", not supported in the original WMI class.
        /// </summary>
        Unknown = 0xFF,

        /// <summary>
        /// Represents the meaning "Device is working properly.".
        /// </summary>
        WorkingProperly = 0x00,

        /// <summary>
        /// Represents the meaning "Device is not configured correctly.".
        /// </summary>
        DeviceNotConfiguredError = 0x01,

        /// <summary>
        /// Represents the meaning "Windows cannot load the driver for this device.".
        /// </summary>
        CannotLoadDriverError = 0x02,

        /// <summary>
        /// Represents the meaning "Driver for this device might be corrupted, or the system may be low on memory or other resources.".
        /// </summary>
        DriverCorruptedError = 0x03,

        /// <summary>
        /// Represents the meaning "Device is not working properly. One of its drivers or the registry might be corrupted.".
        /// </summary>
        NotWorkingProperlyError = 0x04,

        /// <summary>
        /// Represents the meaning "Driver for the device requires a resource that Windows cannot manage.".
        /// </summary>
        DriverResourceError = 0x05,

        /// <summary>
        /// Represents the meaning "Boot configuration for the device conflicts with other devices.".
        /// </summary>
        BootConfigurationError = 0x06,

        /// <summary>
        /// Represents the meaning "Cannot filter.".
        /// </summary>
        CannotFilterError = 0x07,

        /// <summary>
        /// Represents the meaning "Driver loader for the device is missing.".
        /// </summary>
        DriverLoaderMissingError = 0x08,

        /// <summary>
        /// Represents the meaning "Device is not working properly. The controlling firmware is incorrectly reporting the resources for the device.".
        /// </summary>
        DeviceNotWorkingProperlyFirmwareError = 0x09,

        /// <summary>
        /// Represents the meaning "Device cannot start.".
        /// </summary>
        DeviceCannotStartError = 0x0A,

        /// <summary>
        /// Represents the meaning "Device failed.".
        /// </summary>
        DeviceFailedError = 0x0B,

        /// <summary>
        /// Represents the meaning "Device cannot find enough free resources to use.".
        /// </summary>
        DeviceTooFewResourcesError = 0x0C,

        /// <summary>
        /// Represents the meaning "Windows cannot verify the device's resources.".
        /// </summary>
        VerifyDeviceResourceError = 0x0D,

        /// <summary>
        /// Represents the meaning "Device cannot work properly until the computer is restarted.".
        /// </summary>
        DeviceCannotWorkProperlyUnitlRestartError = 0x0E,

        /// <summary>
        /// Represents the meaning "Device is not working properly due to a possible re-enumeration problem.".
        /// </summary>
        DeviceNotWorkingProperlyReenumerationError = 0x0F,

        /// <summary>
        /// Represents the meaning "Windows cannot identify all of the resources that the device uses.".
        /// </summary>
        IdentifyResourcesForDeviceError = 0x10,

        /// <summary>
        /// Represents the meaning "Device is requesting an unknown resource type.".
        /// </summary>
        UnknownResourceTypeError = 0x11,

        /// <summary>
        /// Represents the meaning "Device drivers must be reinstalled.".
        /// </summary>
        DeviceDriversMustBeResinstalledError = 0x12,

        /// <summary>
        /// Represents the meaning "Failure using the VxD loader.".
        /// </summary>
        FailureUsingVxDLoaderError = 0x13,

        /// <summary>
        /// Represents the meaning "Registry might be corrupted.".
        /// </summary>
        RegistryMightBeCorruptedError = 0x14,

        /// <summary>
        /// Represents the meaning "System failure. If changing the device driver is ineffective, see the hardware documentation. Windows is removing the device.".
        /// </summary>
        SystemFailureRemovingDeviceError = 0x15,

        /// <summary>
        /// Represents the meaning "Device is disabled.".
        /// </summary>
        DeviceDisabledError = 0x16,

        /// <summary>
        /// Represents the meaning "System failure. If changing the device driver is ineffective, see the hardware documentation.".
        /// </summary>
        SystemFailureError = 0x17,

        /// <summary>
        /// Represents the meaning "Device is not present, not working properly, or does not have all of its drivers installed.".
        /// </summary>
        DeviceNotPresentError = 0x18,

        /// <summary>
        /// Represents the meaning "Windows is still setting up the device.".
        /// </summary>
        StillSettingUpTheDeviceError = 0x19,

        /// <summary>
        /// Represents the meaning "Windows is still setting up the device.".
        /// </summary>
        StillSettingUpTheDeviceError_2 = 0x1A,

        /// <summary>
        /// Represents the meaning "Device does not have valid log configuration.".
        /// </summary>
        InvalidDeviceLogConfigurationError = 0x1B,

        /// <summary>
        /// Represents the meaning "Device drivers are not installed.".
        /// </summary>
        DeviceDriversNotInstalledError = 0x1C,

        /// <summary>
        /// Represents the meaning "Device is disabled. The device firmware did not provide the required resources.".
        /// </summary>
        DeviceDisabledDueToFirmwareResourceError = 0x1D,

        /// <summary>
        /// Represents the meaning "Device is using an IRQ resource that another device is using.".
        /// </summary>
        IRQConflictError = 0x1E,

        /// <summary>
        /// Represents the meaning "Device is not working properly. Windows cannot load the required device drivers.".
        /// </summary>
        DeviceNotWorkingProperlyCannotLoadDrivers = 0x1F
    }

    public enum StatusInfoEnum
    {
        /// <summary>
        /// Represents the meaning "Other".
        /// </summary>
        Other = 0x01,

        /// <summary>
        /// Represents the meaning "Unknown".
        /// </summary>
        Unknown = 0x02,

        /// <summary>
        /// Represents the meaning "Enabled".
        /// </summary>
        Enabled = 0x03,

        /// <summary>
        /// Represents the meaning "Disabled".
        /// </summary>
        Disabled = 0x04,

        /// <summary>
        /// Represents the meaning "Not Applicable".
        /// </summary>
        NotApplicable = 0x05
    }

    public AvailabilityEnum Availability
    {
        get
        {
            try
            {
                Int16 value = (Int16)this.ManagedObject.GetPropertyValue("Availability");

                if (!Enum.IsDefined(typeof(AvailabilityEnum), value))
                {
                    // Handle not supported values here
                    throw new NotSupportedException($"The value {value} is not supported for conversion to the {nameof(AvailabilityEnum)} enumeration.");
                }

                return (AvailabilityEnum)value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return AvailabilityEnum.Unknown;
            }
        }
    }

    public string Caption
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Caption");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string ClassGuid
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("ClassGuid");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string[] CompatibleID
    {
        get
        {
            try
            {
                string[] value = (string[])this.ManagedObject.GetPropertyValue("ClassGuid");

                if (value == null)
                    // Handle null value.
                    return new string[0];

                return value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return new string[0];
            }
        }
    }

    public ConfigManagerErrorCodeEnum ConfigManagerErrorCode
    {
        get
        {
            try
            {
                Int16 value = (Int16)this.ManagedObject.GetPropertyValue("ConfigManagerErrorCode");

                if (!Enum.IsDefined(typeof(ConfigManagerErrorCodeEnum), value))
                {
                    // Handle not supported values here
                    throw new NotSupportedException($"The value {value} is not supported for conversion to the {nameof(ConfigManagerErrorCodeEnum)} enumeration.");
                }

                return (ConfigManagerErrorCodeEnum)value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return ConfigManagerErrorCodeEnum.Unknown;
            }
        }
    }

    public bool ConfigManagerUserConfig
    {
        get
        {
            try
            {
                return (bool)this.ManagedObject.GetPropertyValue("ConfigManagerUserConfig");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return false;
            }
        }
    }

    public string CreationClassName
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("CreationClassName");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string Description
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Description");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string DeviceID
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("DeviceID");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public bool ErrorCleared
    {
        get
        {
            try
            {
                return (bool)this.ManagedObject.GetPropertyValue("ErrorCleared");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return false;
            }
        }
    }

    public string ErrorDescription
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("ErrorDescription");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string[] HardwareID
    {
        get
        {
            try
            {
                string[] value = (string[])this.ManagedObject.GetPropertyValue("HardwareID");

                if (value == null)
                    // Handle null value.
                    return new string[0];

                return value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return new string[0];
            }
        }
    }

    public DateTime InstallDate
    {
        get
        {
            try
            {
                DateTime value = (DateTime)this.ManagedObject.GetPropertyValue("InstallDate");
                return value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return DateTime.MinValue;
            }
        }
    }

    public UInt32 LastErrorCode
    {
        get
        {
            try
            {
                return (UInt32)this.ManagedObject.GetPropertyValue("LastErrorCode");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return 0;
            }
        }
    }

    public string Manufacturer
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Manufacturer");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string Name
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Name");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string PNPClass
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("PNPClass");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string PNPDeviceID
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("PNPDeviceID");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public UInt16[] PowerManagementCapabilities
    {
        get
        {
            try
            {
                UInt16[] value = (UInt16[])this.ManagedObject.GetPropertyValue("PowerManagementCapabilities");

                if (value == null)
                    // Handle null value.
                    return new UInt16[0];

                return value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return new UInt16[0];
            }
        }
    }

    public bool PowerManagementSupported
    {
        get
        {
            try
            {
                return (bool)this.ManagedObject.GetPropertyValue("PowerManagementSupported");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return false;
            }
        }
    }

    public bool Present
    {
        get
        {
            try
            {
                return (bool)this.ManagedObject.GetPropertyValue("Present");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return false;
            }
        }
    }

    public string Service
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Service");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string Status
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("Status");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public StatusInfoEnum StatusInfo
    {
        get
        {
            try
            {
                Int16 value = (Int16)this.ManagedObject.GetPropertyValue("StatusInfo");

                if (!Enum.IsDefined(typeof(StatusInfoEnum), value))
                {
                    // Handle not supported values here
                    throw new NotSupportedException($"The value {value} is not supported for conversion to the {nameof(StatusInfoEnum)} enumeration.");
                }

                return (StatusInfoEnum)value;
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return StatusInfoEnum.NotApplicable;
            }
        }
    }

    public string SystemCreationClassName
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("SystemCreationClassName");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }

    public string SystemName
    {
        get
        {
            try
            {
                return (string)this.ManagedObject.GetPropertyValue("SystemName");
            }
            catch
            {
                // Handle exception caused by accessing the property value.
                return "Unknown";
            }
        }
    }




}
