namespace Domain.Entities
{
    public class UserDailyUploadStats
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public DateOnly Date { get; set; }
        public int UploadCount { get; set; }
    }
}
