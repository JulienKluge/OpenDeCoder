using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Transposition_Sort : ICoder
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
            get { return false; } //I don't see any valid reason to do that...
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
            get { return "Sort"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Sort Type", ParameterType.ComboBox, new string[] { "Character Sort Ascending", "Character Sort Descending",
            "Word Sort Ascending", "Word Sort Descending",
            "Word Length Sort Ascending", "Word Length Sort Descending",
            "Case Sort Ascending", "Case Sort Descending"})
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int sortTypeIndex = (int)parameters[0].Value;
            if (sortTypeIndex == 0 || sortTypeIndex == 1)
            {
                char[] c = pattern.ToCharArray();
                List<char> cList = new List<char>(c);
                cList.Sort();
                if (sortTypeIndex == 1)
                { cList.Reverse(); }
                return new string(cList.ToArray());
            }
            else if (sortTypeIndex == 2 || sortTypeIndex == 3)
            {
                Regex regex = new Regex("[a-zA-Z]+", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                MatchCollection mc = regex.Matches(pattern);
                List<Match> ml = new List<Match>();
                for (int i = 0; i < mc.Count; ++i) { ml.Add(mc[i]); }
                ml.Sort((Match a, Match b) => { return string.Compare(a.Value, b.Value); });
                if (sortTypeIndex == 2)
                { ml.Reverse(); }
                int j = 0;
                StringBuilder outString = new StringBuilder(pattern);
                for (int i = mc.Count - 1; i >= 0; --i)
                {
                    outString.Remove(mc[i].Index, mc[i].Length);
                    outString.Insert(mc[i].Index, ml[j]);
                    j++;
                }
                return outString.ToString();
            }
            else if (sortTypeIndex == 4 || sortTypeIndex == 5)
            {
                Regex regex = new Regex("[a-zA-Z]+", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                MatchCollection mc = regex.Matches(pattern);
                List<Match> ml = new List<Match>();
                for (int i = 0; i < mc.Count; ++i) { ml.Add(mc[i]); }
                ml.Sort((Match a, Match b) =>
                {
                    if (a.Length > b.Length) { return 1; }
                    if (a.Length < b.Length) { return -1; }
                    else { return 0; }
                });
                if (sortTypeIndex == 4)
                { ml.Reverse(); }
                int j = 0;
                StringBuilder outString = new StringBuilder(pattern);
                for (int i = mc.Count - 1; i >= 0; --i)
                {
                    outString.Remove(mc[i].Index, mc[i].Length);
                    outString.Insert(mc[i].Index, ml[j]);
                    j++;
                }
                return outString.ToString();
            }
            else if (sortTypeIndex == 6 || sortTypeIndex == 7)
            {
                Regex regex = new Regex("[a-zA-Z]+", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                MatchCollection mc = regex.Matches(pattern);
                StringBuilder outString = new StringBuilder(pattern);
                for (int i = 0; i < mc.Count; ++i)
                {
                    char[] c = mc[i].Value.ToCharArray();
                    List<char> cl = new List<char>(c);
                    List<char> toAppend = new List<char>();
                    for (int j = 0; j < cl.Count; ++j)
                    {

                        if (char.IsUpper(cl[j]))
                        {
                            toAppend.Add(cl[j]);
                            cl.RemoveAt(j);
                            j--;
                        }
                    }
                    outString.Remove(mc[i].Index, mc[i].Length);
                    if (sortTypeIndex == 6)
                    { outString.Insert(mc[i].Index, new string(cl.ToArray()) + new string(toAppend.ToArray())); }
                    else
                    { outString.Insert(mc[i].Index, new string(toAppend.ToArray()) + new string(cl.ToArray())); }
                }
                return outString.ToString();
            }
            return pattern;
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
