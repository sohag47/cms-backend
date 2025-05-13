namespace cms_backend.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "OK";
        public T Data { get; set; }

        public ApiResponse(T data, string message = "OK", bool success = true)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
