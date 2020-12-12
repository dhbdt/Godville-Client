using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GodvilleClient.GodvilleService;

namespace GodvilleClient
{
    public partial class StartForm : Form
    {
        NetworkStream networkStreamWrite;
        readonly List<string> phraseList = new List<string>();

        public StartForm()
        {
            InitializeComponent();
            lblHeroName.Text = Program.Client.HeroName;
            lblYourHealth.Text = Program.Client.CountLives.ToString();
        }

        private void btnStartDuel_Click(object sender, EventArgs e)
        {
            btnStartDuel.Visible = false;
            btnGood.Visible = true;
            btnBad.Visible = true;

            try
            {
                Thread readerThread = new Thread(new ThreadStart(ReadClientMsg));
                readerThread.Start();

                Thread writerThread = new Thread(new ThreadStart(WriteToServerMsg));
                writerThread.Start();
            }
            catch (Exception ex)
            {
                Logger.AddErrorMessage(ex.Message);
            }
        }

        void WriteToServerMsg()
        {
            TcpListener tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            tcpServer.Start();
            while (true)
            {
                TcpClient arenaServer = tcpServer.AcceptTcpClient();
                networkStreamWrite = arenaServer.GetStream();
                return;
            }
        }
        readonly Model.ClientMsg clientMsg = new Model.ClientMsg();
        void ReadClientMsg()
        {
            //GrpcChannel channel = Connection.GetDispatcherChannel();
            //var client = new GodvilleServiceClient(channel);
            //string serverAddress = client.StartDuel(new ClientData {
            //    Id = Program.Client.Id, 
            //    Nickname = Program.Client.Nickname,
            //    HealthCount = Program.Client.CountLives
            //}).Ip;

            //заглушка
            string serverAddress = "192.168.100.6:8888";
            //заглушка
            var lines = serverAddress.Split(":");


            int port = int.Parse(lines[1]);
            using (TcpClient tcpClient = new TcpClient(lines[0], port))
            {
                NetworkStream networkStreamRead = tcpClient.GetStream();
                string input;
                StreamReader sr = new StreamReader(networkStreamRead);
                while (true)
                {
                    input = sr.ReadLine();
                    if (input != null)
                    {
                        MessageBox.Show(input);
                        //Model.ClientMsg clientMsg = JsonSerializer.Deserialize<Model.ClientMsg>(input);
                        clientMsg.Type = 4;
                        clientMsg.Phrase= "hello";
                        if (clientMsg.Type == 4)
                        {
                            lblEnemyName.SetPropertyThreadSafe(() => lblEnemyName.Text, "hugi");
                        }
                    }
                }
            }
        }


        private void btnGood_Click(object sender, EventArgs e)
        {
            if (networkStreamWrite == null)
                return;
            string response = "1";
            byte[] data = Encoding.UTF8.GetBytes(response);
            networkStreamWrite.Write(data, 0, data.Length); // сказать серверу, что клиент сделал хорошо
        }

        private void btnBad_Click(object sender, EventArgs e)
        {
            if (networkStreamWrite == null)
                return;
            string response = "0";
            byte[] data = Encoding.UTF8.GetBytes(response);
            networkStreamWrite.Write(data, 0, data.Length); // сказать серверу, что клиент сделал плохо
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

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (networkStreamWrite == null)
                return;
            networkStreamWrite.Close();
        }
    }
}
