using System;
using System.Text;
using System.Collections.Generic;

namespace flxkbr.unknownasofyet.text
{
    public class DialogUnit
    {
        private static readonly int lineLength = Globals.MaxTextLineLength;

        public string Source { get; }
        public string Trigger { get; }
        public List<List<string>> Paragraphs { get; }

        private int linesPerParagraph;

        public DialogUnit(DialogObject dialog)
        {
            this.Source = dialog.Source;
            this.Trigger = dialog.Trigger;
            this.linesPerParagraph = (this.Source != null) ? 4 : 5;
            this.Paragraphs = calculateParagraphs(dialog.Text);
        }

        private List<List<string>> calculateParagraphs(List<string> blocks)
        {
            List<List<string>> paragraphs = new List<List<string>>();
            foreach (string block in blocks) {
                string[] words = block.Split(" ");
                List<string> paragraph = new List<string>();
                StringBuilder line = new StringBuilder();
                int len = 0;
                bool firstLine = true;
                foreach (var word in words)
                {
                    if (firstLine)
                    { // only first time
                        line.Append(word);
                        len = word.Length;
                        firstLine = false;
                    } // add to line
                    else if (len + word.Length + 1 <= lineLength)
                    {
                        line.Append(' ').Append(word);
                        len += (word.Length + 1);
                    }
                    else // new line
                    {
                        paragraph.Add(line.ToString());
                        line.Clear();
                        line.Append(word);
                        len = word.Length;
                        if (paragraph.Count == linesPerParagraph)
                        {
                            paragraphs.Add(paragraph);
                            paragraph = new List<string>();
                        }
                    }
                }
                paragraph.Add(line.ToString());
                paragraphs.Add(paragraph);
            }
            return paragraphs;
        }

        public void debug_printParagraphs()
        {
            System.Console.WriteLine($"Source: {this.Source}");
            System.Console.WriteLine($"Trigger: {this.Trigger}");
            System.Console.WriteLine("Paragraphs:");
            foreach (var para in this.Paragraphs)
            {
                foreach (var line in para)
                {
                    System.Console.WriteLine(line);
                }
                System.Console.WriteLine("-----");
            }
        }
    }
}