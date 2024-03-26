using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uno.Class;
using Uno.Classes;
using Uno.Properties;

namespace Uno
{
    public partial class frmUno : Form
    {
        Random random = new Random();

        public Player currentPlayer;
        public bool joinedOrHosted = false;
        public bool isHost = false;
        public bool isStarted = false;
        public bool shuttingDown = false;

        public Deck deck;
        public CardFunctionality cardFunctionality;
        public PlayerDatabase playerDatabase;
        public ServerHost serverHost;
        public ServerJoin serverJoin;

        public ChatBox chatBox;
        public AdminConsole adminConsole;

        public Card lastCardPlayed;

        public bool myTurn = false;

        public int seconden { get; set; }
        public bool YourTurn { get; set; }
        public bool OtherTurn { get; set; }

        public frmUno()
        {
            InitializeComponent();
            InitMethod();
            pnlMain.Paint += set_background;
            pnlMultiplayer.Paint += set_background;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();

        }

        void InitMethod()
        {
            adminConsole = new AdminConsole();
            chatBox = new ChatBox();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Uno!";

            deck = new Deck();
            deck.InitDeck();

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

            pnlMultiplayer.Show();
            pnlMultiplayer.BringToFront();
            pnlMain.Hide();
        }

        private void HostGame_Click(object sender, EventArgs e)
        {
#if DEBUG
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.Text = "User" + random.Next(0, 1000);
            }
            if (string.IsNullOrEmpty(txtPortHost.Text)) txtPortHost.Text = "5565";
#endif
            if (!string.IsNullOrEmpty(txtPortHost.Text) && !string.IsNullOrEmpty(txtUsername.Text))
            {
                if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 2 && !txtUsername.Text.Contains(' '))
                {
                    string username = txtUsername.Text.Trim();

                    if (int.TryParse(txtPortHost.Text.Trim(), out int port))
                    {
                        HostGame(port, username);
                    }
                    else MessageBox.Show("Invalid Port!", "Error while hosting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Username must be between 4 and 24 characters long", "Error while hosting");
                }
            }
            else
            {
                MessageBox.Show("Please fill both Host Port and Username", "Error while hosting.");
            }

        }
        private void JoinGame_Click(object sender, EventArgs e)
        {
#if DEBUG
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.Text = "User" + random.Next(0, 1000);
            }
            if (string.IsNullOrEmpty(txtIPAddressJoin.Text)) txtIPAddressJoin.Text = "127.0.0.1";
            if (string.IsNullOrEmpty(txtPortJoin.Text)) txtPortJoin.Text = "5565";
