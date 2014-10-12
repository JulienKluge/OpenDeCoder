using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeCoder.Misc
{
    public class ProgramDebugEntry
    {
        public string Message = string.Empty;
        public readonly DateTime TimeStamp;
        public bool IsError = false;

        public ProgramDebugEntry(string message, bool isError = false)
        {
            this.Message = message;
            this.TimeStamp = DateTime.Now;
            this.IsError = isError;
        }
    }
}
