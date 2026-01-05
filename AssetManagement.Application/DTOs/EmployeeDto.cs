namespace AssetManagement.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DirectorateId { get; set; }
        public string DirectorateName { get; set; } = string.Empty;
    }
}