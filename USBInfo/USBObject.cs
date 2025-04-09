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
        foreach (PropertyData prop in _managedObject.Properties)
        {
            var val = prop.Value;
            if (val is not null)
            {
                string? valStr = prop.Value.ToString();
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

    protected ManagementObject ManagedObject { get { return _managedObject; } }

}
