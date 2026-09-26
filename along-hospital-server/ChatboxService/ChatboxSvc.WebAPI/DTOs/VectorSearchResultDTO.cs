namespace ChatboxSvc.WebAPI.DTOs
{
    public class VectorSearchResultDTO
    {
        public float Score { get; set; }
        public string PayloadJson { get; set; } = "";
    }
}
