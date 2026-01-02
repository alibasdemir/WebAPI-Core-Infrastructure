namespace Application.Features.UserOperationClaims.Queries.GetUsersByOperationClaimId
{
    public class GetUsersByOperationClaimIdResponseDTO
    {
        public int OperationClaimId { get; set; }
        public int TotalUsers { get; set; }
        public IList<UserInfoDTO> Users { get; set; }
    }

    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}