#endif
            if (txtUsername.Text.Length <= 24 && txtUsername.Text.Length > 2 && !txtUsername.Text.Contains(' '))
            {
                string username = txtUsername.Text.Trim();

                if (IPAddress.TryParse(txtIPAddressJoin.Text.Trim(), out IPAddress ip))
                {
                    if (int.TryParse(txtPortJoin.Text.Trim(), out int port) && port > 0 && port <= 65535)
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
                MessageBox.Show("Username must be between 3 and 24 characters long, and cannot contain whitespaces", "Error While joining");
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

                chatBox.lblTitleExtern.Text = $"       Chatbox - {currentPlayer.Name} - {response}:{port}";
                chatBox.lblTitleExtern.Image = Resources.Chat;
                chatBox.lblTitleExtern.ImageAlign = ContentAlignment.MiddleLeft;
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

                pnlMultiplayer.SetCellPosition(btnStartGame, new TableLayoutPanelCellPosition(7, 13));
                pnlMultiplayer.SetColumnSpan(btnStartGame, 3);
                pnlMultiplayer.SetRowSpan(btnStartGame, 2);

                pctrAdminConsole = new CustomPictureBox()
                {
                    Dock = DockStyle.Fill,
                    Parent = pnlMultiplayer,
                    SizeMode = PictureBoxSizeMode.CenterImage,
                    Image = Properties.Resources.Terminal,
                    BorderStyle = BorderStyle.FixedSingle
                };
                adminConsole.lblTitleExtern.Text = $"       Admin Console - {currentPlayer.Name}";
                adminConsole.lblTitleExtern.Image = Properties.Resources.Terminal;
                adminConsole.lblTitleExtern.ImageAlign = ContentAlignment.MiddleLeft;
                pctrAdminConsole.Click += (sender, e) => { adminConsole.Show(); };

                pnlMultiplayer.SetCellPosition(pctrAdminConsole, new TableLayoutPanelCellPosition(2, 0));
                pnlMultiplayer.SetColumnSpan(pctrAdminConsole, 2);
                pnlMultiplayer.SetRowSpan(pctrAdminConsole, 2);

                InitGUIForPlayers();

                btnHostServer.Enabled = false;
                txtPortHost.Enabled = false;
                txtUsername.Enabled = false;

                btnJoinServer.Enabled = false;
                txtIPAddressJoin.Enabled = false;
                txtPortJoin.Enabled = false;

                shuttingDown = false;
            }
        }
        readonly CustomLabel[] playerLabels = new CustomLabel[4];
        readonly CustomPictureBox[] playerPictures = new CustomPictureBox[4];
        private void InitGUIForPlayers()
        {
            Image userIcon = Resources.UserIcon64px;

            CustomPictureBox player1PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            pnlMultiplayer.SetCellPosition(player1PictureBox, new TableLayoutPanelCellPosition(3, 18));
            pnlMultiplayer.SetColumnSpan(player1PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player1PictureBox, 3);
            CustomLabel player1Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = currentPlayer.Name,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player1Label, new TableLayoutPanelCellPosition(3, 21));
            pnlMultiplayer.SetColumnSpan(player1Label, 2);

            CustomPictureBox player2PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            pnlMultiplayer.SetCellPosition(player2PictureBox, new TableLayoutPanelCellPosition(6, 18));
            pnlMultiplayer.SetColumnSpan(player2PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player2PictureBox, 3);
            CustomLabel player2Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player2Label, new TableLayoutPanelCellPosition(6, 21));
            pnlMultiplayer.SetColumnSpan(player2Label, 2);

            CustomPictureBox player3PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.CenterImage

            };
            pnlMultiplayer.SetCellPosition(player3PictureBox, new TableLayoutPanelCellPosition(9, 18));
            pnlMultiplayer.SetColumnSpan(player3PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player3PictureBox, 3);
            CustomLabel player3Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player3Label, new TableLayoutPanelCellPosition(9, 21));
            pnlMultiplayer.SetColumnSpan(player3Label, 2);

            CustomPictureBox player4PictureBox = new CustomPictureBox()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Image = userIcon,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            pnlMultiplayer.SetCellPosition(player4PictureBox, new TableLayoutPanelCellPosition(12, 18));
            pnlMultiplayer.SetColumnSpan(player4PictureBox, 2);
            pnlMultiplayer.SetRowSpan(player4PictureBox, 3);
            CustomLabel player4Label = new CustomLabel()
            {
                Parent = pnlMultiplayer,
                Dock = DockStyle.Fill,
                Text = "Waiting...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMultiplayer.SetCellPosition(player4Label, new TableLayoutPanelCellPosition(12, 21));
            pnlMultiplayer.SetColumnSpan(player4Label, 2);

            playerLabels[0] = player1Label;
            playerLabels[1] = player2Label;
            playerLabels[2] = player3Label;
            playerLabels[3] = player4Label;

            playerPictures[0] = player1PictureBox;
            playerPictures[1] = player2PictureBox;
            playerPictures[2] = player3PictureBox;
            playerPictures[3] = player4PictureBox;
        }

        public void AddPlayerToGUI(int playerIndex, Player player)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => playerLabels[playerIndex].Text = player.Name));
            else
                playerLabels[playerIndex].Text = player.Name;
        }

        public void RemovePlayerFromGUI(int playerIndex)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
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
                }));
            }
            else
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
        }

        private async void JoinGame(IPAddress ip, int port, string username)
        {
            AppendLogBox($"Connecting to {ip}:{port} as {username}");

            (Player, bool) joinServer = await Task.Run(() => serverJoin.JoinGame(ip, port, username));

            if (joinServer.Item2)
            {
                currentPlayer = joinServer.Item1;

                joinedOrHosted = true;

                chatBox.lblTitleExtern.Text = $"       Chatbox - {currentPlayer.Name} - {ip}:{port}";
                chatBox.lblTitleExtern.Image = Resources.Chat;
                chatBox.lblTitleExtern.ImageAlign = ContentAlignment.MiddleLeft;
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

        public void DisconnectedFromServerClient()
        {
            joinedOrHosted = false;
            isHost = false;
            chatBox.lblTitleExtern.Text = string.Empty;
            playerDatabase.RemovePlayer(currentPlayer);
            playerDatabase.players.Clear();

            btnHostServer.Enabled = true;
            txtPortHost.Enabled = true;
            txtUsername.Enabled = true;

            btnJoinServer.Enabled = true;
            txtIPAddressJoin.Enabled = true;
            txtPortJoin.Enabled = true;

            pctrChatBox.Parent = pnlMultiplayer;
            pnlMultiplayer.SetCellPosition(pctrChatBox, new TableLayoutPanelCellPosition(0, 0));

            pnlMultiplayer.Show();
            pnlMultiplayer.BringToFront();
            Application.DoEvents();
            pnlMain.Hide();
        }

        public async void DisconnectedFromServerHost()
        {
            joinedOrHosted = false;
            isHost = false;
            chatBox.lblTitleExtern.Text = string.Empty;
            chatBox.Hide();

            shuttingDown = true;
            await serverHost.BroadcastData("KICK");

            Player[] players = new Player[playerDatabase.players.Count];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = playerDatabase.players[i];
            }
            foreach (Player player in players)
            {
                playerDatabase.RemovePlayer(player);
            }
            foreach (CustomLabel label in playerLabels)
            {
                label.Dispose();
            }
            foreach (CustomPictureBox picBox in playerPictures)
            {
                picBox.Dispose();
            }

            serverHost.clients.Clear();

            btnHostServer.Enabled = true;
            txtPortHost.Enabled = true;
            txtUsername.Enabled = true;

            btnJoinServer.Enabled = true;
            txtIPAddressJoin.Enabled = true;
            txtPortJoin.Enabled = true;

            pctrChatBox.Parent = pnlMultiplayer;
            pnlMultiplayer.SetCellPosition(pctrChatBox, new TableLayoutPanelCellPosition(0, 0));

            pnlMultiplayer.Show();
            pnlMultiplayer.BringToFront();
            Application.DoEvents();
            pnlMain.Hide();

            pctrAdminConsole.Dispose();
            btnStartGame.Dispose();

            isStarted = false;

            AppendLogBox("Server has been closed because a player has disconnected");
        }

        public void AppendLogBox(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    CustomRichTextBox txtBox = txtServerLog;
                    txtBox.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                CustomRichTextBox txtBox = txtServerLog;
                txtBox.AppendText(message + Environment.NewLine);
            }
        }

        private void PctrChatBox_Click(object sender, EventArgs e)
        {
            if (joinedOrHosted)
            {
                if (chatBox == null || chatBox.IsDisposed)
                {
                    chatBox = new ChatBox();
                    chatBox.SetReferences(this, pctrChatBox);

                    chatBox.lblTitleExtern.Text = $"       Chatbox - {currentPlayer.Name}";
                    chatBox.lblTitleExtern.Image = Resources.Chat;
                    chatBox.lblTitleExtern.ImageAlign = ContentAlignment.MiddleLeft;
                }
                chatBox.Show();
                chatBox.OpenChatBox();
            }
            else
                MessageBox.Show("You must Host or Join a server first.", "ChatBox");
        }


        public void StartGameButtonState(bool state)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { btnStartGame.Enabled = state; }));
            else
                btnStartGame.Enabled = state;

        }
        private async void StartGame()
        {
            await deck.Shuffle();

            do
            {
                lastCardPlayed = deck.playingDeck.LastOrDefault();
                deck.playingDeck.Remove(lastCardPlayed);
            } while (lastCardPlayed.Color == Card.ColorEnum.Black);

            cardFunctionality.currentColor = lastCardPlayed.Color;
            await serverHost.BroadcastData($"PILE {lastCardPlayed.ID}");

            int startingPlayer = random.Next(0, playerDatabase.players.Count);
            if (playerDatabase.players[startingPlayer] != currentPlayer)
            {
                playerDatabase.PlayerClientDictionary.TryGetValue(playerDatabase.players[startingPlayer], out TcpClient client);
                serverHost.SendDataToSpecificClient("TURN", client);
            }
            else
            {
                cardFunctionality.canPlay = true;
                Text += " YOUR TURN!!!";
            }

            for (int i = 0; i < 7; i++)
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

            isStarted = true;

            SetInventoryGUI();
        }

        public void StartGameClient()
        {
            pctrChatBox.Parent = pnlMain;

            pnlMain.Show();
            pnlMain.BringToFront();
            Application.DoEvents();
            pnlMultiplayer.Hide();

            SetInventoryGUI();
        }

        CustomTableLayoutPanel pnlInventory;
        CustomLabel cardOnTopPile;
        CustomPictureBox deckToDrawFrom;
        public void SetInventoryGUI()
        {
            pnlInventory?.Dispose();
            cardOnTopPile?.Dispose();
            deckToDrawFrom?.Dispose();

            pnlInventory = new CustomTableLayoutPanel() { Dock = DockStyle.Fill, Parent = pnlMain, ColumnCount = 1, RowCount = 1 };
            pnlMain.SetColumnSpan(pnlInventory, pnlMain.ColumnCount - 2);
            pnlMain.SetRowSpan(pnlInventory, 3);
            pnlMain.SetRow(pnlInventory, 16);
            pnlMain.SetColumn(pnlInventory, 1);

            cardOnTopPile = new CustomLabel() { Dock = DockStyle.Fill, Parent = pnlMain };
            pnlMain.SetColumnSpan(cardOnTopPile, 2);
            pnlMain.SetRowSpan(cardOnTopPile, 6);
            pnlMain.SetCellPosition(cardOnTopPile, new TableLayoutPanelCellPosition(11, 8));

            deckToDrawFrom = new CustomPictureBox() { Dock = DockStyle.Fill, Parent = pnlMain };
            deckToDrawFrom.SizeMode = PictureBoxSizeMode.Zoom;
            pnlMain.SetColumnSpan(deckToDrawFrom, 2);
            pnlMain.SetRowSpan(deckToDrawFrom, 2);
            pnlMain.SetCellPosition(deckToDrawFrom, new TableLayoutPanelCellPosition(15, 10));


            UpdateInventoryGUI();
        }
        public void UpdateInventoryGUI()
        {
            foreach (Control control in pnlInventory.Controls)
            {
                control.Dispose();
            }

            Image originalImageTopPile = lastCardPlayed.Image;
            int widthTopPileCard = 57;
            int heightTopPileCard = 86;

            Image resizedImageTopPile = new Bitmap(originalImageTopPile, new Size(widthTopPileCard, heightTopPileCard));

            cardOnTopPile.Image = resizedImageTopPile;

            if (lastCardPlayed.ToColor() == Color.Black)
                cardOnTopPile.ForeColor = Color.White;

            if (deck.playingDeck.Count > 0)
            {
                Image originalImageDeck = Resources.backCard;
                int widthDeckCard = 57;
                int heightDeckCard = 86;

                Image resizedImageDeck = new Bitmap(originalImageDeck, new Size(widthDeckCard, heightDeckCard));

                deckToDrawFrom.Image = resizedImageDeck;
            }
            else
                deckToDrawFrom.Image = Resources.Terminal;

            deckToDrawFrom.Click += async (sender, e) =>
            {
                string message = $"DECK {currentPlayer.Name}";

                if (!currentPlayer.IsHost)
                {
                    if (cardFunctionality.canPlay)
                        await serverJoin.SendDataToServer(message);
                }
                else
                {
                    if (cardFunctionality.canPlay)
                        cardFunctionality.DrawCardsFromDeck(currentPlayer, 1);
                }
                SetInventoryGUI();
            };

            List<Card> inventory = currentPlayer.Inventory;
            pnlInventory.ColumnCount = 1;
            pnlInventory.RowCount = 1;
            for (int i = 0; i < inventory.Count; i++)
            {

                Image originalImageInventory = inventory[i].Image;
                int widthInventoryCard = 57;
                int heightInventoryCard = 86;

                Image resizedImageInventory = new Bitmap(originalImageInventory, new Size(widthInventoryCard, heightInventoryCard));

                pnlInventory.ColumnCount++;
                CustomLabel cardlabel = new CustomLabel() { Dock = DockStyle.Fill, Parent = pnlInventory, Text = $" ", Image = resizedImageInventory, Tag = inventory[i].ID };

                int cardID = (int)cardlabel.Tag;
                cardlabel.Click += async (sender, e) =>
                {
                    string message = $"PLAY {currentPlayer.Name} {cardID}";
                    deck.idToCard.TryGetValue(cardID, out Card cardCard);

                    if (cardFunctionality.ThrowCardInPile(cardCard, currentPlayer))
                    {
                        if (currentPlayer.IsHost)
                        {
                            await serverHost.BroadcastData(message);
                        }
                        else
                        {
                            await serverJoin.SendDataToServer(message);
                        }
                    }
                    SetInventoryGUI();
                };
            }
            pnlInventory.ColumnCount--;

            pnlInventory.ColumnStyles.Clear();
            for (int i = 0; i < pnlInventory.ColumnCount; i++)
            {
                pnlInventory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / pnlInventory.ColumnCount));
            }
        }

        private void set_background(Object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Rectangle gradient_rectangle = new Rectangle(0, 0, Width, Height);

            Brush b = new LinearGradientBrush(gradient_rectangle, Color.FromArgb(255, 70, 70), Color.FromArgb(176, 0, 0), 65f);

            graphics.FillRectangle(b, gradient_rectangle);
        }

        private void enterHover(object sender, EventArgs e)
        {

        }

        private void leaveHover(object sender, EventArgs e)
        {

        }
    }
}
