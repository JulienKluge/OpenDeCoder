using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Edit_Replace : ICoder
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
            get { return "Find & Replace"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Find Pattern", ParameterType.TextBox, ""),
                new ParameterInfo("Replace Pattern", ParameterType.TextBox, ""),
                new ParameterInfo("Search Kind", ParameterType.ComboBox, new string[] { "Normal search", "Word search", "Regex search" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            string findPattern = (string)parameters[0].Value;
            string replacePattern = (string)parameters[1].Value;
            int searchKindIndex = (int)parameters[2].Value;
            if (findPattern.Length == 0)
            { feedback = new FeedbackInfo_Error("Search string length is 0"); return pattern; }
            feedback = null;
            if (searchKindIndex == 0)
            {
                return pattern.Replace(findPattern, replacePattern);
            }
            else if (searchKindIndex == 1)
            {
                string result = ""; bool worked = false;
                try
                {
                    Regex regex = new Regex("\\b" + Regex.Escape(findPattern) + "\\b", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                    result = regex.Replace(pattern, replacePattern);
                    worked = true;
                }
                catch (Exception) { feedback = new FeedbackInfo_Error("Invalid Regex"); }
                if (worked)
                { return result; }
                else
                { return pattern; }
            }
            else
            {
                string result = ""; bool worked = false;
                try
                {
                    Regex regex = new Regex(findPattern, RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
                    result = regex.Replace(pattern, replacePattern);
                    worked = true;
                }
                catch (Exception) { feedback = new FeedbackInfo_Error("Invalid Regex"); }
                if (worked)
                { return result; }
                else
                { return pattern; }
            }
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
