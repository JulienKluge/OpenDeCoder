using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Edit_TrimLines : ICoder
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
            get { return "Edit"; }
            set { }
        }

        public string Name
        {
            get { return "Trim Lines"; }
            set { }
        }

        public string ParameterName_1
        {
            get { return ""; }
            set { }
        }
        public string ParameterName_2
        {
            get { return ""; }
            set { }
        }
        public string ParameterName_3
        {
            get { return "Trim Type"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Trim Type", ParameterType.ComboBox, new string[] { "Trim", "Trim Left", "Trim Right", "Trim And Merge" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int trimTypeIndex = (int)parameters[0].Value;
            return Private_Decode(pattern, trimTypeIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decode(pattern, 0), Private_Decode(pattern, 1), Private_Decode(pattern, 2), Private_Decode(pattern, 3) };
        }

        private string Private_Decode(string pattern, int trimTypeIndex)
        {
            string[] linesWithR = pattern.Split('\n');
            int linesLength = linesWithR.Length;
            if (trimTypeIndex == 0)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < linesLength; ++i)
                {
                    outString.Append(linesWithR[i].Trim());
                    if ((i + 1) < linesLength)
                    { outString.AppendLine(); }
                }
                return outString.ToString();
            }
            else if (trimTypeIndex == 1)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < linesLength; ++i)
                {
                    outString.Append((linesWithR[i].Trim('\r')).TrimStart());
                    if ((i + 1) < linesLength)
                    { outString.AppendLine(); }
                }
                return outString.ToString();
            }
            else if (trimTypeIndex == 2)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < linesLength; ++i)
                {
                    outString.Append((linesWithR[i].Trim('\r')).TrimEnd());
                    if ((i + 1) < linesLength)
                    { outString.AppendLine(); }
                }
                return outString.ToString();
            }
            else
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < linesLength; ++i)
                { outString.Append(linesWithR[i].Trim()); }
                return outString.ToString();
            }
        }
    }
}
