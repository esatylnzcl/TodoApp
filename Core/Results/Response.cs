namespace Core.Results;

public class Response
{
    public bool Success { get; private set; }
    
    public string Message { get; private set; }

    public Response(bool success , string message)
    {
        Success = success;
        Message = message;
    }
    
    public static Response SuccessResponse(string message = "Process succeed")
        => new Response(true, message);

    public static Response ErrorResponse(string message = "Process failed")
        => new Response(false, message);
    
}