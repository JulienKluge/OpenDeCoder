using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Conversion_ASCII : ICoder
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
            get { return "From ASCII"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("To", ParameterType.ComboBox, new string[] { "HEX", "Dec", "Binary", "Base64", "Octal" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int toIndex = (int)parameters[0].Value;
            return Private_Decode(pattern, toIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decode(pattern, 0), Private_Decode(pattern, 1), Private_Decode(pattern, 2), Private_Decode(pattern, 3), Private_Decode(pattern, 4) };
        }

        private string Private_Decode(string pattern, int toIndex)
        {
            byte[] data = Encoding.ASCII.GetBytes(pattern);
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
                return Convert.ToBase64String(data);
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
    }
}
