namespace NatechAssignmentCommon.Ipbase.Info;

public class Security
{
    public bool? is_anonymous { get; set; }
    public bool? is_datacenter { get; set; }
    public bool? is_vpn { get; set; }
    public bool? is_bot { get; set; }
    public bool? is_abuser { get; set; }
    public bool? is_known_attacker { get; set; }
    public bool? is_proxy { get; set; }
    public bool? is_spam { get; set; }
    public bool? is_tor { get; set; }
    public bool? is_icloud_relay { get; set; }
    public int? threat_score { get; set; }
}