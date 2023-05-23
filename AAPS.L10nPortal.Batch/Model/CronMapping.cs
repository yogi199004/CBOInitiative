namespace AAPS.L10nPortal.Batch.Model
{
    public class CronMapping
    {
        public string? CronTime { get; set; }
        public string? CronDescription { get; set; }
        public bool IsActive { get; set; }
        public string? BatchDescription { get; set; }
    }
}
