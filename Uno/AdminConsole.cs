using System;
using System.Configuration;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class AdminConsole : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        Form1 form1;
        public CustomLabel lblTitleExtern;
        PlayerDatabase playerDatabase;
        Deck deck;

        public bool newMessage = false;

        ServerHost serverHost;

        public AdminConsole()
        {
            InitializeComponent();

            lblTitleExtern = lblTitle;
        }

        public void SetReferences(Form1 form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.serverHost = form1.serverHost;
            this.deck = form1.deck;
        }

        private void btnSendDataToServer_Click(object sender, EventArgs e)
        {
            string command = txtCommandInput.Text;
            txtCommandInput.Text = string.Empty;

            if (command.ToLower().StartsWith("help") || command.ToLower().StartsWith("commands"))
            {
                AppendCommandBox("Commands: DRAW, LIST, CLEAR");
                AppendCommandBox("Do NOT use MSG command from the console.");
            }
            else if (command.ToLower().StartsWith("list"))
            {
                AppendCommandBox("All Cards:");
                string allCards = string.Empty;
                foreach (Card card in deck.cardsDeckList)
                {
                    allCards += $"ID: {card.ID}, Card: {card.Color} {card.ToString()} || ";
                }
                AppendCommandBox(allCards);
                AppendCommandBox("Players Cards:");
                foreach (Player player in playerDatabase.players)
                {
                    AppendCommandBox("Player: " + player.Name);
                    string playerCards = string.Empty;
                    foreach (Card card in player.playerInventory)
                    {
                        playerCards += $"ID: {card.ID}, Card: {card.Color} {card.ToString()} || ";
                    }
                    AppendCommandBox(playerCards);
                }
            }
            else if (command.ToLower().StartsWith("clear"))
            {
                txtCommandLog.Clear();
            }
            else
            {

                if (cmbPlayerSelection.SelectedIndex != -1)
                {
                    playerDatabase.NamePlayerDictionary.TryGetValue(cmbPlayerSelection.SelectedItem.ToString().Trim(), out Player player);
                    playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);
                    serverHost.SendDataToSpecificClient(txtCommandInput.Text, client);
                }
                else
                {
                    MessageBox.Show("Select a player");
                }
            }
        }

        public void AppendCommandBox(string message, Color? color = null, string sender = "Server")
        {

            CustomRichTextBox txtBox = txtCommandLog;

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

        private void cmbPlayerSelection_DropDown(object sender, EventArgs e)
        {
            cmbPlayerSelection.Items.Clear();

            foreach (Player player in playerDatabase.players)
            {
                cmbPlayerSelection.Items.Add(player.Name);
            }
        }
    }
}
