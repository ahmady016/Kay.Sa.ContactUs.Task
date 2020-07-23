using System;
using System.Linq;
using System.Reflection;

namespace Domain
{
  public static class Extensions
  {
    // function to return the PropertyInfo of the given propName
    public static PropertyInfo GetProperty(this object obj, string propName)
    {
      return obj.GetType()
                .GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    }
    // function to return Array of All PropertiesInfo of the given Object
    public static PropertyInfo[] GetProperties(this object obj)
    {
      return obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
    // function that use reflection to get the property based on its name at runtime
    public static object GetValue(this object obj, string propName)
    {
      //Get property info
      var prop = obj.GetProperty(propName.Trim());
      if (prop == null)
        return null;
      return prop.GetValue(obj);
    }
    // function that use reflection to set the property value based on its name at runtime
    public static void SetValue(this object obj, string propName, object value)
    {
      var prop = obj.GetProperty(propName);
      if (prop == null)
        return;
      //check if type is nullable, if so return its underlying type , if not, return type
      var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
      TypeCode typeCode = Type.GetTypeCode(propertyType);
      switch (typeCode)
      {
        case TypeCode.Boolean:
          prop.SetValue(obj, Convert.ToBoolean(value), null);
          break;
        case TypeCode.String:
          prop.SetValue(obj, Convert.ToString(value), null);
          break;
        case TypeCode.Byte:
          prop.SetValue(obj, Convert.ToByte(value), null);
          break;
        case TypeCode.SByte:
          prop.SetValue(obj, Convert.ToSByte(value), null);
          break;
        case TypeCode.UInt16:
          prop.SetValue(obj, Convert.ToUInt16(value), null);
          break;
        case TypeCode.UInt32:
          prop.SetValue(obj, Convert.ToUInt32(value), null);
          break;
        case TypeCode.UInt64:
          prop.SetValue(obj, Convert.ToUInt64(value), null);
          break;
        case TypeCode.Int16:
          prop.SetValue(obj, Convert.ToInt16(value), null);
          break;
        case TypeCode.Int32:
          prop.SetValue(obj, Convert.ToInt32(value), null);
          break;
        case TypeCode.Int64:
          prop.SetValue(obj, Convert.ToInt64(value), null);
          break;
        case TypeCode.Single:
          prop.SetValue(obj, Convert.ToSingle(value), null);
          break;
        case TypeCode.Double:
          prop.SetValue(obj, Convert.ToDouble(value), null);
          break;
        case TypeCode.Decimal:
          prop.SetValue(obj, Convert.ToDecimal(value), null);
          break;
        case TypeCode.DateTime:
          prop.SetValue(obj, Convert.ToDateTime(value), null);
          break;
        case TypeCode.Object:
          if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
          {
            prop.SetValue(obj, Guid.Parse(value.ToString()), null);
            return;
          }
          prop.SetValue(obj, value, null);
          break;
        default:
          prop.SetValue(obj, value, null);
          break;
      }
    }
    // function that takes string and split it by the givin separator
    // and removes any empty elements (white spaces or empty strings)
    public static string[] SplitAndRemoveEmpty(this string str, Char separator)
    {
      // convert comma separated list to array so that we can remove empty items
      string[] strArr = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      //Remove empty items from array using where()
      //and trim each element using select(), then return it
      return strArr.Where(item => !string.IsNullOrWhiteSpace(item))
                      .Select(item => item.Trim())
                      .ToArray();
    }
    // function that removes any empty elements (white spaces or empty strings)
    // and join it back the resulted string
    public static string RemoveEmptyElements(this string str, Char separator)
    {
      var strArr = str.SplitAndRemoveEmpty(separator);
      // join strArr back to a string by the given separator
      return string.Join(separator, strArr);
    }
    // get comma separated string of all props
    // [that represent table fields i.e without navigation props]
    public static string GetPrimitivePropsNames(this object obj)
    {
      return string.Join(",",
        obj.GetProperties()
          .Where(prop => Type.GetTypeCode(Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) != TypeCode.Object)
          .Select(prop => $"[{prop.Name}]")
      );
    }

  }
}
