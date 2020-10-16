using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Configuration;

namespace Netfolder_Client
{
    public partial class Form2 : Form
    {
        
        IPAddress defaultHostAddress;

        private static Socket mainSock;
        private const string address = "10.10.20.39";
        private const int port = 8007;

        public Form2()
        {
            InitializeComponent();

            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(address), port);
            mainSock.Connect(serverAddress);
            Thread ServerON = new Thread(ClientReadThread);
            ServerON.Start();
        }
        private void ClientReadThread()
        {
            while (true)
            {
                string data = null;
                byte[] bytes = new byte[256];
                mainSock.Receive(bytes);

                data = Encoding.UTF8.GetString(bytes);

                string[] tokens = data.Split('\x01');
                string action = tokens[0];
                string question = tokens[1];
                string ID = tokens[2];
                string PW = tokens[3];

                int action1 = Int32.Parse(action);
                int question1 = Int32.Parse(question);

                if (action1 != 0)
                {
                    switch (action1)
                    {
                        case 1://로그인시도에 대한 반환
                               //ID가 존재하는지 확인하고 존재하지 않을시 1반환
                               //ID가 존재하고 PW가 매치되지 않을시 2반환
                               //ID가 존재하고 PW가 매칭되었으나 인증이 안되었을시 3반환
                               //ID가 존재하고 PW가 매칭되고 인증까지 확인시 4반환 및 로그인 리스트에 소켓등록
                            switch (question1)
                            {
                                case 1:
                                    MessageBox.Show("ID가 존재하지 않습니다.");
                                    break;
                                case 2:
                                    MessageBox.Show("PW가 틀립니다.");
                                    break;
                                case 3:
                                    MessageBox.Show("ID가 관리자의 인증을 대기중입니다.");
                                    break;
                                case 4:
                                    MessageBox.Show("로그인성공");
                                    break;
                                case 5:
                                    MessageBox.Show("이미 로그인 중인 계정입니다.");
                                    break;
                            }
                            data = null;
                            action1 = 0;

                            break;
                        case 2://회원가입시도에 대한 반환
                               //ID가 기존에 존재하는지 확인하고 존재할시 존재한다는 신호를 반환한다.1
                               //새로운ID와 PW를 DB에 등록하고 아이디등록을 완료했다는 신호를 반환한다.2
                            switch (question1)
                            {
                                case 1:
                                    MessageBox.Show("ID가 이미 존재합니다");
                                    break;
                                case 2:
                                    MessageBox.Show("ID 등록완료");
                                    break;
                            }
                            data = null;
                            action1 = 0;
                            break;
                        case 3://비밀번호찾기시도에 대한 반환
                               //ID가 기존에 존재하는지 확인하고 존재할시 매칭되는 비빌번호를 반환한다.1
                               //ID가 존재하지 않을경우 존재하지 않는다는 신호를 반환한다.2
                            switch (question1)
                            {
                                case 1:
                                    MessageBox.Show($"PW는 {PW}");
                                    break;
                                case 2:
                                    MessageBox.Show("ID가 존재하지 않습니다!!");
                                    break;
                            }
                            data = null;
                            action1 = 0;
                            break;
                    }
                }
            }
        }

        private void btn_LOGIN_Click(object sender, EventArgs e)
        {
            string data = null;
            byte[] bytes = new byte[256];
            data = "1"+"\x01"+txtID.Text+"\x01"+txtPW.Text+"\x01";
            bytes = Encoding.UTF8.GetBytes(data);
            mainSock.Send(bytes);
            data = null;
        }

        private void btn_JOIN_Click(object sender, EventArgs e)
        {
            string data = null;
            byte[] bytes = new byte[256];
            data = "2" + "\x01" + txtID.Text + "\x01" + txtPW.Text + "\x01";
            bytes = Encoding.UTF8.GetBytes(data);
            mainSock.Send(bytes);
            data = null;
        }

        private void btn_SEARCHPW_Click(object sender, EventArgs e)
        {
            string data = null;
            byte[] bytes = new byte[256];
            data = "3" + "\x01" + txtID.Text + "\x01" + txtPW.Text + "\x01";
            bytes = Encoding.UTF8.GetBytes(data);
            mainSock.Send(bytes);
            data = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientReadThread();
        }
    }
}
