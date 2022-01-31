namespace WebApi.UseCases.CustomError;

public sealed class CustomErrorResponse
{
    public string RequestId { get; set; } = string.Empty;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
