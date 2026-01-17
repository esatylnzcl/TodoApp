namespace Core.Results;

public class DataResponse<T> : Response
{
    public T? Data { get; private set; }

    public DataResponse(bool success, string message, T? data) : base(success, message)
    {
        Data = data;
    }

    public static DataResponse<T> SuccessDataResponse(T data, string message = "Process succeed") =>
        new DataResponse<T>(true, message, data);

    public static DataResponse<T> ErrorDateResponse(T? data, string message = "Process failed") =>
        new DataResponse<T>(false, message, data);
}