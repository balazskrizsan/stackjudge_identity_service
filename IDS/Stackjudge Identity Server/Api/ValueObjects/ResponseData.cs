namespace Stackjudge_Identity_Server.Api.ValueObjects;

public class ResponseData<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
    public string RequestId { get; set; }
}