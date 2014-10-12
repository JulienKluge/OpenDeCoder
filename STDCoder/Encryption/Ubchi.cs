using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Coding
{
    public class Coding_Encryption_Ubchi : ICoder
    {
        /*
        **
        ** TODO: Rewrite!!!!
        ** 
        */
        public bool Insert
        {
            get { return false; }
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
            get { return "Encryption"; }
            set { }
        }

        public string Name
        {
            get { return "Übchi"; }
            set { }
        }

        public ParameterInfo[] RetrieveArguments()
        {
            return new ParameterInfo[] {
                new ParameterInfo("Passphrase", ParameterType.TextBox, "password"),
                new ParameterInfo("Crypt Way", ParameterType.ComboBox, new string[] { "Decrypt", "Encrypt" })
            };
        }

        public string Decode(string pattern, ParameterResult[] parameters, bool isPreview, out IFeedbackInfo feedback)
        {
            feedback = null;
            string passphrase = (string)parameters[0].Value;
            int cryptWayIndex = (int)parameters[1].Value;
            if (string.IsNullOrWhiteSpace(passphrase))
            {
                feedback = new FeedbackInfo_Error("No valid passcode");
                return pattern;
            }
            if (passphrase.Length > pattern.Length)
            { passphrase = passphrase.Substring(0, pattern.Length); }
            int[] keyNumbers = GenerateKeyNumbers(passphrase);
            if (cryptWayIndex == 0)
            {
                return TranspositionateDecryptString(TranspositionateDecryptString(pattern, keyNumbers), keyNumbers);
            }
            else
            {
                return TranspositionateEncryptString(TranspositionateEncryptString(pattern, keyNumbers), keyNumbers);
            }
        }

        private string TranspositionateDecryptString(string pattern, int[] keyNumbers)
        {
            StringBuilder outString = new StringBuilder();
            int columnsCount = (int)Math.Ceiling((float)pattern.Length / (float)keyNumbers.Length);
            string[] columns = new string[columnsCount];
            for (int i = 0; i < columnsCount; ++i)
            { columns[i] = pattern.Substring(i * keyNumbers.Length, ((i + 1) == columnsCount) ? pattern.Length % keyNumbers.Length : keyNumbers.Length); }
            for (int i = 0; i < columnsCount; ++i)
            {
                outString.Append(columns[i] + " ");
            }
            return outString.ToString();
        }

        private string TranspositionateEncryptString(string pattern, int[] keyNumbers)
        {
            StringBuilder outString = new StringBuilder();
            int columnsCount = (int)Math.Ceiling((float)pattern.Length / (float)keyNumbers.Length);
            string[] columns = new string[columnsCount];
            for (int i = 0; i < columnsCount; ++i)
            { columns[i] = pattern.Substring(i * columnsCount, ((i + 1) == columnsCount) ? pattern.Length % columnsCount : columnsCount); }
            int min = int.MaxValue; int index = -1;
            bool[] taken = new bool[keyNumbers.Length];
            for (int i = 0; i < taken.Length; ++i) { taken[i] = false; }
            for (int i = 0; i < keyNumbers.Length; ++i)
            {
                min = int.MaxValue; index = -1;
                for (int j = 0; j < keyNumbers.Length; ++j)
                {
                    if (!taken[j])
                    {
                        if (keyNumbers[j] < min)
                        {
                            min = keyNumbers[j];
                            index = j;
                        }
                    }
                }
                taken[index] = true;
                for (int j = 0; j < columnsCount; ++j)
                {
                    if ((j + 1) == columnsCount)
                    {
                        if ((pattern.Length % keyNumbers.Length) >= (index + 1))
                        { outString.Append(columns[j][index]); }
                    }
                    else
                    { outString.Append(columns[j][index]); }
                }
            }
            return outString.ToString();
        }

        private int[] GenerateKeyNumbers(string keywords)
        {
            keywords = keywords.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                int[] keyList = new int[keywords.Length];
                bool[] taken = new bool[keywords.Length];
                for (int i = 0; i < taken.Length; ++i) { taken[i] = false; }
                char min = char.MaxValue; int index = -1; int step = 0;
                for (int i = 0; i < keyList.Length; ++i)
                {
                    min = char.MaxValue; index = -1;
                    for (int j = 0; j < keyList.Length; ++j)
                    {
                        if (!taken[j])
                        {
                            if (keywords[j] < min)
                            {
                                min = keywords[j];
                                index = j;
                            }
                        }
                    }
                    taken[index] = true;
                    step++;
                    keyList[index] = step;
                }
                return keyList;
            }
            else
            { return new int[] { 1 }; }
        }

        public string[] BruteForce(string pattern)
        {
            return new string[0];
        }
    }
}
