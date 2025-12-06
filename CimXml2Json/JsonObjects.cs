using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CimXml2Json;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class RatingsData
{
    [JsonPropertyName("ratings-proposal-id")]
    public string id { get; set; } = string.Empty;
    public string comment { get; set; } = string.Empty;
    [JsonPropertyName("transmission-facilities")]
    public List<TransmissionFacilities> transmissionFacilities { get; set; } = new();
}

public class TransmissionFacilities
{
    public string id { get; set; } = string.Empty;
    public List<Segment> segments { get; set; } = new();
}

public class Segment
{
    public string id { get; set; } = string.Empty;
    public List<Rating> ratings { get; set; } = new();
}

public class Rating
{
    public string rating_type { get; set; } = string.Empty;
    public Period period { get; set; } = new();
    public List<Value> values { get; set; } = new();
    public Metadata metadata { get; set; } = new();
}

public class Metadata
{
    public string sourceSystem { get; set; } = string.Empty;
    public string calculationMethod { get; set; } = string.Empty;
}

public class Value
{
    public string name { get; set; } = string.Empty;
    public string value { get; set; } = string.Empty;
    public string unit { get; set; } = string.Empty;
}

public class Period
{
    public DateTime start { get; set; } = DateTime.MinValue;
    public string duration { get; set; } = string.Empty;
}
