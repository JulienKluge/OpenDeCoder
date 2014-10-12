using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Decoding_Numerals : ICoder
    {
        private Dictionary<string, string> replaceDict_1;
        private Dictionary<string, string> replaceDict_2;
        private Dictionary<string, string> replaceDict_3;
        public Coding_Decoding_Numerals()
        {
            replaceDict_1 = new Dictionary<string, string>();
            replaceDict_2 = new Dictionary<string, string>();
            replaceDict_3 = new Dictionary<string, string>();
            replaceDict_1.Add("one", "1");
            replaceDict_1.Add("two", "2");
            replaceDict_1.Add("three", "3");
            replaceDict_1.Add("four", "4");
            replaceDict_1.Add("five", "5");
            replaceDict_1.Add("six", "6");
            replaceDict_1.Add("seven", "7");
            replaceDict_1.Add("eight", "8");
            replaceDict_1.Add("nine", "9");
            replaceDict_1.Add("zero", "0");
            replaceDict_2.Add("eins", "1");
            replaceDict_2.Add("zwei", "2");
            replaceDict_2.Add("drei", "3");
            replaceDict_2.Add("vier", "4");
            replaceDict_2.Add("fünf", "5");
            replaceDict_2.Add("funf", "5");
            replaceDict_2.Add("fuenf", "5");
            replaceDict_2.Add("sechs", "6");
            replaceDict_2.Add("sieben", "7");
            replaceDict_2.Add("acht", "8");
            replaceDict_2.Add("neun", "9");
            replaceDict_2.Add("null", "0");
            replaceDict_3.Add("ix", "9");
            replaceDict_3.Add("x", "10");
            replaceDict_3.Add("viii", "8");
            replaceDict_3.Add("vii", "7");
            replaceDict_3.Add("vi", "6");
            replaceDict_3.Add("iv", "4");
            replaceDict_3.Add("iii", "3");
            replaceDict_3.Add("ii", "2");
            replaceDict_3.Add("i", "1");
            replaceDict_3.Add("v", "5");
        }

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
            get { return "DeCoding"; }
            set { }
        }

        public string Name
        {
            get { return "Numerals"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] { new ParameterInfo("Numeral Type", ParameterType.ComboBox, new string[] { "Numerals (English US, 0-9)", "Numerals (German, 0-9)", "Numerals Roman (1-10)" }) };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int numeralTypeIndex = (int)parameters[0].Value;
            return Private_Decrypt(pattern, numeralTypeIndex);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, 0), Private_Decrypt(pattern, 1), Private_Decrypt(pattern, 2) };
        }

        private string Private_Decrypt(string pattern, int numeralTypeIndex)
        {
            Dictionary<string, string> replaceDict;
            if (numeralTypeIndex == 0)
            { replaceDict = replaceDict_1; }
            else if (numeralTypeIndex == 1)
            { replaceDict = replaceDict_2; }
            else
            { replaceDict = replaceDict_3; }
            StringBuilder strBuilder = new StringBuilder(pattern);
            foreach (var k in replaceDict)
            {
                int index;
                while ((index = pattern.IndexOf(k.Key, StringComparison.InvariantCultureIgnoreCase)) >= 0)
                {
                    strBuilder.Remove(index, k.Key.Length);
                    strBuilder.Insert(index, k.Value);
                    pattern = strBuilder.ToString();
                }
            }
            return strBuilder.ToString();
        }
    }
}
