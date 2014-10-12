using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Conversion_Base64 : ICoder
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
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return "Conversion"; }
            set { }
        }

        public string Name
        {
            get { return "From Base64"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("To", ParameterType.ComboBox, new string[] { "HEX", "Dec", "Binary", "ASCII", "Octal" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int toIndex = (int)parameters[0].Value;
            IFeedbackInfo outFeedback;
            string resultPattern = Private_Decrypt(pattern, toIndex, out outFeedback);
            feedback = outFeedback;
            return resultPattern;
        }

        public string[] BruteForce(string pattern)
        {
            string[] outStringArray = new string[5];
            IFeedbackInfo o;
            for (int i = 0; i < outStringArray.Length; ++i)
            {
                outStringArray[i] = Private_Decrypt(pattern, i, out o);
            }
            return outStringArray;
        }

        private string Private_Decrypt(string pattern, int toIndex, out IFeedbackInfo feedback)
        {
            feedback = null;
            byte[] data = new byte[0];
            bool Converted = false;
            try
            {
                data = Convert.FromBase64String(pattern);
                Converted = true;
            }
            catch (Exception) { Converted = false; }
            if (!Converted)
            {
                List<char> dataList = new List<char>();
                char[] c = pattern.ToCharArray();
                for (int i = 0; i < c.Length; ++i)
                {
                    if (IsCharValidBase64(c[i]))
                    { dataList.Add(c[i]); }
                }
                if (dataList.Count > 3)
                {
                    for (int i = dataList.Count - 3; i >= 0; --i)
                    {
                        if (dataList[i] == '=')
                        {
                            dataList.RemoveAt(i);
                        }
                    }
                }
                int removeCount = dataList.Count % 4;
                for (int i = 0; i < removeCount; ++i)
                { dataList.RemoveAt(dataList.Count - 1); }
                if (dataList.Count == 0)
                {
                    feedback = new FeedbackInfo_Error("Not enough valid characters");
                    return pattern;
                }
                else
                {
                    feedback = new FeedbackInfo_Warning("Invalid characters removed");
                }
                data = Convert.FromBase64String(new string(dataList.ToArray()));
            }
            if (toIndex == 0)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < data.Length; ++i)
                {
                    outString.Append(data[i].ToString("X"));
                    if (i != (data.Length - 1))
                    { outString.Append(" "); }
                }
                return outString.ToString();
            }
            else if (toIndex == 1)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < data.Length; ++i)
                {
                    outString.Append(data[i].ToString().PadLeft(3, '0'));
                    if (i != (data.Length - 1))
                    { outString.Append(" "); }
                }
                return outString.ToString();
            }
            else if (toIndex == 2)
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < data.Length; ++i)
                {
                    outString.Append(Convert.ToString(data[i], 2).PadLeft(8, '0'));
                    if (i != (data.Length - 1))
                    { outString.Append(" "); }
                }
                return outString.ToString();
            }
            else if (toIndex == 3)
            {
                return Encoding.ASCII.GetString(data);
            }
            else
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < data.Length; ++i)
                {
                    outString.Append(Convert.ToString(data[i], 8).PadLeft(3, '0'));
                    if (i != (data.Length - 1))
                    { outString.Append(" "); }
                }
                return outString.ToString();
            }
        }

        private bool IsCharValidBase64(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '+' || c == '/' || c == '=')
            {
                return true;
            }
            return false;
        }
    }
}
