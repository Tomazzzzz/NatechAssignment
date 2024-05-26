namespace NatechAssignmentCommon.Ipbase.Error;

public class ErrorResponse
{
    public string message { get; set; }
    public Errors errors { get; set; }
    public string info { get; set; }
}