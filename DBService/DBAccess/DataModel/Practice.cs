namespace DBAccess.DataModel
{
    public class Practice
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int State { get; set; } = 0;
        public string? Attachment { get; set; }
    }
}