namespace NatechAssignmentCommon.DTO;

public class GetLocationForIPResponse
{
    public string IP { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string TimeZone { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}