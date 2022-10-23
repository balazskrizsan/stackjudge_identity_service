using Microsoft.AspNetCore.Mvc;

namespace Stackjudge_Identity_Server.Api.ValueObjects;

public class ResponseEntity<T> : ObjectResult
{
    public int CustomStatusCode { get; set; }

    public ResponseEntity(T value) : base(value)
    {
        StatusCode = CustomStatusCode;
    }
}