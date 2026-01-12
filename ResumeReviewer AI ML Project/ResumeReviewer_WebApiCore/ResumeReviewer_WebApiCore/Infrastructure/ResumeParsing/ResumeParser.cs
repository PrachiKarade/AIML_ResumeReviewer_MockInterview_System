using DocumentFormat.OpenXml.Packaging;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ResumeReviewer_WebApiCore.Infrastructure.ResumeParsing
{
    public class ResumeParser:IResumeParser
    {
        public async Task<string> ParseAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            return extension switch
            {
                ".pdf" => await ParsePdf(file),
                ".docx" => await ParseDocx(file),
                _ => throw new NotSupportedException("Unsupported file type")
            };
        }

        private async Task<string> ParsePdf(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var pdf = PdfDocument.Open(stream);

            var text = new StringWriter();

            foreach (Page page in pdf.GetPages())
            {
                text.WriteLine(page.Text);
            }

            return await Task.FromResult(text.ToString().Trim());
        }

        private async Task<string> ParseDocx(IFormFile file)
        {
            using var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            stream.Position = 0;

            using var wordDoc = WordprocessingDocument.Open(stream, false);

            var body = wordDoc.MainDocumentPart?.Document?.Body;

            if (body == null)
                return string.Empty;

            return body.InnerText.Trim();
        }
    }
}
