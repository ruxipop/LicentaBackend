namespace BackendLi.Controller.Exceptions;

[Serializable]
public class ResourceNotFoundException:Exception
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }
}