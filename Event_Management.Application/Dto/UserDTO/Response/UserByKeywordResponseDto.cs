namespace Event_Management.Application.Dto.UserDTO.Response
{
    public  class UserByKeywordResponseDto
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
