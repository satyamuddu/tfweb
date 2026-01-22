using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;

namespace CimXml2Json;

public class JsonFileWriter
{


public RatingsData AddRealTimeRating(RatingsData newRating, CancellationToken cancellationToken)
    {
        string jsonContent = System.Text.Json.JsonSerializer.Serialize(newRating, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }); 
        foreach (var coId in newRating.transmissionFacilities)
        {
            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Initialize FilePaths with configuration
            var filePaths = new FilePaths(configuration);
            FilePaths.Instance = filePaths;
            filePaths.FilePathMappings.TryGetValue(coId.id, out string? filePath);

            if (filePath == null)
            {
                throw new Exception($"File path for CO ID '{coId}' not found.");
            }

            System.IO.File.WriteAllText(filePath, jsonContent);
        }
        return newRating;
    }

    private readonly string _filePath;
    public RatingsData AddRealTimeRating(string coId, RatingsData newRating)
    {
        string jsonContent = System.Text.Json.JsonSerializer.Serialize(newRating, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

    // Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Initialize FilePaths with configuration
var filePaths = new FilePaths(configuration);
FilePaths.Instance = filePaths;
    filePaths.FilePathMappings.TryGetValue(coId, out string? filePath);
    
    if (filePath == null)
    {
      throw new Exception($"File path for CO ID '{coId}' not found.");
    }



        System.IO.File.WriteAllText(filePath, jsonContent);
        return newRating;
    }
        
}