using CimXml2Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


TransformCim2Json transformCim2Json = new TransformCim2Json();
string filePath = "/Users/muddusatyanarayana/Documents/tfweb/TransmissionFacilityWebApp/data/esca60_gev_original.xml";//Path.Combine(Environment.CurrentDirectory, "data", "esca60_gev_original.xml");
transformCim2Json.Parse(filePath);
