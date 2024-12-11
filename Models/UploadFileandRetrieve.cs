namespace WebApiValidation.Models
{
    public class UploadFileandRetrieve
    {
        public int FileId { get; set; }
        public int Course_Id { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[] FileContent { get; set; }
        public Course? Course { get; set; }
    }
}
