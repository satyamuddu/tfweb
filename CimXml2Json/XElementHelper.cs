using System;
using System.Xml.Linq;

namespace CimXml2Json;

public class XElementHelper
{
    public static readonly string ALIASNAME = ".AliasName";
    public static readonly string EQUIPMENT_CONTAINER = "Equipment.EquipmentContainer";
    public static readonly string OPERATIONAL_LIMIT_SET_EQUIPMENT = "OperationalLimitSet.Equipment";

    public static readonly string OPERATIONAL_LIMIT_SET = "OperationalLimit.OperationalLimitSet";
    internal static string GetAliasName(XElement element)
    {
        foreach (var item in element.Elements())
        {
            if (item.Name.LocalName.EndsWith(ALIASNAME))
            {
                return item.Value;
            }
        }
        return string.Empty;
    }

    internal static string GetEnumValue(XElement item, string LocalName)
    {
        foreach (var element in item.Elements())
        {
            if (element.Name.LocalName == LocalName)
            {
                return element.FirstAttribute!.Value.Substring(element.FirstAttribute!.Value.LastIndexOf('.') + 1);
            }
        }
        return string.Empty;
    }

    internal static string GetGeographicalRegion(XElement x)
    {
        string aliasName = GetAliasName(x);
        if (!string.IsNullOrEmpty(aliasName))
        {
            return aliasName.Split('_')[0];
        }
        return string.Empty;
    }

    internal static string GetId(XElement oltype)
    {
        if (oltype.FirstAttribute!.Name.LocalName == "ID")
        {
            return oltype.FirstAttribute.Value;
        }
        return string.Empty;
    }

    internal static string GetParentMRID(XElement item, string localName)
    {
        foreach (var element in item.Elements())
        {
            if (element.Name.LocalName == localName)
            {
                var refAttr = element.Attribute(XName.Get("resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
                if (refAttr != null)
                {
                    return refAttr.Value.Substring(refAttr.Value.IndexOf('#') + 1);
                }
            }
        }
        return string.Empty;
    }

    internal static string GetRefId(XElement ol, string v)
    {
        foreach (var element in ol.Elements())
        {
            if (element.Name.LocalName == v)
            {
                var refAttr = element.Attribute(XName.Get("resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
                if (refAttr != null)
                {
                    return refAttr.Value.Substring(refAttr.Value.IndexOf('#') + 1);
                }
            }
        }
        return string.Empty;
    }

    internal static string GetValue(XElement item, string LocalName)
    {
        foreach (var element in item.Elements())
        {
            if (element.Name.LocalName == LocalName)
            {
                return element.Value;
            }
        }
        return string.Empty;
    }

    internal static bool HasParent(XElement child, string parentMRID, string resourceName)
    {
        foreach (var element in child.Elements())
        {
            if (element.Name.LocalName == resourceName)
            {
                var refAttr = element.Attribute(XName.Get("resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
                if (refAttr != null && refAttr.Value.EndsWith(parentMRID))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
