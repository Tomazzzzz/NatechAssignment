using System;

namespace NatechAssignmentCommon.DTO;

public class GetLocationForListOfIpStatusResponse
{
    public int TotalIpCount { get; set; }
    public int ProcessedIpCount { get; set; }
    public TimeSpan EstimatedTime { get; set; }
}