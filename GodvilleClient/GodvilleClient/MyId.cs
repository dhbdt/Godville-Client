using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodvilleClient
{
    public class MyId
    {
        public static int TryGetMyId()
        {
            int myId = -1;
            try
            {
                using (StreamReader sr = new StreamReader(Model.Config.MyIdFilePath))
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        return myId;
                    myId = int.Parse(line);
                }
            }
            catch (Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
            return myId;
        }
        public static void SetMyId(int myId)
        {
            try
            {
                File.WriteAllText(Model.Config.MyIdFilePath, "");
                using (StreamWriter sw = new StreamWriter(Model.Config.MyIdFilePath))
                {
                    sw.Write(myId);
                }
            } catch(Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
        }

    }
}
