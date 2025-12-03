using System;
using System.Xml.Linq;
namespace CimXml2Json;
using AsyncLogger;

public class TransformCim2Json
{
    private Dictionary<string, Dictionary<string, XElement>> cimXElements;
    RatingsData ratingsData = new();
    string jsonFile = "ratings.json";
    Stack<string> logStrings = new Stack<string>();
    public RatingsData GetRatingsData() => ratingsData;

    AsyncFileLogger logger = new AsyncFileLogger(
            logDirectory: "Logs",
            maxFileSizeBytes: 500_000,
            minimumLevel: LogLevel.Info  // <--- Filtering enabled
        );

    public void Parse(string cimXmlFilePath)
    {
        logger.Log(LogLevel.Info, $"Starting to load CIM XML file: {cimXmlFilePath}");
        Parser parser = new Parser();
        parser.LoadXml(cimXmlFilePath);
        cimXElements = parser.cimXElements;
        logger.Log(LogLevel.Info, $"Loaded CIM XML file: {cimXmlFilePath}");

    }
    public void ToJson()
    {
    
        var Lines = cimXElements.ContainsKey("Line") ?
            cimXElements["Line"].Values.ToDictionary(x => XElementHelper.GetId(x), x => x) :
            new Dictionary<string, XElement>();

        var ACLineSegmentList = cimXElements.ContainsKey("ACLineSegment") ?
        cimXElements["ACLineSegment"].Values.Where(acs =>
            Lines.Any(line =>
                XElementHelper.HasParent(acs, line.Key, XElementHelper.EQUIPMENT_CONTAINER))).ToDictionary(x => XElementHelper.GetId(x), x => x) :
        new Dictionary<string, XElement>();

        var OperationalLimitSetList = cimXElements.ContainsKey("OperationalLimitSet") ?
        cimXElements["OperationalLimitSet"].Values.Where(ols =>
            ACLineSegmentList.Any(acs =>
                XElementHelper.HasParent(ols, acs.Key, XElementHelper.OPERATIONAL_LIMIT_SET_EQUIPMENT))).ToDictionary(x => XElementHelper.GetId(x), x => x) :
        new Dictionary<string, XElement>();

        var OperationalLimitList = cimXElements.ContainsKey("OperationalLimit") ?
        cimXElements["OperationalLimit"].Values.Where(ol =>
            OperationalLimitSetList.Any(ols =>
                XElementHelper.GetRefId(ol, "OperationalLimit.OperationalLimitSet") ==
                ols.Key)).ToDictionary(x => XElementHelper.GetId(x), x => x) :
        new Dictionary<string, XElement>();


        var OperationalLimitTypeList = cimXElements.ContainsKey("OperationalLimitType") ?
            cimXElements["OperationalLimitType"].Values.Where(olt =>
                XElementHelper.GetEnumValue(olt, "OperationalLimitType.Direction") == "AbsoluteValue").ToList() :
            new List<XElement>();



        foreach (var line in Lines)
        {
            TransmissionFacilities tf = new TransmissionFacilities();
            tf.id = XElementHelper.GetAliasName(line.Value);
            logger.Log(LogLevel.Info, $"Processing Line: {tf.id}, ID: {line.Key}");
            AddACLineSegments(ACLineSegmentList, OperationalLimitSetList, OperationalLimitList, line, tf);
            if (tf.segments.Count > 0)
                ratingsData.transmissionFacilities.Add(tf);
            logger.Log(LogLevel.Info, $"Added Transmission Facility: {tf.id} with {tf.segments.Count} segments.");
        }

        AddRatingsData();

    
    }
    public void WriteToFile(string outputPath )
    {
        string json = System.Text.Json.JsonSerializer.Serialize(ratingsData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);

    }

    private void AddACLineSegments(Dictionary<string, XElement> ACLineSegmentList, Dictionary<string, XElement> OperationalLimitSetList, Dictionary<string, XElement> OperationalLimitList, KeyValuePair<string, XElement> line, TransmissionFacilities tf)
    {
        foreach (var acs in ACLineSegmentList.Values.Where(acs =>
            XElementHelper.HasParent(acs, line.Key, XElementHelper.EQUIPMENT_CONTAINER)).ToList())
        {
            Segment segment = new Segment();
            segment.id = XElementHelper.GetAliasName(acs);

            var olsList = OperationalLimitSetList.Values.Where(ols =>
                XElementHelper.HasParent(ols, XElementHelper.GetId(acs), XElementHelper.OPERATIONAL_LIMIT_SET_EQUIPMENT)).ToList();


            //segment.id += " " + XElementHelper.GetId(acs); // For debugging purpose
            foreach (var ols in olsList)
            {
                AddOperationalLimits(OperationalLimitList, segment, ols);
            }
            if (segment.ratings.Count > 0)
                tf.segments.Add(segment);
            else
            {
                logStrings.Push($"No valid ratings found for Segment AliasName = {segment.id}.");
            }
        }
    }

