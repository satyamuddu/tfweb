using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CimXml2Json
{
    public class FilePaths
    {
        public static readonly string XmlFilePath = "/Users/muddusatyanarayana/Documents/tfweb/TransmissionFacilityWebApp/data/esca60_gev_original.xml";
        public static readonly string ECARFilePath = "/Users/muddusatyanarayana/Documents/tfweb/CimXml2JsonTest/ECAR_ratings.json";
        public static readonly string NEPOOLFilePath = "/Users/muddusatyanarayana/Documents/tfweb/CimXml2JsonTest/NEPOOL_ratings.json";

        public static readonly Dictionary<string, string> FilePathMappings = new Dictionary<string, string>
        {
            { "XmlFilePath", XmlFilePath },
            { "ECAR", ECARFilePath },
            { "NEPOOL", NEPOOLFilePath }
        };
    }
}