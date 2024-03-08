using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uno.Classes;

namespace Uno
{
    public partial class ChatBox : Form
    {
        CustomPictureBox openChatBox;
        Image[] chatBoxStates;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        frmUno form1;
        public CustomLabel lblTitleExtern;

        public bool newMessage = false;

        public ChatBox()
        {
            InitializeComponent();

            lblTitleExtern = lblTitle;
        }

        public void SetReferences(frmUno form1, CustomPictureBox openChatBox)
        {
            this.form1 = form1;
            this.openChatBox = openChatBox;
            chatBoxStates = new Image[] {Properties.Resources.Chat, Properties.Resources.ChatNewMessage };
        }

        private async void btnSendDataToServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessageBox.Text))
            {
                string message = txtMessageBox.Text;
                AppendChatBox(message, Color.Blue, form1.currentPlayer.Name);

                switch (form1.isHost)
                {
                    case true:
                        await form1.serverHost.BroadcastData("MSG " + form1.currentPlayer.Name + " " + message);
                        break;
                    case false:
                        await form1.serverJoin.SendDataToServer("MSG " + form1.currentPlayer.Name + " " + message);
                        break;
                }

                txtMessageBox.Text = string.Empty;
            }
        }

        public void OpenChatBox()
        {
            if (openChatBox.Image != chatBoxStates[0])
                openChatBox.Image = chatBoxStates[0];
        }

        public void AppendChatBox(string message, Color? color = null, string sender = "Server")
        {
            if (!this.Visible && openChatBox.Image != chatBoxStates[1])
            {
                openChatBox.Image = chatBoxStates[1];
            }

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

        private void txtMessageBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                btnSendDataToServer_Click(sender, e);
            }
        }
    }
}
