namespace ThunderServer.API.DTOs
{
    public sealed record FileUploadResponse
    {
        public List<ThunderFileDto>? ThunderFiles { get; set; }
    }
}