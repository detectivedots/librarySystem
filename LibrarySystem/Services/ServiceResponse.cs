namespace LibrarySystem.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Entities { get; set; }
    }
}
