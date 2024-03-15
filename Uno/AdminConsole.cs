using System;
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

        frmUno form1;
        public CustomLabel lblTitleExtern;
        PlayerDatabase playerDatabase;
        Deck deck;

        public bool newMessage = false;

        ServerHost serverHost;

        bool shutdown = false;
        bool kick = false;

        public AdminConsole()
        {
            InitializeComponent();
            lblTitleExtern = lblTitle;
        }

        public void SetReferences(frmUno form1)
        {
            this.form1 = form1;
            this.playerDatabase = form1.playerDatabase;
            this.serverHost = form1.serverHost;
            this.deck = form1.deck;
        }

        private async void btnExecuteCommand_Click(object sender, EventArgs e)
        {
            string command = txtCommandInput.Text;
            AppendCommandBox(command, true);
            txtCommandInput.Text = string.Empty;

            //HELP
            if (command.ToLower().Trim() == ("help") || command.ToLower().Trim() == ("commands"))
            {
                AppendCommandBox("Commands: data, list, clear/cls, players, exit, shutdown");
            }
            //HELP + COMMAND
            else if (command.ToLower().Trim().StartsWith("help") && command.Split(' ').Length > 1)
            {
                string[] commands = command.Split(' ');
                if (commands[1] == "data")
                {
                    AppendCommandBox("Usage: data PLAYER COMMAND");
                    AppendCommandBox("Sends data to a player/client");
                    AppendCommandBox("Example: data player234 MSG hello friend");
                    AppendCommandBox("See ServerJoin class for all commands.");
                }
                else if (commands[1] == "list")
                {
                    AppendCommandBox("Usage: list (player[optional])");
                    AppendCommandBox("Lists all the cards with their IDs, Or lists the inventory of a player");
                }
                else if (commands[1] == "clear" || commands[1] == "cls")
                {
                    AppendCommandBox("Usage: clear");
                    AppendCommandBox("Usage: cls");
                    AppendCommandBox("Clears the console");
                }
                else if (commands[1] == "players")
                {
                    AppendCommandBox("Usage: players");
                    AppendCommandBox("Lists all the players");
                }
                else if (commands[1] == "exit")
                {
                    AppendCommandBox("Usage: exit");
                    AppendCommandBox("Closes the console");
                }
                else if (commands[1] == "shutdown")
                {
                    AppendCommandBox("Usage: shutdown");
                    AppendCommandBox("Closes the whole application.");
                }
            }
            //LIST
            else if (command.ToLower().Trim() == ("list"))
            {
                AppendCommandBox("All Cards:");
                string allCards = string.Empty;
                foreach (Card card in deck.playingDeck)
                {
                    allCards += $"ID: {card.ID}, Card: {card.Color} {card} || ";
                }
                AppendCommandBox(allCards);
            }
            //LIST + PLAYER
            else if (command.ToLower().Trim().StartsWith("list") && command.Split(' ').Length > 1)
            {
                string[] commands = command.Split(' ');
                if (playerDatabase.NamePlayerDictionary.TryGetValue(commands[1], out Player player))
                {
                    AppendCommandBox($"{commands[1]}'s Cards:");
                    string playerCards = string.Empty;
                    foreach (Card card in player.Inventory)
                    {
                        playerCards += $"ID: {card.ID}, Card: {card.Color} {card} || ";
                    }
                    AppendCommandBox(playerCards);
                    await serverHost.BroadcastData("CHEATS");
                }
                else
                {
                    AppendCommandBox($"Specified player \"{commands[1]}\" not found");
                }
            }
            //PLAYERS
            else if (command.ToLower().Trim() == ("players"))
            {
                string players = string.Empty;
                foreach (Player player in playerDatabase.players)
                {
                    players += "\"" + player.Name + "\"" + " ";
                }
                AppendCommandBox(players.Trim());
            }
            //DATA
            else if (command.ToLower().Trim().StartsWith("data"))
            {
                if (command.Split(' ').Length > 2)
                {
                    string[] commands = command.Split(' ');
                    if (playerDatabase.NamePlayerDictionary.TryGetValue(commands[1], out Player player))
                    {
                        playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);
                        serverHost.SendDataToSpecificClient(command.Substring(commands[0].Length + 1 + commands[1].Length + 1), client);
                    }
                    else
                    {
                        AppendCommandBox($"Specified player \"{commands[1]}\" not found");
                    }
                }
                else
                {
                    AppendCommandBox("Incorrect Usage: [data PLAYER DATA]");
                }
            }
            //CLEAR
            else if (command.ToLower().Trim() == ("clear") || command.ToLower().Trim() == ("cls"))
            {
                txtCommandLog.Clear();
            }
            //EXIT
            else if (command.ToLower().Trim() == "exit")
            {
                this.Hide();
            }
            //SHUTDOWN
            else if (command.ToLower().Trim() == ("shutdown"))
            {
                if (shutdown)
                    Application.Exit();
                else
                {
                    AppendCommandBox("[WARNING!], this will shutdown the server and kick everyone playing.");
                    AppendCommandBox("Type shutdown again to close the application");
                    shutdown = true;
                }
            }
            //KICK
            else if (command.ToLower().Trim().StartsWith("kick"))
            {
                if (command.Split(' ').Length > 1)
                {
                    if (form1.isStarted)
                    {
                        if (kick)
                        {
                            string[] commands = command.Split(' ');
                            playerDatabase.NamePlayerDictionary.TryGetValue(commands[1], out Player player);
                            playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);

                            serverHost.SendDataToSpecificClient("KICK", client);
                        }
                        else
                        {
                            AppendCommandBox("[WARNING!] Kicking a player while the game is started is going to terminate the server");
                            AppendCommandBox("Type \"kick\" again in order to kick");
                            kick = true;
                        }

                    }
                    else
                    {
                        string[] commands = command.Split(' ');
                        playerDatabase.NamePlayerDictionary.TryGetValue(commands[1], out Player player);
                        playerDatabase.PlayerClientDictionary.TryGetValue(player, out TcpClient client);

                        serverHost.SendDataToSpecificClient("KICK", client);
                    }

                }
                else
                {
                    AppendCommandBox("Incorrect usage: [kick PLAYER]");
                }

            }
            else
            {
                AppendCommandBox($"Command {command} not found. Use \"help\" to list all commands");
            }

            shutdown = (command.ToLower().Trim() != "shutdown") ? false : shutdown;
            kick = (!command.ToLower().Trim().StartsWith("kick")) ? false : kick;
        }

        public void AppendCommandBox(string message, bool commandInput = false)
        {

            CustomRichTextBox txtBox = txtCommandLog;

            txtBox.SelectionStart = txtBox.Text.Length;
            txtBox.SelectionLength = 0;

            if (commandInput)
            {
                txtBox.AppendText(Environment.NewLine);
                txtBox.SelectionColor = Color.Red;
                txtBox.AppendText(form1.currentPlayer.Name + ": ");
            }

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

        private void txtCommandInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                btnExecuteCommand_Click(sender, e);
            }
        }
    }
}
