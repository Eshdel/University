using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Manager_Server
{
    public partial class ServerView : Form
    {
        Server server;
        public ServerView()
        {
           InitializeComponent();
           server = new Server("127.0.0.1", 8005);
            //

            server.ReceiveMessageNotify += DisplayMessage;
            server.SendMessageNotify += DisplayMessage;
            this.radioButton2.Checked = true;
        }

        void DisplayMessage(string message) 
        {
            this.listBox1.Items.Add(message);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try { 
                if(radioButton1.Checked)
                    server.TurnOnAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try 
            { 
                if (radioButton2.Checked)
                    server.TurnOff();
            }
            catch(Exception exception) 
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
