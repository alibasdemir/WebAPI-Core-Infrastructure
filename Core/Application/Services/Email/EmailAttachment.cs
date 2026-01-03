namespace Core.Application.Services.Email
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

        public EmailAttachment(string fileName, byte[] content, string contentType = "application/octet-stream")
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("FileName cannot be null or empty.", nameof(fileName));
            if (content == null || content.Length == 0)
                throw new ArgumentException("Content cannot be null or empty.", nameof(content));

            FileName = fileName;
            Content = content;
            ContentType = contentType;
        }
    }
}
