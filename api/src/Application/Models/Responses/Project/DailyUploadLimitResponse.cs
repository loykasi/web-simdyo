namespace Application.Models.Responses.Project
{
    public class DailyUploadLimitResponse
    {
        public int Limit { get; set; }
        public int UploadCount { get; set; }
        public int Remaining => Limit - UploadCount;
    }
}
