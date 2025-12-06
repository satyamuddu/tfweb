using CimXml2Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


CimXml2Json.JsonFileReader jsonFileReader = new CimXml2Json.JsonFileReader();
RatingsData ratingsData = jsonFileReader.GetByCo("ECAR");
