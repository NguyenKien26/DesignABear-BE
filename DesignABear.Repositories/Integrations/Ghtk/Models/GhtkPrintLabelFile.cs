namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkPrintLabelFile
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/pdf";
        public string FileName { get; set; } = "label.pdf";
    }
}