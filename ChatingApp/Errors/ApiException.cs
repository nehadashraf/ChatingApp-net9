namespace ChatingApp.Errors
{
    public class ApiException(int statusCode,string meassage,string?details)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = meassage;
        public string? Details { get; set; } = details;
    }
}
