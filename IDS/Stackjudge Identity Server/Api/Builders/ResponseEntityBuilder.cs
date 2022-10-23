using System.Net;
using Stackjudge_Identity_Server.Api.ValueObjects;

namespace Stackjudge_Identity_Server.Api.Builders;

public class ResponseEntityBuilder<T>
{
    public T Data { get; set; }
    public int ErrorCode { get; set; }
    public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;

    public ResponseEntity<ResponseData<T>> Build()
    {
        bool success = ErrorCode == 0;

        var data = new ResponseData<T>
        {
            Data = Data, Success = success, ErrorCode = ErrorCode, RequestId = "1"
        };

        return new ResponseEntity<ResponseData<T>>(data)
        {
            StatusCode = ResponseStatusCode.GetHashCode()
        };
    }
}