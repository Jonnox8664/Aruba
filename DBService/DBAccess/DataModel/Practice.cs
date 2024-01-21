namespace DBAccess.DataModel
{
    public class Practice
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int State { get; set; } = 0;
        public string? Attachment { get; set; }
    }

    public class PracticeFullData
    {
        public long PracticeId { get; set; }
        public int State { get; set; } = 0;
        public string? Attachment { get; set; }
        public long UserId { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? FiscalCode { get; set; }
        public DateTime? Birthday { get; set; }
    }
}