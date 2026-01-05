namespace AssetManagement.Application.DTOs
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int AssetItemId { get; set; }
        public string AssetItemSerialNumber { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}