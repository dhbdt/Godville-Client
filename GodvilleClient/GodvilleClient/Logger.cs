using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodvilleClient
{
    class Logger
    {
        static StreamWriter streamWriter = new StreamWriter(Model.Config.ErrorOutputFilePath);

        public static void AddErrorMessage(string message)
        {
            streamWriter.WriteLine(message);
        }
    }
}
