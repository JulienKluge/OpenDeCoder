using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Edit_TextInfo : ICoder
    {
        public bool Insert
        {
            get { return true; }
            set { }
        }
        public bool IsApplyAble
        {
            get { return false; }
            set { }
        }

        public bool BruteForceAble
        {
            get { return false; }
            set { }
        }
        public bool BruteForceDefaultSelected
        {
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return "Edit"; }
            set { }
        }

        public string Name
        {
            get { return "Text Info"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return null;
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            if (isPreview)
            {
                feedback = new FeedbackInfo_Info(string.Empty, true);
                StringBuilder outString = new StringBuilder();
                Dictionary<char, int> charDictionary = new Dictionary<char, int>();
                for (int i = 0; i < pattern.Length; ++i)
                {
                    if (charDictionary.ContainsKey(pattern[i]))
                    { charDictionary[pattern[i]]++; }
                    else
                    { charDictionary.Add(pattern[i], 1); }
                }
                List<KeyValuePair<char, int>> charCountList = charDictionary.ToList();
                charCountList.Sort((x, y) =>
                {
                    if (x.Value == y.Value)
                    { return 0; }
                    else if (x.Value < y.Value)
                    { return 1; }
                    else
                    { return -1; }
                });
                outString.AppendLine(charCountList.Count + " different characters");
                for (int i = 0; i < charCountList.Count; ++i )
                {
                    outString.AppendLine("\"" + charCountList[i].Key.ToString() + "\": " + charCountList[i].Value.ToString().PadLeft(3, '0'));
                }
                return outString.ToString();
            }
            feedback = null;
            return string.Empty;
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
