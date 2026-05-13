namespace NVCMS.API.Model
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public int TotalRecords { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int totalRecords = 0)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalRecords = totalRecords
            };
        }

        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}