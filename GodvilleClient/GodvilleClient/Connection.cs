using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GodvilleClient
{
    public class Connection
    {
        readonly List<string> aliveDispatchers = new List<string>();
        public static GrpcChannel GetDispatcherChannel()
        {
            // найти живых и выбрать среди них случайного диспетчера 
            //for (int i = 0; i < Model.Config.DispatcherList.Count; i++)
            //{
            //    //Thread t = new Thread(new ParameterizedThreadStart(CheckDispatcherIsAlive));
            //    //t.Start(myParameterObject);
            //    Thread thread = new Thread(() => CheckDispatcherIsAlive(i));
            //    thread.Start();
            //}

            return GrpcChannel.ForAddress(Model.Config.DispatcherList[0]);
        }

        public static void CheckDispatcherIsAlive(int index)
        {
            
        }

    }
}
