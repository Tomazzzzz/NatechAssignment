namespace NatechAssignmentCommon.Ipbase.Info;

public class Data
{
    public string ip { get; set; }
    public string hostname { get; set; }
    public string type { get; set; }
    public Range_Type range_type { get; set; }
    public Connection connection { get; set; }
    public Location location { get; set; }
    public string[] tlds { get; set; }
    public Timezone timezone { get; set; }
    public Security security { get; set; }
    public Domains domains { get; set; }
}