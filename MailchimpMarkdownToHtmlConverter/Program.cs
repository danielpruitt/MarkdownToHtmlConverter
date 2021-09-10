using MailchimpMarkdownToHtmlConverter.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MailchimpMarkdownToHtmlConverter
{
    public class Program
    {
        //pick up straight from my specific harddrive
        //public const string sampleOneFilePath = "C:/Users/danie/source/repos/MailchimpMarkdownToHtmlConverter/MailchimpMarkdownToHtmlConverter/MarkdownFiles/sample-one.md";
        //public const string sampleTwoFilePath = "C:/Users/danie/source/repos/MailchimpMarkdownToHtmlConverter/MailchimpMarkdownToHtmlConverter/MarkdownFiles/sample-two.md";

        static void Main(string[] args)
        {
            string filePath = string.Empty;
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            Console.WriteLine("Press 1 for Sample 1, press 2 for Sample 2, or press 3 for the Readme");
            string input = Console.ReadLine();

            int sampleNumber = 0;
            bool success = int.TryParse(input, out sampleNumber);
            if (success)
            {
                switch (sampleNumber)
                {
                    case 1:
                        Console.WriteLine("You have chosen sample one");
                        filePath = "sample-one.md";
                        break;
                    case 2:
                        Console.WriteLine("You have chosen sample two");
                        filePath = "sample-two.md";
                        break;
                    case 3:
                        Console.WriteLine("You have chosen sample two");
                        filePath = "readme.md";
                        break;
                    default:
                        Console.WriteLine("Invalid argument");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Input not valid");
            }
           

            if (!string.IsNullOrEmpty(filePath))
            {
                List<string> lines = File.ReadAllLines(projectDirectory + "/MarkdownFiles/" + filePath, Encoding.UTF8).ToList();
                if (lines.Count == 0) Console.WriteLine("File does not have lines to read.");

                List<string> generatedHtml = ParseMdLine(lines);
                generatedHtml.ForEach(x => Console.WriteLine("{0}\t", x));
            }
            else
                Console.WriteLine("No file path selected");
           
        }
        public static List<string> ParseMdLine(List<string> lines)
        {
            List<string> generatedHtml = new List<string>();
            using (ParsingManager mgr = new ParsingManager())
            {
                foreach (string line in lines)
                {
                    string trimmed = line.TrimStart(); // use this to make sure we get the first character and not just whitespace
 
                    if (string.IsNullOrEmpty(trimmed))
                    {
                        generatedHtml.Add(string.Empty);
                    }
                    else
                    {
                        mgr.SelectLineType(ref generatedHtml, line);
                    }
                   
                }
                //TODO: check for any lis here and then add the preceding ul tag and closing 
            }
            return generatedHtml;
        }       
    }
}
