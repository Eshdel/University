using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace File_Manager_Server
{
    public class Server
    {
        private static Server singleton;

        public delegate void MessageHandler(string message);

        public event MessageHandler ReceiveMessageNotify;

        public event MessageHandler SendMessageNotify;

        public bool isActive { get; private set; } = false;

        IPEndPoint ipPoint;

        Socket listenSocket;

        Thread serverThread;

        public Server(string ip, int port = 8050)
        {

            if (singleton == null)
            {
                singleton = this;
                singleton.ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            }
            else { throw new ArgumentException("Server already exists"); }
        }

        public void TurnOff()
        {
            singleton.isActive = false;
            if (listenSocket != null)
                if(listenSocket.Connected)
                    singleton.listenSocket.Close();
        }

        public async Task TurnOnAsync()
        {
            singleton.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            singleton.isActive = true;
            singleton.listenSocket.Bind(ipPoint);
            singleton.listenSocket.Listen(1);

            if(await Task.Run(() => Start()))
            {
                Update();
            }
        }

        private async Task<string> Resume()
        {
            Socket handler;

            try { 
                 handler = listenSocket.Accept();
            }
            catch
            {
                return null;
            }

            byte[] data;
            data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт
            try { 
                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);
            }
            catch
            {
                handler.Close();
                return "";
            }

            string message = GetAnswer(builder.ToString());

            byte[] msg = Encoding.Unicode.GetBytes(message);
            handler.Send(msg);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            return builder.ToString();
        }

        private async void Update() 
        {
            string rest = null;

            rest = await Task.Run(() => Resume());

            if (rest != null)
            {
                ReceiveMessageNotify.Invoke(rest.ToString());

                if(singleton.isActive)
                    Update();
            }
        }

        private string GetAnswer(string request) 
        {
            if (request.Contains(".txt"))
                return GetFileData(request);

            else
                return GetDirectoryInfo(request);
        }

        private async Task<bool> Start() 
        {

            Socket handler;
            handler = listenSocket.Accept();
            
            string message = GetDirectoryInfo();
            byte[] msg = Encoding.Unicode.GetBytes(message);
            handler.Send(msg);

            handler.Close();

            return true;
        }

        private string GetDirectoryInfo(string dirName = "EmptyTypeDirecory") 
        {
            DirectoryInfo directory;
            string data = "";

            if (dirName == "EmptyTypeDirecory") { 

                DriveInfo[] drives = DriveInfo.GetDrives();

                data = "TYPE_FILES_PROTOCOL_92320229119\n";

                foreach (DriveInfo drive in drives)
                {
                    var dirs = Directory.GetDirectories(drive.Name);

                    foreach (string s in dirs)
                    {
                        data += s + "\n";
                    }
                }
            }

            try { directory = new DirectoryInfo(dirName); }

            catch
            {
                return "Error";
            }


            if (directory.Exists)
            {
                data = "TYPE_FILES_PROTOCOL_92320229119\n";
                data += directory.FullName + "\n";


                string[] dirs = null;

                try {
                    dirs = Directory.GetDirectories(dirName);
                }
                catch 
                {
                    return "";
                }
                foreach (string s in dirs)
                {
                    data += s + "\n";
                }

                string[] files = Directory.GetFiles(dirName);
                foreach (string s in files)
                {
                    data += s + "\n";
                }
            }

            return data;
        }


        private string GetFileData(string path) 
        {

            if (File.Exists(path))
            {
                string data = "TYPE_FILE_PROTOCOL_.TXT_92320229119\n";
                data += path + "\n";
                var streamReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8);
                data += streamReader.ReadToEnd();
                return data;
            }

            else 
                return "File don't find";
        }
    }
}
