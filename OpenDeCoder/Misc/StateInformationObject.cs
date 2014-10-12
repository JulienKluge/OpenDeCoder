using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDeCoder.Coding;

namespace OpenDeCoder.Misc
{
    public class StateInformationObject
    {
        public string Text;
        public int SelectedTabIndex;
        public int SelectedLVIndex;
        public object[] parameterValues;

        public StateInformationObject(string currentText, int currentTabIndex, int currentLVIndex, object[] paramValues)
        {
            this.Text = currentText;
            this.SelectedTabIndex = currentTabIndex;
            this.SelectedLVIndex = currentLVIndex;
            this.parameterValues = paramValues;
        }
    }
}
