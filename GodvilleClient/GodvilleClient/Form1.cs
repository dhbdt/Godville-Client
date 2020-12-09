using Grpc.Net.Client;
using SimpleGrpcService;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodvilleClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitAsync();

        }

        async Task InitAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            string name = "dhbdt";
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            MessageBox.Show(reply.Message);
        }
    }
}
