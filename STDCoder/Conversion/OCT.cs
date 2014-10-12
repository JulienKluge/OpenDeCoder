using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Conversion_OCT : ICoder
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
            get { return "From Octal (00-377)"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("To", ParameterType.ComboBox, new string[] { "Decimal", "ASCII", "Binary", "Base64", "Hexadecimal" })
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
            bool GotInvalidCharacters = false, GotNumberFragmentation = false;
            StringBuilder newPattern = new StringBuilder(pattern);
            bool InReplacing = false;
            for (int i = 0; i < newPattern.Length; ++i)
            {
                char c = newPattern[i];
                if (!((c >= '0' && c <= '7') || c == ' '))
                {
                    newPattern.Remove(i, 1);
                    if (!InReplacing)
                    { newPattern.Insert(i, ' '); InReplacing = true; }
                    GotInvalidCharacters = true;
                    i--;
                }
                else
                {
                    InReplacing = false;
                }
            }
            string[] splits = newPattern.ToString().Split(' ');
            List<byte> dataList = new List<byte>();
            for (int i = 0; i < splits.Length; ++i)
            {
                if (!string.IsNullOrWhiteSpace(splits[i]))
                {
                    if (splits[i].Length > 3)
                    {
                        GotNumberFragmentation = true;
                        for (int j = 0; j < splits[i].Length; j += 3)
                        {
                            int conversion = Convert.ToInt32(splits[i].Substring(j, Math.Min(3, splits[i].Length - j)), 8);
                            conversion = Math.Min(255, Math.Max(0, conversion));
                            dataList.Add((byte)conversion);
                        }
                    }
                    else
                    {
                        int conversion = Convert.ToInt32(splits[i], 8);
                        conversion = Math.Min(255, Math.Max(0, conversion));
                        dataList.Add((byte)conversion);
                    }
                }
            }
            data = dataList.ToArray();
            if (data.Length == 0)
            {
                feedback = new FeedbackInfo_Error("Not enough valid characters");
                return pattern;
            }
            if (GotInvalidCharacters)
            {
                if (GotNumberFragmentation)
                {
                    feedback = new FeedbackInfo_Warning("Invalid characters removed & Number fragmentation");
                }
                else
                {
                    feedback = new FeedbackInfo_Warning("Invalid characters removed");
                }
            }
            else if (GotNumberFragmentation)
            {
                feedback = new FeedbackInfo_Warning("Number fragmentation");
            }
            if (toIndex == 0)
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
            else if (toIndex == 1)
            {
                return Encoding.ASCII.GetString(data);
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
                return Convert.ToBase64String(data);
            }
            else
            {
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < data.Length; ++i)
                {
                    outString.Append(data[i].ToString("X").PadLeft(3, '0'));
                    if (i != (data.Length - 1))
                    { outString.Append(" "); }
                }
                return outString.ToString();
            }
        }
    }
}
