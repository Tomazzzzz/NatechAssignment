namespace NatechAssignmentCommon.Ipbase.Info;

public class Location
{
    public int geonames_id { get; set; }
    public float latitude { get; set; }
    public float longitude { get; set; }
    public string zip { get; set; }
    public Continent continent { get; set; }
    public Country country { get; set; }
    public City city { get; set; }
    public Region region { get; set; }
}