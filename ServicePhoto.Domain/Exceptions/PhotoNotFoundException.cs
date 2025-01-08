namespace ServicePhoto.Domain.Exceptions
{
    public class PhotoNotFoundException : DomainException
    {
        public PhotoNotFoundException(string? message) : base(message)
        {
        }

        public PhotoNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
