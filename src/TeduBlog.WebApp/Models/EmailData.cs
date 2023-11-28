namespace TeduBlog.WebApp.Models
{
    public class EmailData
    {
        public required string ToEmail { get; set; }
        public required string Subject { get; set; }
        public string? Content { get; set; }
        public string? Template { get; set; }
        public dynamic? TemplateData { get; set; }
        public string? FromName { get; set; }
        public string? FromEmail { get; set; }
        public string? ToName { get; set; }
    }
}
