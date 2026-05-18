using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace Itransition_Course_Project.Services
{
    public static class MarkdownHelper
    {
        public static HtmlString ParseMarkdownToHtml(this string? rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
                return new HtmlString(string.Empty);
            
            string safeHtml = rawText
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");

            safeHtml = Regex.Replace(safeHtml, @"^###\s+(.+)$", "<h5>$1</h5>", RegexOptions.Multiline);
            safeHtml = Regex.Replace(safeHtml, @"^##\s+(.+)$", "<h4>$1</h4>", RegexOptions.Multiline);
            safeHtml = Regex.Replace(safeHtml, @"^#\s+(.+)$", "<h3>$1</h3>", RegexOptions.Multiline);

            safeHtml = Regex.Replace(safeHtml, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
            
            safeHtml = Regex.Replace(safeHtml, @"\*(.*?)\*", "<em>$1</em>");

            safeHtml = Regex.Replace(safeHtml, @"^\*\s+(.+)$", "<li>$1</li>", RegexOptions.Multiline);
            
            if (safeHtml.Contains("<li>"))
            {
                safeHtml = safeHtml.Replace("<li>", "<ul><li>").Replace("</li>", "</li></ul>");
                safeHtml = safeHtml.Replace("</ul>\n<ul>", "\n");
            }
            
            safeHtml = safeHtml.Replace("\r\n", "<br />").Replace("\n", "<br />");

            return new HtmlString(safeHtml);
        }
    }
}