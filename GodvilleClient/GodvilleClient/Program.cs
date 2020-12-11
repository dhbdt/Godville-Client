
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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int myId = MyId.TryGetMyId();
            Model.LoginData loginData = new Model.LoginData();
            while (myId == -1)
            {
                LoginForm loginForm = new LoginForm(loginData);
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // найти живых и выбрать среди них случайного диспетчера 
                    using var channel = GrpcChannel.ForAddress(Model.Config.DispatcherList[0]);
                    var client = new GodvilleServiceClient(channel);
                    string serverIp;
                    try
                    {
                        serverIp = client.Login(
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
                    myId = ConnectServerCheckMyId(serverIp);
                    if (myId == -1)
                        MessageBox.Show("Неверное имя пользователя или пароль");
                    else
                        MyId.SetMyId(myId);
                }
                else if (result == DialogResult.Ignore)
                {
                    Model.RegisterData regData = new Model.RegisterData();
                    RegisterForm rf = new RegisterForm(regData);
                    if (rf.ShowDialog() == DialogResult.OK)
                    {
                        // найти живых и выбрать среди них случайного диспетчера 
                        using var channel = GrpcChannel.ForAddress(Model.Config.DispatcherList[0]);
                        var client = new GodvilleServiceClient(channel);

                        string serverIp;
                        try
                        {
                            serverIp = client.Register(
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
                        myId = ConnectServerCheckMyId(serverIp);
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
