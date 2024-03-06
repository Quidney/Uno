namespace Uno
{
    partial class frmUno
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.btnDrawCard = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.pnlMultiplayer = new System.Windows.Forms.TableLayoutPanel();
            this.timerTurn = new System.Windows.Forms.Timer(this.components);
            this.lblUsername = new Uno.Classes.CustomLabel();
            this.txtUsername = new Uno.Classes.CustomTextBox();
            this.lblIPAdressJoin = new Uno.Classes.CustomLabel();
            this.txtIPAddressJoin = new Uno.Classes.CustomTextBox();
            this.lblPortJoin = new Uno.Classes.CustomLabel();
            this.txtPortJoin = new Uno.Classes.CustomTextBox();
            this.lblPortHost = new Uno.Classes.CustomLabel();
            this.txtPortHost = new Uno.Classes.CustomTextBox();
            this.btnJoinServer = new Uno.Classes.CustomButton();
            this.btnHostServer = new Uno.Classes.CustomButton();
            this.txtServerLog = new Uno.Classes.CustomTextBox();
            this.lblJoin = new Uno.Classes.CustomLabel();
            this.lblHost = new Uno.Classes.CustomLabel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlMain.SuspendLayout();
            this.pnlMultiplayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.ColumnCount = 25;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.Controls.Add(this.btnStartGame, 11, 14);
            this.pnlMain.Controls.Add(this.btnDrawCard, 15, 14);
            this.pnlMain.Controls.Add(this.lblTimer, 2, 21);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 25;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlMain.Size = new System.Drawing.Size(1600, 1055);
            this.pnlMain.TabIndex = 0;
            // 
            // btnStartGame
            // 
            this.pnlMain.SetColumnSpan(this.btnStartGame, 3);
            this.btnStartGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartGame.Location = new System.Drawing.Point(708, 592);
            this.btnStartGame.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartGame.Name = "btnStartGame";
            this.pnlMain.SetRowSpan(this.btnStartGame, 3);
            this.btnStartGame.Size = new System.Drawing.Size(184, 118);
            this.btnStartGame.TabIndex = 7;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.BtnStartGame_Click);
            // 
            // btnDrawCard
            // 
            this.pnlMain.SetColumnSpan(this.btnDrawCard, 3);
            this.btnDrawCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawCard.Location = new System.Drawing.Point(964, 592);
            this.btnDrawCard.Margin = new System.Windows.Forms.Padding(4);
            this.btnDrawCard.Name = "btnDrawCard";
            this.pnlMain.SetRowSpan(this.btnDrawCard, 3);
            this.btnDrawCard.Size = new System.Drawing.Size(184, 118);
            this.btnDrawCard.TabIndex = 8;
            this.btnDrawCard.Text = "Draw Card";
            this.btnDrawCard.UseVisualStyleBackColor = true;
            this.btnDrawCard.Click += new System.EventHandler(this.DrawCard);
            // 
            // lblTimer
            // 
            this.pnlMain.SetColumnSpan(this.lblTimer, 3);
            this.lblTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.Location = new System.Drawing.Point(131, 882);
            this.lblTimer.Name = "lblTimer";
            this.pnlMain.SetRowSpan(this.lblTimer, 2);
            this.lblTimer.Size = new System.Drawing.Size(186, 84);
            this.lblTimer.TabIndex = 9;
            this.lblTimer.Text = "Timer";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTimer.Visible = false;
            // 
            // pnlMultiplayer
            // 
            this.pnlMultiplayer.ColumnCount = 25;
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.Controls.Add(this.lblUsername, 6, 2);
            this.pnlMultiplayer.Controls.Add(this.txtUsername, 6, 3);
            this.pnlMultiplayer.Controls.Add(this.lblIPAdressJoin, 1, 5);
            this.pnlMultiplayer.Controls.Add(this.txtIPAddressJoin, 1, 6);
            this.pnlMultiplayer.Controls.Add(this.lblPortJoin, 1, 7);
            this.pnlMultiplayer.Controls.Add(this.txtPortJoin, 1, 8);
            this.pnlMultiplayer.Controls.Add(this.lblPortHost, 11, 5);
            this.pnlMultiplayer.Controls.Add(this.txtPortHost, 11, 6);
            this.pnlMultiplayer.Controls.Add(this.btnJoinServer, 1, 9);
            this.pnlMultiplayer.Controls.Add(this.btnHostServer, 11, 7);
            this.pnlMultiplayer.Controls.Add(this.txtServerLog, 17, 1);
            this.pnlMultiplayer.Controls.Add(this.lblJoin, 1, 4);
            this.pnlMultiplayer.Controls.Add(this.lblHost, 11, 4);
            this.pnlMultiplayer.Controls.Add(this.pbLogo, 4, 13);
            this.pnlMultiplayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMultiplayer.Location = new System.Drawing.Point(0, 0);
            this.pnlMultiplayer.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMultiplayer.Name = "pnlMultiplayer";
            this.pnlMultiplayer.RowCount = 25;
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlMultiplayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlMultiplayer.Size = new System.Drawing.Size(1600, 1055);
            this.pnlMultiplayer.TabIndex = 9;
            // 
            // lblUsername
            // 
            this.lblUsername.AssignedCard = null;
            this.lblUsername.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblUsername, 5);
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsername.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(309, 66);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(249, 33);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtUsername
            // 
            this.pnlMultiplayer.SetColumnSpan(this.txtUsername, 5);
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(309, 102);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(249, 40);
            this.txtUsername.TabIndex = 4;
            // 
            // lblIPAdressJoin
            // 
            this.lblIPAdressJoin.AssignedCard = null;
            this.lblIPAdressJoin.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblIPAdressJoin, 4);
            this.lblIPAdressJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIPAdressJoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAdressJoin.Location = new System.Drawing.Point(54, 165);
            this.lblIPAdressJoin.Name = "lblIPAdressJoin";
            this.lblIPAdressJoin.Size = new System.Drawing.Size(198, 33);
            this.lblIPAdressJoin.TabIndex = 5;
            this.lblIPAdressJoin.Text = "Ip-Address of Server";
            this.lblIPAdressJoin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtIPAddressJoin
            // 
            this.pnlMultiplayer.SetColumnSpan(this.txtIPAddressJoin, 5);
            this.txtIPAddressJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPAddressJoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIPAddressJoin.Location = new System.Drawing.Point(54, 201);
            this.txtIPAddressJoin.Name = "txtIPAddressJoin";
            this.txtIPAddressJoin.Size = new System.Drawing.Size(249, 40);
            this.txtIPAddressJoin.TabIndex = 2;
            // 
            // lblPortJoin
            // 
            this.timerTurn.Interval = 1000;
            this.timerTurn.Tick += new System.EventHandler(this.timerTurn_Tick);
            this.lblPortJoin.AssignedCard = null;
            this.lblPortJoin.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblPortJoin, 4);
            this.lblPortJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPortJoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortJoin.Location = new System.Drawing.Point(54, 231);
            this.lblPortJoin.Name = "lblPortJoin";
            this.lblPortJoin.Size = new System.Drawing.Size(198, 33);
            this.lblPortJoin.TabIndex = 7;
            this.lblPortJoin.Text = "Port of Server";
            this.lblPortJoin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPortJoin
            // 
            this.pnlMultiplayer.SetColumnSpan(this.txtPortJoin, 5);
            this.txtPortJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPortJoin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortJoin.Location = new System.Drawing.Point(54, 267);
            this.txtPortJoin.MaxLength = 5;
            this.txtPortJoin.Name = "txtPortJoin";
            this.txtPortJoin.Size = new System.Drawing.Size(249, 40);
            this.txtPortJoin.TabIndex = 6;
            this.txtPortJoin.TextChanged += new System.EventHandler(this.LimitPortInput_MaxLimit);
            this.txtPortJoin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LimitPortInput);
            // 
            // lblPortHost
            // 
            this.lblPortHost.AssignedCard = null;
            this.lblPortHost.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblPortHost, 4);
            this.lblPortHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPortHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortHost.Location = new System.Drawing.Point(564, 165);
            this.lblPortHost.Name = "lblPortHost";
            this.lblPortHost.Size = new System.Drawing.Size(198, 33);
            this.lblPortHost.TabIndex = 8;
            this.lblPortHost.Text = "Port to Host";
            this.lblPortHost.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPortHost
            // 
            this.pnlMultiplayer.SetColumnSpan(this.txtPortHost, 5);
            this.txtPortHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPortHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortHost.Location = new System.Drawing.Point(564, 201);
            this.txtPortHost.MaxLength = 5;
            this.txtPortHost.Name = "txtPortHost";
            this.txtPortHost.Size = new System.Drawing.Size(249, 40);
            this.txtPortHost.TabIndex = 9;
            this.txtPortHost.TextChanged += new System.EventHandler(this.LimitPortInput_MaxLimit);
            this.txtPortHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LimitPortInput);
            // 
            // btnJoinServer
            // 
            this.pnlMultiplayer.SetColumnSpan(this.btnJoinServer, 3);
            this.btnJoinServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJoinServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJoinServer.Location = new System.Drawing.Point(54, 300);
            this.btnJoinServer.Name = "btnJoinServer";
            this.pnlMultiplayer.SetRowSpan(this.btnJoinServer, 2);
            this.btnJoinServer.Size = new System.Drawing.Size(147, 60);
            this.btnJoinServer.TabIndex = 0;
            this.btnJoinServer.Text = "Join";
            this.btnJoinServer.UseVisualStyleBackColor = true;
            this.btnJoinServer.Click += new System.EventHandler(this.JoinGame);
            // 
            // btnHostServer
            // 
            this.pnlMultiplayer.SetColumnSpan(this.btnHostServer, 3);
            this.btnHostServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHostServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHostServer.Location = new System.Drawing.Point(564, 234);
            this.btnHostServer.Name = "btnHostServer";
            this.pnlMultiplayer.SetRowSpan(this.btnHostServer, 2);
            this.btnHostServer.Size = new System.Drawing.Size(147, 60);
            this.btnHostServer.TabIndex = 1;
            this.btnHostServer.Text = "Host";
            this.btnHostServer.UseVisualStyleBackColor = true;
            this.btnHostServer.Click += new System.EventHandler(this.HostGame);
            // 
            // txtServerLog
            // 
            this.txtServerLog.BackColor = System.Drawing.Color.Red;
            this.pnlMultiplayer.SetColumnSpan(this.txtServerLog, 7);
            this.txtServerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServerLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerLog.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.txtServerLog.Location = new System.Drawing.Point(870, 36);
            this.txtServerLog.Multiline = true;
            this.txtServerLog.Name = "txtServerLog";
            this.txtServerLog.ReadOnly = true;
            this.pnlMultiplayer.SetRowSpan(this.txtServerLog, 23);
            this.txtServerLog.Size = new System.Drawing.Size(351, 753);
            this.txtServerLog.TabIndex = 10;
            // 
            // lblJoin
            // 
            this.lblJoin.AssignedCard = null;
            this.lblJoin.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblJoin, 4);
            this.lblJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJoin.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJoin.Location = new System.Drawing.Point(54, 132);
            this.lblJoin.Name = "lblJoin";
            this.lblJoin.Size = new System.Drawing.Size(198, 33);
            this.lblJoin.TabIndex = 11;
            this.lblJoin.Text = "JOIN";
            this.lblJoin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHost
            // 
            this.lblHost.AssignedCard = null;
            this.lblHost.AutoSize = true;
            this.pnlMultiplayer.SetColumnSpan(this.lblHost, 4);
            this.lblHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHost.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHost.Location = new System.Drawing.Point(564, 132);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(198, 33);
            this.lblHost.TabIndex = 12;
            this.lblHost.Text = "HOST";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbLogo
            // 
            this.pnlMultiplayer.SetColumnSpan(this.pbLogo, 9);
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLogo.Image = global::Uno.Properties.Resources.UNO_Logo_svg;
            this.pbLogo.Location = new System.Drawing.Point(207, 432);
            this.pbLogo.Name = "pbLogo";
            this.pnlMultiplayer.SetRowSpan(this.pbLogo, 11);
            this.pbLogo.Size = new System.Drawing.Size(453, 357);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 13;
            this.pbLogo.TabStop = false;
            // 
            // frmUno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(1280, 844);
            this.ClientSize = new System.Drawing.Size(1600, 1055);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlMultiplayer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmUno";
            this.Text = "Uno!";
            this.pnlMain.ResumeLayout(false);
            this.pnlMultiplayer.ResumeLayout(false);
            this.pnlMultiplayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Button btnDrawCard;
        private System.Windows.Forms.TableLayoutPanel pnlMultiplayer;
        private Uno.Classes.CustomButton btnJoinServer;
        private Uno.Classes.CustomButton btnHostServer;
        private Classes.CustomTextBox txtIPAddressJoin;
        private Classes.CustomLabel lblUsername;
        private Classes.CustomTextBox txtUsername;
        private Classes.CustomLabel lblIPAdressJoin;
        private Classes.CustomLabel lblPortJoin;
        private Classes.CustomTextBox txtPortJoin;
        private Classes.CustomTextBox txtPortHost;
        private Classes.CustomLabel lblPortHost;
        private Classes.CustomTextBox txtServerLog;
        private Classes.CustomLabel lblJoin;
        private Classes.CustomLabel lblHost;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Timer timerTurn;
        private System.Windows.Forms.PictureBox pbLogo;
    }
}

