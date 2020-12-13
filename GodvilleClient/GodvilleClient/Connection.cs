using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GodvilleClient.GodvilleService;

namespace GodvilleClient
{
    public class Connection
    {
        static readonly List<string> aliveDispatchers = new List<string>();
        public static GrpcChannel GetDispatcherChannel()
        {
            Task[] tasks = new Task[Model.Config.DispatcherList.Count];
            // найти живых и выбрать среди них случайного диспетчера 
            for (int i = 0; i < Model.Config.DispatcherList.Count; i++)
            {
                //Thread thread = new Thread(() => CheckDispatcherIsAlive(i));
                //thread.Start();

                tasks[i] = Task.Factory.StartNew(() => CheckDispatcherIsAlive(i));
                //tasks[i].Start();

                //CheckDispatcherIsAlive(i);


            }
            Task.WaitAll(tasks);
            Random random = new Random();
            int dispatcher = random.Next(aliveDispatchers.Count - 1);

            return GrpcChannel.ForAddress(Model.Config.DispatcherList[dispatcher]);
        }

        public static void CheckDispatcherIsAlive(int index)
        {
            var channel = GrpcChannel.ForAddress(Model.Config.DispatcherList[index]);
            var client = new GodvilleServiceClient(channel);

            try
            {
                client.Check(new Empty { }, deadline: DateTime.UtcNow.AddSeconds(5));
                aliveDispatchers.Add(Model.Config.DispatcherList[index]);
            }
            catch(Exception e)
            {
                // диспетчер недоступен, ничего не делаем
            }
        }

    }
}
