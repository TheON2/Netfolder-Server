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
using System.Data.SQLite;
using Netfolder_Client;

namespace Netfolder_Server
{

    public partial class Form1 : Form
    {
        Socket mainSock;
        string dumID;
        string dumPW;
        string dumKEY;
        string dumONOFF;

        List<Socket> List1 = new List<Socket>();//로그인 성공 이전의 클라이언트 소켓 리스트
        List<Socket> List2 = new List<Socket>();//로그인 성공한 클라이언트 소켓 리스트
        public Form1()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            mainSock.Bind(new IPEndPoint(IPAddress.Any, 8007));
            mainSock.Listen(10);
            mainSock.BeginAccept(AcceptCallback, null);
        }

        private void ServerReadThread(object Client)
        {
            Socket client = (Socket)Client;
            while (true)
            {
                string data = null;
                byte[] bytes = new byte[256];
                client.Receive(bytes);

                data = Encoding.UTF8.GetString(bytes);

                string[] tokens = data.Split('\x01');
                string action = tokens[0];
                string ID = tokens[1];
                string PW = tokens[2];

                string DBID=null;
                string DBPW=null;
                string DBKEY=null;
                string DBONOFF = null;

                int action1 = Convert.ToInt32(action);

                if (action1 != 0)
                {          
                    switch (action1)
                    {
                        case 1://로그인시도
                            
                            SQLiteDataReader rdr1 = Sqlload("SELECT * FROM ACCOUNT WHERE ID='" + ID + "';");

                            while(rdr1.Read())
                            {
                                DBID = rdr1["ID"].ToString();
                                DBPW = rdr1["PW"].ToString();
                                DBKEY = rdr1["KEY"].ToString();
                                DBONOFF = rdr1["ONOFF"].ToString();
                            }

                            if (DBID != ID)
                            {
                                data = "1" + "\x01" + "1" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                bytes = Encoding.UTF8.GetBytes(data);
                                client.Send(bytes);
                            }//ID가 존재하는지 확인하고 존재하지 않을시 1반환
                            else
                            {
                                if (DBPW != PW)
                                {
                                    data = "1" + "\x01" + "2" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                    bytes = Encoding.UTF8.GetBytes(data);
                                    client.Send(bytes);
                                }//ID가 존재하고 PW가 매치되지 않을시 2반환
                                else
                                {
                                    if (DBKEY == "NO")
                                    {
                                        data = "1" + "\x01" + "3" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                        bytes = Encoding.UTF8.GetBytes(data);
                                        client.Send(bytes);
                                    }//ID가 존재하고 PW가 매칭되었으나 인증이 안되었을시 3반환
                                    else
                                    {
                                        if (DBONOFF == "ON")
                                        {
                                            data = "1" + "\x01" + "5" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                            bytes = Encoding.UTF8.GetBytes(data);
                                            client.Send(bytes);
                                        }//ID가 존재하고 PW가 매칭되었으나 인증까지 되었으나 이미 로그인중일 경우 5반환
                                        else
                                        {
                                            SQLiteDataReader rdr5 = Sqlload("UPDATE ACCOUNT SET ONOFF = 'ON' WHERE ID='" + ID + "';");
                                            rdr5.Read();
                                            data = "1" + "\x01" + "4" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                            bytes = Encoding.UTF8.GetBytes(data);
                                            client.Send(bytes);
                                            List2.Add(client);
                                        }//ID가 존재하고 PW가 매칭되고 인증까지 확인시 4반환 및 로그인 리스트에 소켓등록
                                    }
                                }
                            }
                            
                            action1 = 0;
                            data = null;
                            break;
                        case 2://회원가입시도
                            SQLiteDataReader rdr2 = Sqlload("select * from ACCOUNT WHERE ID='" + ID + "';");
                            while (rdr2.Read())
                            {
                                DBID = rdr2["ID"].ToString();
                                DBPW = rdr2["PW"].ToString();
                                DBKEY = rdr2["KEY"].ToString();
                            }
                            if (DBID != ID)
                            {
                                SQLiteDataReader rdr3 = Sqlload("INSERT INTO ACCOUNT VALUES ('" + ID + "', '" + PW + "','NO','OFF') ");
                                rdr3.Read();
                                data = "2" + "\x01" + "2" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                bytes = Encoding.UTF8.GetBytes(data);
                                client.Send(bytes);
                            }//새로운ID와 PW를 DB에 등록하고 아이디등록을 완료했다는 신호를 반환한다.2
                            else
                            {                               
                                data = "2" + "\x01" + "1" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                bytes = Encoding.UTF8.GetBytes(data);
                                client.Send(bytes);
                            }//ID가 기존에 존재하는지 확인하고 존재할시 존재한다는 신호를 반환한다.1
                            action1 = 0;
                            data = null;
                            break;
                        case 3://비밀번호찾기시도
                            SQLiteDataReader rdr4 = Sqlload("select * from ACCOUNT WHERE ID='" + ID + "';");
                            while (rdr4.Read())
                            {
                                DBID = rdr4["ID"].ToString();
                                DBPW = rdr4["PW"].ToString();
                                DBKEY = rdr4["KEY"].ToString();
                            }
                            if (DBID != ID)
                            {
                                data = "3" + "\x01" + "2" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
                                bytes = Encoding.UTF8.GetBytes(data);
                                client.Send(bytes);
                            }//ID가 존재하지 않을경우 존재하지 않는다는 신호를 반환한다.2
                            else
                            {
                                data = "3" + "\x01" + "1" + "\x01" + DBID + "\x01" + DBPW + "\x01";
                                bytes = Encoding.UTF8.GetBytes(data);
                                client.Send(bytes);
                            }//ID가 기존에 존재하는지 확인하고 존재할시 매칭되는 비빌번호를 반환한다.1
                            action1 = 0;
                            data = null;
                            break;
                    }
                }
            }
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = null;
            byte[] bytes = new byte[256];
            data = "2" + "\x01" + "1" + "\x01" + txtSENDID.Text + "\x01" + txtSendPW.Text + "\x01";
            bytes = Encoding.UTF8.GetBytes(data);
            
                for (int i = 0; i < List1.Count; i++)
                {
                    Socket socket = List1[i];
                    socket.Send(bytes);
                }           
        }
        void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = mainSock.EndAccept(ar);

            Thread ServerRead = new Thread(new ParameterizedThreadStart(ServerReadThread));
            ServerRead.Start(client);
            List1.Add(client);
            // 또 다른 클라이언트의 연결을 대기한다.
            mainSock.BeginAccept(AcceptCallback, null);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainSock.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source = C:/sqlite-tools-win32-x86-3330000/one.db;");
            conn.Open();

            String sql = "select * from ACCOUNT";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string ID = rdr["ID"].ToString();
                string PW = rdr["PW"].ToString();
                string KEY = rdr["KEY"].ToString();
                string ONOFF = rdr["ONOFF"].ToString();

                MessageBox.Show(ID +PW +KEY +ONOFF);
            }
            rdr.Close();
        }
        private SQLiteDataReader Sqlload(string sql)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source = C:/sqlite-tools-win32-x86-3330000/one.db;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteDataReader rdr5 = Sqlload("UPDATE ACCOUNT SET KEY = 'YES' WHERE ID='" + txtSENDID.Text + "';");
            rdr5.Read();
            MessageBox.Show(txtSENDID.Text + "의 사용인증을 완료했습니다.");
        }
    }
}
