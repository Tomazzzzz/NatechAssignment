namespace NatechAssignmentCommon.Ipbase.Info;

public class Country
{
    public string alpha2 { get; set; }
    public string alpha3 { get; set; }
    public string[] calling_codes { get; set; }
    public Currency[] currencies { get; set; }
    public string emoji { get; set; }
    public string ioc { get; set; }
    public Language[] languages { get; set; }
    public string name { get; set; }
    public string name_translated { get; set; }
    public string[] timezones { get; set; }
    public bool is_in_european_union { get; set; }
    public string fips { get; set; }
    public int geonames_id { get; set; }
    public string hasc_id { get; set; }
    public string wikidata_id { get; set; }
}