using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Uno.Classes;

namespace Uno
{
    public partial class ChatBox : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        Form1 form1;
        public CustomLabel lblTitleExtern;

        public ChatBox()
        {
            InitializeComponent();

            lblTitleExtern = lblTitle;
        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
        }


        private void btnSendDataToServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSendDataToServer.Text))
            {
                string message = txtSendDataToServer.Text;
                AppendChatBox(message, Color.Blue, form1.currentPlayer.Name);

                switch (form1.isHost)
                {
                    case true:
                        form1.serverHost.BroadcastData("MSG " + form1.currentPlayer.Name + " " + message);
                        break;
                    case false:
                        form1.serverJoin.SendDataToServer("MSG " + form1.currentPlayer.Name + " " + message);
                        break;
                }

                txtSendDataToServer.Text = string.Empty;

            }
        }
        public void AppendChatBox(string message, Color? color = null, string sender = "Server")
        {
            CustomRichTextBox txtBox = txtChatBox;

            txtBox.SelectionStart = txtBox.Text.Length;
            txtBox.SelectionLength = 0;

            txtBox.SelectionColor = color ?? Color.Red;
            txtBox.AppendText($"{sender}: ");
            txtBox.SelectionColor = txtBox.ForeColor;
            txtBox.AppendText(message + Environment.NewLine);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
