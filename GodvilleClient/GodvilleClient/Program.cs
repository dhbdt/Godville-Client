
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

            int myId = TryGetMyId(Model.Config.MyIdFilePath);
            Model.LoginData loginData = new Model.LoginData();
            if (myId == -1)
            {
                LoginForm loginForm = new LoginForm(loginData);
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // найти живых и выбрать среди них случайного диспетчера 
                    
                    using var channel = GrpcChannel.ForAddress("http://192.168.100.6:5000");

                    var client = new GodvilleServiceClient(channel);
                    try
                    {
                        string serverIp = client.Login(
                            new LoginData
                            {
                                Login = loginData.Login,
                                Password = loginData.Password,
                                ClientIp = ip
                            }).Ip;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return;
                    }
                    // создать сокет, который слушает этот ip
                    // получить от сервера через сокет свой id, если все успешно (внутри реализовать "дай мне другой сервер, если этот не отвечает")
                    // if (myId == -1)
                    //      MessageBox.Show("Неверное имя пользователя или пароль");
                    // повторить, пока не успех (не if, a while)
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

                        string serverIp = client.Register(
                            new RegisterData
                            {
                                LoginData = new LoginData { Login = regData.Login, Password = regData.Password, ClientIp = ip },
                                Nickname = regData.Nickname
                            }).Ip;

                        // создать сокет, который слушает этот ip
                        // получить от сервера через сокет свой id, если все успешно (внутри реализовать "дай мне другой сервер, если этот не отвечает")
                    }
                    else
                        return;
                }
                else if (result == DialogResult.Cancel)
                    return;
            }
            Application.Run(new StartForm());
        }
        static int TryGetMyId(string path)
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
    }
}