    private void AddOperationalLimits(Dictionary<string, XElement> OperationalLimitList, Segment segment, XElement ols)
    {
        Period period = new Period
        {
            start = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            duration = "PT1H"
        };
        Rating rating = new Rating
        {
            period = period,
            rating_type = "AmbientAdjusted",// + XElementHelper.GetId(ols), // For debugging purpose
            metadata = new Metadata
            {
                sourceSystem = "TROLIE-APP-DEMO",
                calculationMethod = "IEEE 738 (adjusted)"
            }
        };
        var oplimits = OperationalLimitList.Values.Where(ol =>
            XElementHelper.GetRefId(ol, "OperationalLimit.OperationalLimitSet") ==
            XElementHelper.GetId(ols)).ToList();

        foreach (var ol in oplimits)
        {
            bool flowControl = AddOperationalLimit(rating, ol);
            if (!flowControl)
            {
                logStrings.Push($"Skipping OperationalLimit ID = {XElementHelper.GetId(ol)} due to following reasons.");
                continue;
            }
        }
        if (rating.values.Count > 0)
            segment.ratings.Add(rating);
        else
        {
            logStrings.Push($"No valid Operational Limits found for Segment AliasName = {segment.id}.");
            //string prefix = new string('\t', 2); // 2 tabs\
       
            foreach (var log in logStrings)
            {
                logger.Log(LogLevel.Warning, log);
            }
            logStrings.Clear();
        }
    }

    private bool AddOperationalLimit(Rating rating, XElement ol)
    {

        //Get OperationalLimitType
        var operationalLimitTypeId = XElementHelper.GetRefId(ol, "OperationalLimit.OperationalLimitType");
        if (cimXElements.ContainsKey("OperationalLimitType") == false)
        {
            logStrings.Push($"OperationalLimitType key not found in cimXElements for OperationalLimit ID = {XElementHelper.GetId(ol)}");
            return false;
        }
        var operationalLimitTypeElem = cimXElements["OperationalLimitType"].Values.FirstOrDefault(olt => XElementHelper.GetId(olt) == operationalLimitTypeId);
        if (operationalLimitTypeElem == null)
        {
            logStrings.Push($"OperationalLimitType element not found for OperationalLimit ID = {XElementHelper.GetId(ol)}");
            return false;
        }
        var direction = XElementHelper.GetEnumValue(operationalLimitTypeElem, "OperationalLimitType.Direction");
        //Process only AbsoluteValue types
        if (direction != "AbsoluteValue")
        {
            logStrings.Push($"Skipping OperationalLimit ID = {XElementHelper.GetId(ol)} as its OperationalLimitType.Direction = {direction}");
            return false;
        }

        //

        string limitType = string.Empty;
        string limitValue = string.Empty;
        string limitUnit = string.Empty;
        limitUnit = XElementHelper.GetEnumValue(ol, "OperationalLimit.OpLimitType");
        if (limitUnit == "ApparentPowerLimit")
        {
            limitUnit = "MVA";
        }
        else if (limitUnit == "ActivePowerLimit")
        {
            limitUnit = "MW";
        }
        else if (limitUnit == "CurrentLimit")
        {
            limitUnit = "A";
        }
        limitValue = XElementHelper.GetValue(ol, "OperationalLimit.Value");
        //limitValue += "  " + XElementHelper.GetId(ol); // For debugging purpose
        string limitRatingLevelMRID = XElementHelper.GetParentMRID(ol, "OperationalLimit.LimitRatingLevel");
        if (!string.IsNullOrEmpty(limitRatingLevelMRID))
        {
            if (cimXElements["LimitRatingLevel"].ContainsKey(limitRatingLevelMRID))
            {
                limitType = XElementHelper.GetValue(cimXElements["LimitRatingLevel"][limitRatingLevelMRID], "LimitRatingLevel.EMSName");
            }
        }
        if (limitType == string.Empty)
        {
            logger.Log(LogLevel.Warning, $"Limit type is empty, for OperationalLimit ID = {XElementHelper.GetId(ol)}.");
            return false;
        }

        Value value = new Value
        {
            name = limitType,
            value = limitValue,
            unit = limitUnit
        };
        logger.Log(LogLevel.Info, $"Added Operational Limit: {value.name} = {value.value} {value.unit}");
        rating.values.Add(value);
        return true;
    }

    private void AddRatingsData()
    {
        ratingsData.id = "APP-RTR" + DateTime.Now.ToString("yyyyMMdd") + "-001";
        ratingsData.comment = "Forecasted Ambient Adjusted Ratings for transmission line 12345 (segment A-B)";
    }
}