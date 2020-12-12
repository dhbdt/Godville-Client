using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GodvilleClient.Model
{
    [Serializable]
    public class ClientData 
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int CountLives { get; set; }
        DataContractJsonSerializer formatter;
        public ClientData()
        {
            Id = -1;
        }
        public void TryGetClient()
        {
            try
            {
                using (StreamReader sr = new StreamReader(Config.MyIdFilePath))
                {
                    string json = sr.ReadLine();
                    ClientData deserialize = JsonSerializer.Deserialize<ClientData>(json);
                    Id = deserialize.Id;
                    Nickname = deserialize.Nickname;
                    CountLives = deserialize.CountLives;
                }
            }
            catch (Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
            return;
        }
        public void SetMyId()
        {
            try
            {
                string json = JsonSerializer.Serialize<ClientData>(this);
                File.WriteAllText(Model.Config.MyIdFilePath, json);
            } catch(Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
        }
    }
}
