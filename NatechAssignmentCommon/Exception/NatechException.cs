namespace NatechAssignmentCommon.Exception;

public class NatechException : System.Exception
{
    public NatechException()
    {
    }

    public NatechException(string message)
        : base(message)
    {
    }

    public NatechException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}