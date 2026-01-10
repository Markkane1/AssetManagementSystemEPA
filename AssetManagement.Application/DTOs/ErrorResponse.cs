namespace AssetManagement.Application.DTOs;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public int StatusCode { get; set; }

    public ErrorResponse(string message, int statusCode, string? details = null)
    {
        Message = message;
        StatusCode = statusCode;
        Details = details;
    }
}
