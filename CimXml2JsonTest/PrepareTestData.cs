using CimXml2Json;
public class PrepareTestData
{

    public static Metadata GetMetaData() => new Metadata() { sourceSystem = "TROLIE-APP-DEMO", calculationMethod = "IEEE 738 (adjusted)" };
    public static Value GetLDSHDValue() => new Value() { name = "LDSHD", value = "100", unit = "A" };
    public static Value GetEMERGValue() => new Value() { name = "EMERG", value = "50", unit = "A" };
    public static Period GetPeriod() => new Period() { start = DateTime.Now, duration = "PT1H" };
    public static Rating GetRating() => new Rating()
    {
        rating_type = "AmbientAdjusted",
        period = GetPeriod(),
        values = new List<Value>() { GetLDSHDValue(), GetEMERGValue() },
        metadata = GetMetaData()
    };
    public static Segment GetSegment() => new Segment()
    {
        id = "ECAR ECAR 12345 1",
        ratings = new List<Rating>() { GetRating(), GetRating() }
    };
    public static TransmissionFacilities GetTransmissionFacilities() => new TransmissionFacilities()
    {
        id = "ECAR5 ECAR 12345",
        segments = new List<Segment>() { GetSegment(), GetSegment() }
    };
    public static RatingsData GetRatingsData() => new RatingsData()
    {
        id = "SomeId",
        comment = "Test Comment",
        transmissionFacilities = new List<TransmissionFacilities>() { GetTransmissionFacilities(), GetTransmissionFacilities() }
    };

}