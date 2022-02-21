
namespace FlagshipProductTest.Shared.Models
{
    public class Document : BaseModel
    {
        public long UserId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string RawData { get; set; }
        public string Extension { get; set; }
    }

    public enum DocumentType
    {
        Passport = 1
    }
}
