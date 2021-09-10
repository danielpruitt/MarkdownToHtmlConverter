using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailchimpMarkdownToHtmlConverter.Managers
{
    public class ParsingManager : IDisposable
    {
        private bool disposedValue;


        public void SelectLineType(ref List<string> generatedHtml, string line)
        {
            int hashCount = line.TrimStart().Count(x => (x == '#'));
            bool hasHyperlink = CheckForHyperlink(line);

            switch (hashCount)
            {
                case 1:
                    ParseHeader1Html(ref generatedHtml, line);
                    break;
                case 2:
                    ParseHeader2Html(ref generatedHtml, line);
                    break;
                case 6:
                    ParseHeader6Html(ref generatedHtml, line);
                    break;
                case 0: // means unformatted text
                    ParseUnformattedText(ref generatedHtml, line);
                    break;
                default:
                    break;
            }

            if (hasHyperlink)
            {
                ParseLinkText(ref generatedHtml);
            }
        }
        public bool CheckForHyperlink(string line)
        {
            // look for both signifiers of a hyperlink and http
            if (line.Contains("[") && line.Contains("]") && line.Contains("http")) return true;
            else return false;
        }
        public void ParseHeader1Html(ref List<string> generatedHtml, string line)
        {
            string newLine = line.Replace("#", "<h1>");
            newLine += "</h1>";
            generatedHtml.Add(newLine);
        }

        public void ParseHeader2Html(ref List<string> generatedHtml, string line)
        {
            string newLine = line.Replace("##", "<h2>");
            newLine += "</h2>";
            generatedHtml.Add(newLine);
        }

        public void ParseHeader6Html(ref List<string> generatedHtml, string line)
        {
            string newLine = line.Replace("######", "<h6>");
            newLine += "</h6>";
            generatedHtml.Add(newLine);
        }
        public void ParseUnformattedText(ref List<string> generatedHtml, string line)
        {
            // this would be where there would be a check for if lines start with an identifier for lists. at least add the lis in here
            string prevLine = generatedHtml.LastOrDefault();
            //handles if a line continues to the nexts
            if (prevLine.EndsWith("</p>"))
            {
                generatedHtml.RemoveAt(generatedHtml.Count - 1);
                generatedHtml.Add(prevLine.Replace("</p>", ""));
                generatedHtml.Add(line + "</p>");
            }
            else
            {
                generatedHtml.Add("<p>" + line + "</p>");
            }
        }
        public void ParseLinkText(ref List<string> generatedHtml)
        {
            string lastLine = generatedHtml.LastOrDefault();
            int startNameIndex = lastLine.IndexOf("[") + "[".Length;
            int endNameIndex = lastLine.LastIndexOf("]");

            //start at index of the first [ and use difference of the end and start to find the ]
            string hrefName = lastLine.Substring(startNameIndex, endNameIndex - startNameIndex);
            
            int startUrlIndex = lastLine.IndexOf("(") + "(".Length;
            int endUrlIndex = lastLine.LastIndexOf(")");

            string urlName = lastLine.Substring(startUrlIndex, endUrlIndex - startUrlIndex);

            string combinedUrlAndHref = "<a href=" + urlName + ">"+ hrefName + "</a>";
            int length = endUrlIndex - startNameIndex;
            
            //remove at index, then look for the smaller value between the length of original text to be replaced and the differce of total length of last line and start index
            //that way we handle if the replacement happens early then we insert start of the name index. 
            string newLastLine = lastLine.Remove(startNameIndex , Math.Min(length, lastLine.Length - startNameIndex)).Insert(startNameIndex, combinedUrlAndHref).Replace("[", "").Replace(")", "");
           
            generatedHtml.RemoveAt(generatedHtml.Count - 1);
            generatedHtml.Add(newLastLine);
        }
        public void GenerateListHtmlFromLi(ref List<string> generatedList, List<string> lineItems)
        {
            //not yet test
            string ul = "<ul>";
            foreach (string li in lineItems)
            {
                ul += li + "\n";
            }
            ul += "</ul>";

            //TODO: remove the lines from the generatedList and replace with the ul 
        }

        //Standard Dispose. Not need to make it more of a thing for right now 
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ParsingManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
