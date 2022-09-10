using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class ClientFragment : Form
    {
        Client client;

        TreeNode node;

        public ClientFragment()
        {
            InitializeComponent();
            TreeNodeMouseClickEventHandler eventHandler = (sender, args) => client.SendMessageToServer(args.Node.Text);
            TreeNodeMouseClickEventHandler nodeHandler = (sender, args) => node = args.Node;
            //TreeNodeMouseClickEventHandler eventHandler = (sender, args) => MessageBox.Show((args.Node).Text);
            this.treeView1.NodeMouseDoubleClick += eventHandler;
            this.treeView1.NodeMouseDoubleClick += nodeHandler;
      
            client = new Client();

            this.button2.Enabled = client.isConnect;
            this.button1.Enabled = !client.isConnect;

            //client.SendMessageNotify += ShowMessage;
            client.ReceiveMessageNotify += UpdateTreeView;

           // client.ClientDisconnectNotify += (msg) => MessageBox.Show(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                client.ConnectToServer(this.IpTextBox.Text);
                this.button2.Enabled = client.isConnect;
                this.button1.Enabled = !client.isConnect;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                client.DisconnectFromServer();
                this.button2.Enabled = client.isConnect;
                this.button1.Enabled = !client.isConnect;
                ClearData();
            }

            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ShowMessage(string message)
        {
            this.listBox2.Items.Add(message);
        }

        private void UpdateTreeView(string str) 
        {

            var parse = str.Split('\n').ToArray();

            if (parse[0] == "TYPE_FILE_PROTOCOL_.TXT_92320229119")
            {
                UpdateComboBox(parse[1]);

                for (int i = 2; i < parse.Length; i++)
                {
                    listBox2.Items.Add((string)parse[i]);
                }
            }

            if (parse[0] == "TYPE_FILES_PROTOCOL_92320229119")
            {
                UpdateComboBox(parse[1]);

                if(node != null) { 
                    for (int i = 2; i < parse.Length; i++) 
                    {
                   
                        node.Nodes.Add(parse[i]);
                    }
                }
                else
                {
                    for (int i = 2; i < parse.Length; i++)
                    {
                        this.treeView1.Nodes.Add(parse[i]);
                    }
                }
            } 
        }

        private void UpdateComboBox(string path)
        {
            this.label2.Text = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //client.SendMessageToServer("HI" + DateTime.Now.Second);
        }

        private void ClearData() 
        {
            this.listBox2.Items.Clear();
            this.treeView1.Nodes.Clear();
            this.label2.Text = "";
            node = null;
        }

    }
}
