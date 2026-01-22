using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;

namespace CimXml2Json;

public class JsonFileWriter
{


    public static string GetCompanyFromId(string id)
    {
        string company = id.Trim();
        string co = company.Substring(0, company.IndexOf(' '));
        return co;
    }
    public RatingsData AddRealTimeRating(RatingsData newRating, CancellationToken cancellationToken)
    {
        if (newRating == null || newRating.transmissionFacilities == null || newRating.transmissionFacilities.Count == 0)
        {
            throw new ArgumentNullException(nameof(newRating), "RatingsData cannot be null or have empty transmission facilities.");
        }
        // Check for cancellation
        cancellationToken.ThrowIfCancellationRequested();

        string company = newRating.transmissionFacilities[0].id.Trim();
        string co = GetCompanyFromId(company);
        company = co;
        //1. DeSerialize RatingsData to JSON
        JsonFileReader jsonFileReader = new JsonFileReader();
        var existingRatingsData = jsonFileReader.GetByCo(co);
        if (existingRatingsData != null)
        {
            // Merge logic: Here we simply replace existing transmissionFacilities with new ones
            //existingRatingsData.transmissionFacilities = newRating.transmissionFacilities;
            //newRating = existingRatingsData;

            foreach (var tf in newRating.transmissionFacilities)
            {
                existingRatingsData.transmissionFacilities.Add(tf);
            }

        }
        else
        {
            existingRatingsData = newRating;
        }
        newRating = existingRatingsData;

        string jsonContent = System.Text.Json.JsonSerializer.Serialize(newRating, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        var configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .Build();

        // Initialize FilePaths with configuration
        var filePaths = new FilePaths(configuration);
        FilePaths.Instance = filePaths;
        filePaths.FilePathMappings.TryGetValue(company, out string? filePath);

        if (filePath == null)
        {
            throw new Exception($"File path for CO ID '{company}' not found.");
        }

        System.IO.File.WriteAllText(filePath, jsonContent);
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