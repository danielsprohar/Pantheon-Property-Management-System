namespace Nubles.Core.Application.Wrappers.Generics
{
    public class ApiResponse<T> : ApiResponse where T : class
    {
        public T Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(string message) : base(message)
        {
        }

        public ApiResponse(T data, string message = null)
        {
            Data = data;
            Message = message;
            Succeeded = true;
        }
    }
}