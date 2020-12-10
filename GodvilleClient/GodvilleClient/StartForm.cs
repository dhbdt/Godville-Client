using Grpc.Net.Client;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodvilleClient
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            InitAsync();
        }

        async Task InitAsync()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:8888");
            var client = new Greeter.GreeterClient(channel);
            string name = "dhbdt";
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            MessageBox.Show(reply.Message);
        }
    }
}
