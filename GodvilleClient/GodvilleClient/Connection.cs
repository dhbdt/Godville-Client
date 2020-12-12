using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodvilleClient
{
    public class Connection
    {
        public static GrpcChannel GetDispatcherChannel()
        {
            // найти живых и выбрать среди них случайного диспетчера 
            return GrpcChannel.ForAddress(Model.Config.DispatcherList[0]);
        }

    }
}
