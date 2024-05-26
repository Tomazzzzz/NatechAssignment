using System;

namespace NatechAssignmentCommon.Ipbase.Info;

public class Timezone
{
    public string id { get; set; }
    public DateTime current_time { get; set; }
    public string code { get; set; }
    public bool is_daylight_saving { get; set; }
    public int gmt_offset { get; set; }
}