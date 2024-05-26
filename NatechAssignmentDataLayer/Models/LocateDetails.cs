using NatechAssignmentCommon.Enumerator;
using System;
using System.ComponentModel.DataAnnotations;

namespace NatechAssignmentDataLayer.Models;

public class LocateDetails
{
    public Guid Id { get; set; }
    [StringLength(40)]
    public string IP { get; set; }
    public LocateStatus Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    [StringLength(100)]
    public string? ResultMessage { get; set; }
    [StringLength(3)]
    public string? CountryCode { get; set; }
    [StringLength(50)]
    public string? CountryName { get; set; }
    [StringLength(50)]
    public string? TimeZone { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public LocateHeader Header { get; set; }
}