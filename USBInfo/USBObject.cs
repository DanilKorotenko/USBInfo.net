using System.Management;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class USBObject
{
    private ManagementObject _managedObject;

    public USBObject(ManagementObject aManagedObject)
    {
        this._managedObject = aManagedObject;
    }

    public void PrintAllProperties()
    {
        foreach (PropertyData prop in ManagedObject.Properties)
        {
            object? val = prop.Value;
            if (val is not null)
            {
                string? valStr = val.ToString();
                if (valStr is not null)
                {
                    Console.WriteLine("{0}: {1}", prop.Name, valStr);
                }
                else
                {
                    Console.WriteLine("{0}: null", prop.Name);
                }
            }
            else
            {
                Console.WriteLine("{0}: null", prop.Name);
            }
        }
    }

    public string? GetStringProperty(string aPropertyName)
    {
        object? propertyVal = ManagedObject[aPropertyName];
        if (propertyVal != null)
        {
            return propertyVal.ToString();
        }
        return null;
    }

    public string? DeviceID
    {
        get
        {
            return GetStringProperty("DeviceID");
        }
    }

    protected ManagementObject ManagedObject { get { return _managedObject; } }


}
