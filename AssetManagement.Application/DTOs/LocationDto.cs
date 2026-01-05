namespace AssetManagement.Application.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsHeadOffice { get; set; }
        public string? District { get; set; }
    }
}