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
            Model.LoginData loginData = new Model.LoginData();
            if (myId == -1)
            {
                LoginForm loginForm = new LoginForm(loginData);
                DialogResult result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // ����� ����� � ������� ����� ��� ���������� ���������� 
                    // ��������� ������ ���������� � �������� ������ �� ������
                    // string serverIp = Login(loginData);
                    // ������� �����, ������� ������� ���� ip
                    // �������� �� ������� ����� ����� ���� id, ���� ��� ������� (������ ����������� "��� ��� ������ ������, ���� ���� �� ��������")
                    // if (myId == -1)
                    //      MessageBox.Show("�������� ��� ������������ ��� ������");
                    // ���������, ���� �� ����� (�� if, a while)
                }
                else if (result == DialogResult.Ignore)
                {
                    Model.RegisterData regData = new Model.RegisterData();
                    RegisterForm rf = new RegisterForm(regData);
                    if (rf.ShowDialog() == DialogResult.OK)
                    {
                        // � ��� ���� ��� ������
                        // ����� ����� � ������� ����� ��� ���������� ���������� 
                        // ��������� ������ ���������� � �������� ������ �� ������
                        // string serverIp = Register(regData);
                        // ������� �����, ������� ������� ���� ip
                        // �������� �� ������� ����� ����� ���� id, ���� ��� ������� (������ ����������� "��� ��� ������ ������, ���� ���� �� ��������")
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
        
    }
}
