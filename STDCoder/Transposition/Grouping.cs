using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Transposition_Grouping : ICoder
    {
        public bool Insert
        {
            get { return true; }
            set { }
        }
        public bool IsApplyAble
        {
            get { return true; }
            set { }
        }

        public bool BruteForceAble
        {
            get { return true; } //yearh this makes sense. Imagine a grouping of hex values or something like that.
            set { }
        }
        public bool BruteForceDefaultSelected
        {
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return "Transposition"; }
            set { }
        }

        public string Name
        {
            get { return "Grouping"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Group Size", ParameterType.TextBox, "2"),
                new ParameterInfo("Delimiter", ParameterType.TextBox, " "),
                new ParameterInfo("Sort Type", ParameterType.ComboBox, new string[] { "Delete Delimiters", "Keep Delimiters" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            string groupSizeArg = (string)parameters[0].Value;
            int groupSize;
            if (int.TryParse(groupSizeArg, out groupSize))
            {
                string delimiterArg = (string)parameters[1].Value;
                string delimiter = " ";
                if (!string.IsNullOrEmpty(delimiterArg))
                { delimiter = delimiterArg; }
                int sortTypeIndex = (int)parameters[2].Value;
                return Private_Decrypt(pattern, groupSize, delimiter, sortTypeIndex == 0);
            }
            else
            {
                feedback = new FeedbackInfo_Error("'Group Size' Argument is not a number");
            }
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, 2, " ", false), Private_Decrypt(pattern, 3, " ", false) };
        }

        private string Private_Decrypt(string pattern, int groupSize, string delimiter, bool DeleteDelimiter)
        {
            if (DeleteDelimiter)
            {
                pattern = pattern.Replace(delimiter, "");
            }
            string[] lines = pattern.Split('\n');
            StringBuilder outString = new StringBuilder();
            for (int i = 0; i < lines.Length; ++i)
            {
                if (i == (lines.Length - 1))
                {
                    outString.Append(SplitIntoGroups(lines[i].Trim('\r'), delimiter, groupSize));
                }
                else
                {
                    outString.AppendLine(SplitIntoGroups(lines[i].Trim('\r'), delimiter, groupSize));
                }
            }
            return outString.ToString();
        }

        private string SplitIntoGroups(string pattern, string delimiter, int size)
        {
            StringBuilder outString = new StringBuilder(pattern);
            int count = (int)Math.Floor((double)pattern.Length / (double)size);
            if (count > 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    outString.Insert((i + 1) * size + (i * delimiter.Length), delimiter);
                }
            }
            return outString.ToString();
        }
    }
}
