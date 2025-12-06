using CimXml2Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


TransformCim2Json transformCim2Json = new TransformCim2Json();

transformCim2Json.Parse(FilePaths.XmlFilePath);

CimXml2Json.JsonFileReader jsonFileReader = new CimXml2Json.JsonFileReader();
RatingsData ratingsData = jsonFileReader.GetByCo("ECAR");
