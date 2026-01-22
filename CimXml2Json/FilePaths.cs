using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CimXml2Json
{
    public class FilePaths
    {
        private readonly IConfiguration _configuration;
        
        public string XmlFilePath { get; }
        public string ECARFilePath { get; }
        public string NEPOOLFilePath { get; }

        public Dictionary<string, string> FilePathMappings { get; }

        public FilePaths(IConfiguration configuration)
        {
            _configuration = configuration;
            
            XmlFilePath = _configuration["FilePaths:XmlFilePath"] 
                ?? throw new InvalidOperationException("XmlFilePath not configured");
            ECARFilePath = _configuration["FilePaths:ECARFilePath"] 
                ?? throw new InvalidOperationException("ECARFilePath not configured");
            NEPOOLFilePath = _configuration["FilePaths:NEPOOLFilePath"] 
                ?? throw new InvalidOperationException("NEPOOLFilePath not configured");

            FilePathMappings = new Dictionary<string, string>
            {
                { "XmlFilePath", XmlFilePath },
                { "ECAR", ECARFilePath },
                { "NEPOOL", NEPOOLFilePath }
            };
        }

        // Static method for backward compatibility during migration
        public static FilePaths? Instance { get; set; }
    }
}
