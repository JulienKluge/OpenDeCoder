using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Decoding_Morse : ICoder
    {
        private List<string[]> codes;

        public Coding_Decoding_Morse()
        {
            codes = new List<string[]>();
            codes.Add(new string[] { " ", "|" });
            codes.Add(new string[] { "A", ".-" });
            codes.Add(new string[] { "B", "-..." });
            codes.Add(new string[] { "C", "-.-." });
            codes.Add(new string[] { "D", "-.." });
            codes.Add(new string[] { "E", "." });
            codes.Add(new string[] { "F", "..-." });
            codes.Add(new string[] { "G", "--." });
            codes.Add(new string[] { "H", "...." });
            codes.Add(new string[] { "I", ".." });
            codes.Add(new string[] { "J", ".---" });
            codes.Add(new string[] { "K", "-.-" });
            codes.Add(new string[] { "L", ".-.." });
            codes.Add(new string[] { "M", "--" });
            codes.Add(new string[] { "N", "-." });
            codes.Add(new string[] { "O", "---" });
            codes.Add(new string[] { "P", ".--." });
            codes.Add(new string[] { "Q", "--.-" });
            codes.Add(new string[] { "R", ".-." });
            codes.Add(new string[] { "S", "..." });
            codes.Add(new string[] { "T", "-" });
            codes.Add(new string[] { "U", "..-" });
            codes.Add(new string[] { "V", "...-" });
            codes.Add(new string[] { "W", ".--" });
            codes.Add(new string[] { "X", "-..-" });
            codes.Add(new string[] { "Y", "-.--" });
            codes.Add(new string[] { "Z", "--.." });
            codes.Add(new string[] { "0", "-----" });
            codes.Add(new string[] { "1", ".----" });
            codes.Add(new string[] { "2", "..---" });
            codes.Add(new string[] { "3", "...--" });
            codes.Add(new string[] { "4", "....-" });
            codes.Add(new string[] { "5", "....." });
            codes.Add(new string[] { "6", "-...." });
            codes.Add(new string[] { "7", "--..." });
            codes.Add(new string[] { "8", "---.." });
            codes.Add(new string[] { "9", "----." });
            codes.Add(new string[] { ".", ".-.-.-" });
            codes.Add(new string[] { ",", "--..--" });
            codes.Add(new string[] { "?", "..--.." });
            codes.Add(new string[] { "-", "-....-" });
            codes.Add(new string[] { "=", "-...-" });
            codes.Add(new string[] { ":", "---..." });
            codes.Add(new string[] { ";", "-.-.-." });
            codes.Add(new string[] { "(", "-.--." });
            codes.Add(new string[] { ")", "-.--.-" });
            codes.Add(new string[] { "/", "-..-." });
            codes.Add(new string[] { "\"", ".-..-." });
            codes.Add(new string[] { "$", "...-..-" });
            codes.Add(new string[] { "'", ".----." });
            codes.Add(new string[] { Environment.NewLine, ".-.-.." });
            codes.Add(new string[] { "_", "..--.-" });
            codes.Add(new string[] { "@", ".--.-." });
            codes.Add(new string[] { "!", "---." });
            codes.Add(new string[] { "!", "-.-.--" });
            codes.Add(new string[] { "+", ".-.-." });
            codes.Add(new string[] { "~", ".-..." });
            codes.Add(new string[] { "#", "...-.-" });
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
            get { return false; }
            set { }
        }

        public string Header
        {
            get { return "DeCoding"; }
            set { }
        }

        public string Name
        {
            get { return "Morse"; }
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
            get { return "Crypt Way"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] { new ParameterInfo("Crypt Way", ParameterType.ComboBox, new string[] { "Decrypt", "Encrypt"}) };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            int cryptWayIndex = (int)parameters[0].Value;
            return Private_Decrypt(pattern, cryptWayIndex == 0);
        }

        public string[] BruteForce(string pattern)
        {
            return new string[] { Private_Decrypt(pattern, true), Private_Decrypt(pattern, false) };
        }

        private string Private_Decrypt(string pattern, bool decrypt)
        {
            if (decrypt)
            {
                pattern = pattern.Replace("–", "--").Replace("—", "---").Replace("_", "-").Replace("•", ".").Replace("…", "...");
                string[] sCodes = pattern.Split(' ');
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < sCodes.Length; ++i)
                {
                    string s = sCodes[i];
                    bool processed = false;
                    for (int j = 0; j < codes.Count; ++j)
                    {
                        if (codes[j][1] == s)
                        {
                            outString.Append(codes[j][0]);
                            processed = true;
                        }
                    }
                    if (!processed)
                    { outString.Append(s); }
                }
                return outString.ToString();
            }
            else
            {
                pattern = pattern.ToUpperInvariant();
                StringBuilder outString = new StringBuilder();
                for (int i = 0; i < pattern.Length; ++i)
                {
                    string s = pattern[i].ToString();
                    bool processed = false;
                    for (int j = 0; j < codes.Count; ++j)
                    {
                        if (codes[j][0] == s)
                        {
                            outString.Append(codes[j][1] + " ");
                            processed = true;
                        }
                    }
                    if (!processed)
                    { outString.Append(" " + s + " "); }
                }
                return outString.ToString();
            }
        }

    }
}
