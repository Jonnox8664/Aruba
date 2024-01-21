namespace DBAccess.DataModel
{
    public class User
    {
        public long Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? FiscalCode { get; set; }
        public DateTime? Birthday { get; set; }
    }
}