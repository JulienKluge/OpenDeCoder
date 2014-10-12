using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDeCoder.Misc
{
    public static class IngressKeywords
    {
        public static Regex Get_Ingress_Keywords_Regex()
        {
            if (ingressKeywords.Length > 0)
            { return RegexKeywordsHelper.GetRegexAtomicExpressionFromKeywords(ingressKeywords); }
            else
            { return new Regex("ingress", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase); }
        }

        public static string[] ingressKeywords;

        public static void LoadKeyWords()
        {
            string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "coder/Keywords.txt");
            if (File.Exists(pluginDirectory))
            {
                string ReadedString = File.ReadAllText(pluginDirectory);
                ReadedString = (ReadedString.Replace("\r", string.Empty)).Replace('\n', ' ');
                List<string> keywordList = new List<string>();
                string[] ReadedSplit = ReadedString.Split(' ');
                for (int i = 0; i < ReadedSplit.Length; ++i)
                {
                    if (!string.IsNullOrWhiteSpace(ReadedSplit[i]))
                    {
                        keywordList.Add(ReadedSplit[i]);
                    }
                }
                keywordList = keywordList.Distinct(new KeywordStringCompare()).ToList();
                Program.DebugEntrys.Add(new ProgramDebugEntry(keywordList.Count.ToString() + " keywords parsed.", false));
                IngressKeywords.ingressKeywords = keywordList.ToArray();
            }
            else
            {
                Program.DebugEntrys.Add(new ProgramDebugEntry("Keywords file '\\coder\\Keywords.txt' does not exists!", true));
                IngressKeywords.ingressKeywords = new string[0];
            }
        }

        private static class RegexKeywordsHelper
        {
            public static Regex GetRegexAtomicExpressionFromKeywords(string[] keywords)
            {
                bool UseAtomicRegex = true;
                for (int j = 0; j < keywords.Length; ++j)
                {
                    if ((!char.IsLetterOrDigit((keywords[j])[0])) || (!char.IsLetterOrDigit((keywords[j])[keywords[j].Length - 1])))
                    {
                        UseAtomicRegex = false;
                        break;
                    }
                }
                StringBuilder regexBuilder = new StringBuilder();
                if (UseAtomicRegex)
                { regexBuilder.Append(@"(?>"); }
                else
                { regexBuilder.Append(@"("); }
                List<string> orderedKeyWords = new List<string>(keywords);
                int i = 0;
                foreach (string keyword in orderedKeyWords.OrderByDescending(w => w.Length))
                {
                    if ((i++) > 0)
                    { regexBuilder.Append('|'); }
                    regexBuilder.Append(Regex.Escape(keyword));
                }
                regexBuilder.Append(@")");
                return new Regex(regexBuilder.ToString(), RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }

        private class KeywordStringCompare : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return (x == y);
            }
            public int GetHashCode(string x)
            {
                return x.GetHashCode();
            }
        }
    }
}
