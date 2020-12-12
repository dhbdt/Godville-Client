using Grpc.Net.Client;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GodvilleClient.GodvilleService;

namespace GodvilleClient
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void btnStartDuel_Click(object sender, EventArgs e)
        {
            btnStartDuel.Visible = false;
            btnGood.Visible = true;
            btnBad.Visible = true;

            //GrpcChannel channel = Connection.GetDispatcherChannel();
            //var client = new GodvilleServiceClient(channel);
            //string serverAddress = client.StartDuel(new ClientData {
            //    Id = Program.Client.Id, 
            //    Nickname = Program.Client.Nickname,
            //    HealthCount = Program.Client.CountLives
            //}).Ip;

            using (TcpClient tcpClient = new TcpClient("127.0.0.1", 8006))
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    using (StreamReader sr = new StreamReader(networkStream))
                    {
                        var input = sr.ReadLine();
                        Console.WriteLine(input);
                    }
                }
            }
        }

        private void btnGood_Click(object sender, EventArgs e)
        {

        }

        private void btnBad_Click(object sender, EventArgs e)
        {

        }

        private void btnGetStat_Click(object sender, EventArgs e)
        {

        }

        private void linkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var channel = Connection.GetDispatcherChannel();
            var client = new GodvilleServiceClient(channel);
            client.Logout(new ClientId { Id = Program.Client.Id });
        }
    }
}
