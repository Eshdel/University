using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FileManager
{
    public class Client
    {
        public bool isConnect { get; private set; }

        private Socket socket;

        public delegate void MessageHandler(string message);

        public event MessageHandler ReceiveMessageNotify;

        public event MessageHandler SendMessageNotify;


        public async void ConnectToServer(string address, int port = 8005)
        {
            string rest = null;
            
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { 
                socket.Connect(ipPoint);
            }
            catch
            {
                return;
            }

            isConnect = true;
            
            rest = await Task.Run(() => Resume());

            if (rest != null)
            {
                ReceiveMessageNotify.Invoke(rest.ToString());
                DisconnectFromServer();
                ConnectToServer(address,port);
            }
            else
            {
                if (socket.Connected)
                    socket.Close();
            }
        }

        public void SendMessageToServer(string message) 
        {
            if(!isConnect) 
                return;

            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }

        public void DisconnectFromServer() 
        {
            isConnect = false;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private async Task<string> Resume() 
        {

            byte[] data;

            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            try { 
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
            while (socket.Available > 0);
            }
            catch 
            {
                return null;
            }

            return builder.ToString();
        }
    }
}
