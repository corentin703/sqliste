using System.Reflection;
using Sqliste.Core.Constants;

namespace Sqliste.Core.Utils;

public static class SystemParamsUtils
{
    private static string[]? _systemParams;
    
    public static string[] GetAll()
    {
        if (_systemParams != null)
            return _systemParams;

        List<string> systemParams = new();
        FieldInfo[] systemParamsFields = typeof(SystemQueryParametersConstants).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo fieldInfo in systemParamsFields)
        {
            try
            {
                string? systemParam = fieldInfo.GetValue(null) as string;
                if (systemParam == null)
                    continue;

                systemParams.Add(systemParam);
            }
            catch (Exception)
            {
                continue;                
            }
        }

        _systemParams = systemParams.ToArray();
        return _systemParams;
    }
}