namespace DBAccess.DataModel
{
    public class History
    {
        public long Id { get; set; }
        public long PracticeId { get; set; }
        public int State { get; set; } = 0;
        public DateTime? Timestamp { get; set; }
    }
}