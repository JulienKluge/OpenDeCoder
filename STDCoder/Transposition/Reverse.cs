using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Transposition_Reverse : ICoder
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
            get { return true; }
            set { }
        }
        public bool BruteForceDefaultSelected
        {
            get { return true; }
            set { }
        }

        public string Header
        {
            get { return "Transposition"; }
            set { }
        }

        public string Name
        {
            get { return "Reverse"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Type", ParameterType.ComboBox, new string[] { "Reverse Columns", "Reverse Rows", "Complete Reverse", "Reverse Words" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int typeIndex = (int)parameters[0].Value;
            return Private_Decrypt(pattern, typeIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, 0), Private_Decrypt(pattern, 1), Private_Decrypt(pattern, 2), Private_Decrypt(pattern, 3) };
        }

        private string Private_Decrypt(string pattern, int typeIndex)
        {
            if (typeIndex == 0)
            {
                string[] lines = pattern.Split('\n');
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (i == (lines.Length - 1))
                    { outString.Append(ReverseString(lines[i].Trim('\r'))); }
                    else
                    { outString.AppendLine(ReverseString(lines[i].Trim('\r'))); }
                }
                return outString.ToString();
            }
            else if (typeIndex == 1)
            {
                string[] lines = pattern.Split('\n');
                StringBuilder outString = new StringBuilder();
                for (int i = lines.Length - 1; i >= 0; --i)
                {
                    if (i == 0)
                    { outString.Append(lines[i].Trim('\r')); }
                    else
                    { outString.AppendLine(lines[i].Trim('\r')); }
                }
                return outString.ToString();
            }
            else if (typeIndex == 2)
            {
                string[] lines = pattern.Split('\n');
                StringBuilder outString = new StringBuilder();
                for (int i = lines.Length - 1; i >= 0; --i)
                {
                    if (i == 0)
                    { outString.Append(ReverseString(lines[i].Trim('\r'))); }
                    else
                    { outString.AppendLine(ReverseString(lines[i].Trim('\r'))); }
                }
                return outString.ToString();
            }
            else
            {
                Regex regex = new Regex("[a-zA-Z]+", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                MatchCollection mc = regex.Matches(pattern);
                StringBuilder outString = new StringBuilder(pattern);
                for (int i = mc.Count - 1; i >= 0; --i)
                {
                    outString.Remove(mc[i].Index, mc[i].Length);
                    outString.Insert(mc[i].Index, ReverseString(mc[i].Value));
                }
                return outString.ToString();
            }
        }

        private string ReverseString(string str)
        {
            StringBuilder outString = new StringBuilder();
            for (int i = str.Length - 1; i >= 0; --i)
            {
                outString.Append(str[i]);
            }
            return outString.ToString();
        }
    }
}
