using System;

namespace CimXml2Json;

public class JsonFileReader
{
  private RatingsData ReadJsonFile(string coId)
  {

    FilePaths.FilePathMappings.TryGetValue(coId, out string? filePath);
    if (filePath == null)
    {
      throw new Exception($"File path for CO ID '{coId}' not found.");
    }
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
    RatingsData ratingsData = ReadJsonFile(coId);

    return ratingsData;

  }

  public RatingsData GetByCo(string coId, DateTime fromDate, DateTime toDate)
  {
    RatingsData ratingsData = ReadJsonFile(coId);


    var ratingsData1 = new RatingsData
    {
      transmissionFacilities = ratingsData.transmissionFacilities
            .Select(tf => new TransmissionFacilities
            {
              id = tf.id,

              // Only include segments that have ratings in the date range
              segments = tf.segments
                    .Select(seg =>
                    {
                      var filteredRatings = seg.ratings
                      .Where(rat =>
                      {
                        var start = rat.period.start;
                        return start <= toDate && start >= fromDate;
                      })
                      .ToList();

                      // Only add segment if it has ratings
                      return filteredRatings.Any()
                      ? new Segment
                      {
                        id = seg.id,
                        ratings = filteredRatings
                      }
                      : null;
                    })
                    .Where(seg => seg != null)        // remove nulls
                    .Cast<Segment>()                  // cast from Segment? to Segment
                    .ToList()
            })
            // Only add TF if it has segments
            .Where(tf => tf.segments.Any())
            .ToList(),
      comment = ratingsData.comment,
      id = ratingsData.id
    };




    /*
        RatingsData ratingsData1 = new RatingsData
        {
          transmissionFacilities = ratingsData.transmissionFacilities.Select(tf => new TransmissionFacilities
          {
            id = tf.id,
            segments = tf.segments.Select(seg => new Segment
            {
              id = seg.id,
              ratings = seg.ratings.Where(rat =>
              {
                DateTime ratingStart = rat.period.start;
                return ratingStart <= toDate && ratingStart >= fromDate;
              }).ToList()
            }).ToList()
          }).ToList()
        };*/
    return ratingsData1;
  }
}
