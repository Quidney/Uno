using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;

namespace Uno
{
    public partial class Form1 : Form
    {
        public Player currentPlayer;
        public bool joinedOrHosted = false;
        public bool isHost = false;

        public Deck deck;
        public CardFunctionality cardFunctionality;
        public PlayerDatabase playerDatabase;
        public ServerHost serverHost;
        public ServerJoin serverJoin;

        public ChatBox chatBox;
        public AdminConsole adminConsole;

        public Form1()
        {
            InitializeComponent();

            InitMethod();
        }

        void InitMethod()
        {
            chatBox = new ChatBox();
            adminConsole = new AdminConsole();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Uno!";

            deck = new Deck();
            deck.Shuffle();

            cardFunctionality = new CardFunctionality();

            playerDatabase = new PlayerDatabase();

            serverHost = new ServerHost();
            serverJoin = new ServerJoin();

            cardFunctionality.SetReferences(this, pnlMain);
            serverHost.SetReferences(this);
            serverJoin.SetReferences(this);
            playerDatabase.SetReferences(txtServerLog);

            chatBox.SetReferences(this, pctrChatBox);
            adminConsole.SetReferences(this);

            txtServerLog.AppendText($"Server Log: {Environment.NewLine}");
        }

        private void HostGame_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPortHost.Text) && !string.IsNullOrEmpty(txtUsername.Text))
            {
                if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 4)
                {
                    string username = txtUsername.Text;

                    if (int.TryParse(txtPortHost.Text, out int port))
                    {
                        HostGame(port, username);
                    }
                    else MessageBox.Show("Invalid Port!", "Error while hosting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Username must be between 4 and 24 characters long");
                }
            }
            else
            {
                MessageBox.Show("Please fill both Host Port and Username");
            }

        }
        private void JoinGame_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 2 && !txtUsername.Text.Contains(' '))
            {
                string username = txtUsername.Text;

                if (IPAddress.TryParse(txtIPAddressJoin.Text, out IPAddress ip))
                {
                    if (int.TryParse(txtPortJoin.Text, out int port) && port > 0 && port <= 65535)
                    {
                        JoinGame(ip, port, username);
                    }
                    else
                        MessageBox.Show("Invalid Port!", "Error while joining", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Invalid IPAddress!", "Error while joining", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Username must be between 3 and 24 characters long, and cannot contain whitespaces");
            }
        }
        CustomButton btnStartGame;
        CustomPictureBox pctrAdminConsole;
        private async void HostGame(int port, string username)
        {
            string response;
            using (HttpClient httpClient = new HttpClient())
            {
                string api = "https://ipinfo.io/ip";
                response = await httpClient.GetStringAsync(api);
            }
            AppendLogBox($"Hosting server on {response}:{port}");

            (Player, bool) hostServer = serverHost.HostServer(port, username);

            if (hostServer.Item2)
            {
                currentPlayer = hostServer.Item1;
                isHost = currentPlayer.IsHost;

                AppendLogBox("Server is running.");
                joinedOrHosted = true;

                chatBox.lblTitleExtern.Text = $"Chatbox - {response}:{port}";
                chatBox.Show();
                chatBox.Hide();


                btnStartGame = new CustomButton()
                {
                    Text = "Start the Game",
                    Enabled = false,
                    Parent = pnlMultiplayer,
                    Dock = DockStyle.Fill,
                };
                btnStartGame.Click += (sender, e) => StartGame();

                pnlMultiplayer.SetCellPosition(btnStartGame, new TableLayoutPanelCellPosition(6, 13));
                pnlMultiplayer.SetColumnSpan(btnStartGame, 3);
                pnlMultiplayer.SetRowSpan(btnStartGame, 2);

                pctrAdminConsole = new CustomPictureBox()
                {
                    Dock = DockStyle.Fill,
                    Parent = pnlMultiplayer,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Properties.Resources.User_Icon,
                };
                pctrAdminConsole.Click += (sender, e) => { adminConsole.Show(); };

                pnlMultiplayer.SetCellPosition(pctrAdminConsole, new TableLayoutPanelCellPosition(1,0));

                InitGUIForPlayers();

                btnHostServer.Enabled = false;
                txtPortHost.Enabled = false;
                txtUsername.Enabled = false;

                btnJoinServer.Enabled = false;
                txtIPAddressJoin.Enabled = false;
                txtPortJoin.Enabled = false;

            }
        }

        private void InitGUIForPlayers()
        {
            Image userIcon = Properties.Resources.User_Icon;

            CustomPictureBox player1PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pnlMultiplayer.SetCellPosition(player1PictureBox, new TableLayoutPanelCellPosition(1, 18));
            pnlMultiplayer.SetColumnSpan(player1PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player1PictureBox, 3);
            CustomLabel player1Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = currentPlayer.Name,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player1Label, new TableLayoutPanelCellPosition(1, 21));
            pnlMultiplayer.SetColumnSpan(player1Label, 2);

            CustomPictureBox player2PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pnlMultiplayer.SetCellPosition(player2PictureBox, new TableLayoutPanelCellPosition(4, 18));
            pnlMultiplayer.SetColumnSpan(player2PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player2PictureBox, 3);
            CustomLabel player2Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player2Label, new TableLayoutPanelCellPosition(4, 21));
            pnlMultiplayer.SetColumnSpan(player2Label, 2);

            CustomPictureBox player3PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.Zoom

            };
            pnlMultiplayer.SetCellPosition(player3PictureBox, new TableLayoutPanelCellPosition(7, 18));
            pnlMultiplayer.SetColumnSpan(player3PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player3PictureBox, 3);
            CustomLabel player3Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player3Label, new TableLayoutPanelCellPosition(7, 21));
            pnlMultiplayer.SetColumnSpan(player3Label, 2);

            CustomPictureBox player4PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pnlMultiplayer.SetCellPosition(player4PictureBox, new TableLayoutPanelCellPosition(10, 18));
            pnlMultiplayer.SetColumnSpan(player4PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player4PictureBox, 3);
            CustomLabel player4Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player4Label, new TableLayoutPanelCellPosition(10, 21));
            pnlMultiplayer.SetColumnSpan(player4Label, 2);

            playerLabels[0] = player1Label;
            playerLabels[1] = player2Label;
            playerLabels[2] = player3Label;
            playerLabels[3] = player4Label;
        }

        Label[] playerLabels = new Label[4];
        public void AddPlayerToGUI(int playerIndex, Player player)
        {
            playerLabels[playerIndex].Text = player.Name;
        }

        public void RemovePlayerFromGUI(int playerIndex)
        {
            playerLabels[playerIndex].Text = "Waiting...";

            int lastEmptyIndex = playerIndex;
            foreach (CustomLabel label in playerLabels)
            {
                if (label.Text != "Waiting...")
                   if (Array.IndexOf(playerLabels, label) > lastEmptyIndex)
                   {
                        playerLabels[lastEmptyIndex].Text = label.Text;
                        label.Text = "Waiting...";
                        lastEmptyIndex++;
                   }
            }
        }

        private async void JoinGame(IPAddress ip, int port, string username)
        {
            AppendLogBox($"Connecting to {ip}:{port} as {username}");

            (Player, bool) joinServer = await Task.Run(() => serverJoin.JoinGame(ip, port, username));

            if (joinServer.Item2)
            {
                currentPlayer = joinServer.Item1;

                joinedOrHosted = true;

                chatBox.lblTitleExtern.Text = $"Chatbox - {ip}:{port}";
                chatBox.Show();
                chatBox.Hide();

                btnHostServer.Enabled = false;
                txtPortHost.Enabled = false;
                txtUsername.Enabled = false;

                btnJoinServer.Enabled = false;
                txtIPAddressJoin.Enabled = false;
                txtPortJoin.Enabled = false;
            }
            else
            {
                AppendLogBox("Failed to join the server.");
            }
        }

        public void DisconnectedFromServer()
        {
            joinedOrHosted = false;
            isHost = false;
            chatBox.lblTitleExtern.Text = string.Empty;
            playerDatabase.players.Clear();

            btnHostServer.Enabled = true;
            txtPortHost.Enabled = true;
            txtUsername.Enabled = true;

            btnJoinServer.Enabled = true;
            txtIPAddressJoin.Enabled = true;
            txtPortJoin.Enabled = true;
        }

        public void AppendLogBox(string message)
        {
            CustomRichTextBox txtBox = txtServerLog;

            txtBox.AppendText(message + Environment.NewLine);
        }

        private void PctrChatBox_Click(object sender, EventArgs e)
        {
            if (joinedOrHosted)
            {
                chatBox.Show();
                chatBox.OpenChatBox();
            }
                
            else
                MessageBox.Show("You must Host or Join a server first.");
        }


        public void StartGameButtonState(bool state)
        {
            btnStartGame.Enabled = state;
        }
        private async void StartGame()
        {
            for (int i = 0; i < 8; i++)
            {
                foreach (Player player in playerDatabase.players)
                {
                    //Draw Card, Player, Int Card Count
                    cardFunctionality.DrawCardsFromDeck(player, 1);
                }
            }

            pctrChatBox.Parent = pnlMain;
            pctrAdminConsole.Parent = pnlMain;

            await serverHost.BroadcastData("START");

            pnlMain.Show();
            pnlMain.BringToFront();
            Application.DoEvents();
            pnlMultiplayer.Hide();
        }

        public void StartGameClient()
        {
            pctrChatBox.Parent = pnlMain;

            pnlMain.Show();
            pnlMain.BringToFront();
            Application.DoEvents();
            pnlMultiplayer.Hide();
        }
    }
}
