using System.Runtime.Serialization;

namespace sharpbq.Exceptions;

[Serializable]
public class ErrorResultException : Exception
{
    public ErrorResultException() : base("Error fetching query result from GBQ")
    {
    }

    public ErrorResultException(string message) : base(message)
    {
    }
    
    protected ErrorResultException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}