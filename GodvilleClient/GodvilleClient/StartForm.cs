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
            int myId = TryGetMyId(Model.Config.MyIdFilePath);
            if (myId == -1) {
                Model.LoginData loginData = new Model.LoginData();
                LoginForm loginForm = new LoginForm(loginData);
                loginForm.ShowDialog();
            }
        }
        int TryGetMyId(string path)
        {
            int myId = -1;
            try
            {
                StreamReader sr = new StreamReader(path);
                string line = sr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return myId;
                myId = int.Parse(line);
            }
            catch (Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
            return myId;
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
