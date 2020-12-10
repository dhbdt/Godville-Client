using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodvilleClient
{
    static class Program
    {
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
            if (myId == -1)
            {
                Model.LoginData loginData = new Model.LoginData();
                LoginForm loginForm = new LoginForm(loginData);
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // отправить данные на сервер
                    // myId = Login(loginData);
                    // получить от сервера свой id, если все успешно
                    // если id = -1, то неверное имя пользователя и пароль
                    // повторить, пока не успех
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
        
    }
}
