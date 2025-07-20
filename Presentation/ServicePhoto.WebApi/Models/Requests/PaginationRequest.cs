namespace ServicePhoto.WebApi.Models.Requests
{
    public class PaginationRequest
    {
        public int Take { get; set; }
        public int Offset { get; set; }
    }
}
