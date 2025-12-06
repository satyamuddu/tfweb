using System;

namespace CimXml2Json;

public class JsonFileReader
{
  public RatingsData ReadJsonFile(string filePath)
  {
    // Implementation for reading JSON file
    var jsonData = System.IO.File.ReadAllText(filePath);

    if (string.IsNullOrEmpty(jsonData))
    {
      throw new Exception("JSON file is empty or not found.");
    }
    else
    {
      Console.WriteLine("JSON file read successfully.");

      RatingsData? ratingsData = System.Text.Json.JsonSerializer.Deserialize<RatingsData>(jsonData);
      return ratingsData ?? throw new Exception("Failed to deserialize JSON data.");
    }
  }
  public RatingsData GetByCo(string coId)
  {
    // Implementation for getting RatingsData by CO ID
    RatingsData ratingsData = ReadJsonFile(coId + "_ratings.json");

    return ratingsData;

  }
}
