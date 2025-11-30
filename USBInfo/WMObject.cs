using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace USBInfo;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class WMObject : IDisposable
{
    private readonly ManagementObject _managementObject;

    static public IEnumerable<WMObject> searchObjectsWithQuery(string aQuery)
    {
        using (var searcher = new ManagementObjectSearcher(aQuery))
        {
            foreach (ManagementObject drive in searcher.Get())
            {
                yield return new WMObject(drive);
            }
        }
    }

    public WMObject(ManagementObject aManagementObject)
    {
        this._managementObject = aManagementObject;
    }

    protected ManagementObject ManagementObject { get { return _managementObject; } }

    public string? GetStringProperty(string aPropertyName)
    {
        try
        {
            object? propertyVal = ManagementObject.GetPropertyValue(aPropertyName);
            if (propertyVal != null)
            {
                return propertyVal.ToString();
            }
        }
        catch
        {
        }
        return null;
    }

    override public string ToString()
    {
        string result = string.Empty;
        foreach (PropertyData prop in ManagementObject.Properties)
        {
            string? propVal = GetStringProperty(prop.Name);
            if (propVal is not null)
            {
                result += $"{prop.Name}: {propVal} \n";
            }
            else
            {
                result += $"{prop.Name}: null \n";
            }
        }
        return result;
    }

    public void Dispose()
    {
        ((IDisposable)_managementObject).Dispose();
    }

    public string? DeviceID
    {
        get
        {
            return GetStringProperty("DeviceID");
        }
    }

}
