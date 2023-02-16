namespace DLock.Models
{
    public class ApiResponse<T>
    {
        public IEnumerable<string> Messages { get; set; }
        public T Data { get; set; }
    }

    public class ApiResponse : ApiResponse<object>
    {
    }
}
