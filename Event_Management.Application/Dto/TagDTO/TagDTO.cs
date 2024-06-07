namespace Event_Management.Application.Dto
{
	public class TagDTO
    {
        //public int TagId { get; set; }
        public string TagName { get; set; } = null!;
        public Guid EventId { get; set; }
    }
}
