namespace Event_Management.Application.Dto
{
    public class JobDetails
    {
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public string JobStatus { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }
        public DateTimeOffset? NextFireTimeUtc { get; set; }
    }
}
