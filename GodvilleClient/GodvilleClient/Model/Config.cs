using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodvilleClient.Model
{
    class Config
    {
        public static string dateTimeNow = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now).ToString().Replace(":", ".");
        public static string ErrorOutputFilePath { get; set; } = "log " + dateTimeNow +".txt";
        public static string MyIdFilePath { get; set; } = "myid.txt";
        public static List<string> DispatcherList { get; set; } = new List<string>() { "http://localhost:8888" };
    }
}
