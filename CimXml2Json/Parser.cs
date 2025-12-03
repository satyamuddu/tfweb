using System;
using System.Xml.Linq;
using System.Xml;

namespace CimXml2Json;

public class Parser
{

    public Dictionary<string, Dictionary<string, XElement>> cimXElements = new();
    public void LoadXml(string filePath)
    {
        try
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true

            };
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            XmlReader elementReader = XmlReader.Create(fs, settings);
            string localName;
            string id;
            while (elementReader.Read())
            {
                if (elementReader.NodeType == System.Xml.XmlNodeType.Element && elementReader.HasAttributes && elementReader.Depth == 1)
                {
                    localName = elementReader.Name.Substring(elementReader.Name.IndexOf(':') + 1);
                    id = elementReader.GetAttribute(0) ?? string.Empty;
                    var cimElement = XElement.ReadFrom(elementReader) as XElement;
                    if (cimElement != null)
                    {
                        if (cimXElements.TryGetValue(localName, out var dict) == false)
                        {
                            dict = new();
                            cimXElements[localName] = dict;
                        }
                        dict[id] = cimElement;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading XML: {ex.Message}");
        }
    }
}

