namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int TotalRecords { get; set; }

        /// <summary>Mã lỗi máy đọc được (vd ZALO_API_ERROR). Chỉ có khi Success = false.</summary>
        public string? ErrorCode { get; set; }

        /// <summary>Mã truy vết để đối chiếu log.</summary>
        public string? TraceId { get; set; }

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

        public static ApiResponse<T> ErrorResponse(string message, string errorCode, string? traceId, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                TraceId = traceId,
                Data = data
            };
        }
    }
}
