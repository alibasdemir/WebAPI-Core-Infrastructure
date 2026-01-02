namespace Application.Features.Tests.Commands.SoftDelete
{
    public class SoftDeleteTestResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "Test soft deleted successfully.";
        public DateTime DeletedDate { get; set; }
    }
}