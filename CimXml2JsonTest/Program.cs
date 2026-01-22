using CimXml2Json;
using Microsoft.Extensions.Configuration;

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Initialize FilePaths with configuration
var filePaths = new FilePaths(configuration);
FilePaths.Instance = filePaths;

Console.WriteLine("Hello, World!");

TransformCim2Json transformCim2Json = new TransformCim2Json();

transformCim2Json.Parse(filePaths.XmlFilePath);
CimXml2Json.JsonFileReader jsonFileReader = new CimXml2Json.JsonFileReader();
RatingsData ratingsData = jsonFileReader.GetByCo("NEPOOL");

Console.WriteLine("RatingsData loaded for CO: NEPOOL");
CimXml2Json.JsonFileWriter jsonFileWriter = new CimXml2Json.JsonFileWriter();
var cancellationTokenSource = new CancellationTokenSource();
var newRatingsData = jsonFileWriter.AddRealTimeRating(ratingsData, cancellationTokenSource.Token);
Console.WriteLine("RatingsData written back to JSON file.");
