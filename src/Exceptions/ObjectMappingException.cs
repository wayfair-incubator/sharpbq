using System.Runtime.Serialization;

namespace sharpbq.Exceptions;

[Serializable]
public class ObjectMappingException : Exception
{
    public ObjectMappingException() : base("Failed to map query result to target class")
    {
    }

    public ObjectMappingException(string message) : base(message)
    {
    }

    protected ObjectMappingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}