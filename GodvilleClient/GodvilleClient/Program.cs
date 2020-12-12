
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GodvilleClient.GodvilleService;

namespace GodvilleClient
{
    static class Program
    {
        static readonly string ip = GetLocalIPAddress();
        public static Model.ClientData Client { get; set; } = new Model.ClientData();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Client.TryGetClient();
            Model.LoginData loginData = new Model.LoginData();
            while (Client.Id == -1)
            {
                LoginForm loginForm = new LoginForm(loginData);
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using var channel = Connection.GetDispatcherChannel();
                    var serviceClient = new GodvilleServiceClient(channel);
                    string serverIp;
                    try
                    {
                        serverIp = serviceClient.Login(
                            new LoginData
                            {
                                Login = loginData.Login,
                                Password = loginData.Password,
                                ClientIp = ip
                            }).Ip;
                    }
                    catch (Exception e)
                    {
                        Logger.AddErrorMessage(e.Message);
                        return;
                    }
                    Client.Id = ConnectServerCheckMyId(serverIp);
                    if (Client.Id == -1)
                        MessageBox.Show("Неверное имя пользователя или пароль");
                    else
                        Client.SetMyId();
                }
                else if (result == DialogResult.Ignore)
                {
                    Model.RegisterData regData = new Model.RegisterData();
                    RegisterForm rf = new RegisterForm(regData);
                    if (rf.ShowDialog() == DialogResult.OK)
                    {
                        using var channel = Connection.GetDispatcherChannel();
                        var serviceClient = new GodvilleServiceClient(channel);

                        string serverIp;
                        try
                        {
                            serverIp = serviceClient.Register(
                                new RegisterData
                                {
                                    LoginData = new LoginData { Login = regData.Login, Password = regData.Password, ClientIp = ip },
                                    Nickname = regData.Nickname
                                }).Ip;
                        } catch(Exception e)
                        {
                            Logger.AddErrorMessage(e.Message);
                            return;
                        }
                        Client.Id = ConnectServerCheckMyId(serverIp);
                    }
                    else
                        return;
                }
                else if (result == DialogResult.Cancel)
                    return;
            }
            Application.Run(new StartForm());
        }
        
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        static int ConnectServerCheckMyId(string serverAddres)
        {
            int myId = -1;
            var lines = serverAddres.Split(":");
            try
            {
                using (TcpClient tcpClient = new TcpClient(lines[0], int.Parse(lines[1])))
                {
                    using (NetworkStream networkStream = tcpClient.GetStream())
                    {
                        using (StreamReader sr = new StreamReader(networkStream))
                        {
                            string input = sr.ReadLine();
                            myId = int.Parse(input);
                        }
                    }
                }
            } catch(Exception e)
            {
                Logger.AddErrorMessage(e.Message);
            }
            return myId;
        }
    }
}
