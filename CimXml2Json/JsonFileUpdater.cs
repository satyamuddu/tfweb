using Microsoft.Extensions.Configuration;
namespace CimXml2Json;

public class JsonFileUpdater
{
    public RatingsData UpdateRating(RatingsData newRating, CancellationToken cancellationToken)
    {
        if (newRating == null || newRating.transmissionFacilities == null || newRating.transmissionFacilities.Count == 0)
        {
            throw new ArgumentNullException(nameof(newRating), "RatingsData cannot be null or have empty transmission facilities.");
        }
        // Check for cancellation
        cancellationToken.ThrowIfCancellationRequested();

        string company = newRating.transmissionFacilities[0].id.Trim();
        string co = JsonFileWriter.GetCompanyFromId(company);
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
                //existingRatingsData.transmissionFacilities.Where(a=> a.id == tf.id).ToList().ForEach(a => existingRatingsData.transmissionFacilities.Remove(a));
                //existingRatingsData.transmissionFacilities.Add(tf);

                var existingTf = existingRatingsData.transmissionFacilities.FirstOrDefault(a => a.id == tf.id);
                if (existingTf != null)
                {
                    existingRatingsData.transmissionFacilities.Remove(existingTf);
                    existingRatingsData.transmissionFacilities.Add(tf);

                    foreach (var segment in tf.segments)
                    {
                        var existingSegment = existingTf.segments.FirstOrDefault(s => s.id == segment.id);
                        if (existingSegment != null)
                        {
                            existingTf.segments.Remove(existingSegment);
                            existingTf.segments.Add(segment);
                        }
                    }
                }
                else
                {
                    existingRatingsData.transmissionFacilities.Add(tf);
                }
            }
        }
        else
        {
            throw new Exception($"No existing data for CO ID '{company}' not found.");
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
}