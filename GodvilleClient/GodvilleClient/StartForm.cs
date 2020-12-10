using Grpc.Net.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodvilleClient
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }
        
        async Task InitAsync()
        {
            using var channel = GrpcChannel.ForAddress(Model.Config.DispatcherList[0]);
            var client = new Greeter.GreeterClient(channel);
            string name = "dhbdt";
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            MessageBox.Show(reply.Message);
        }
    }
}
